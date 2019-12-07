using BlazorApp.Models;
using BlazorApp.Server.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DatabaseController : ControllerBase
    {
        private readonly BlazorContext _context;

        public DatabaseController(BlazorContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        [HttpGet]
        public IActionResult GetDatabaseRecords()
        {
            var records = _context.DatabaseRecords.ToArray();
            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDatabaseRecordAsync(int id)
        {
            var record = await _context.DatabaseRecords.FindAsync(id);
            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> PostDatabaseRecordAsync(DatabaseRecord record)
        {
            await _context.DatabaseRecords.AddAsync(record);
            await _context.SaveChangesAsync();
            return Ok(record);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveDatabaseRecordAsync(DatabaseRecord record)
        {
            _context.DatabaseRecords.Remove(record);
            await _context.SaveChangesAsync();
            return Ok(record);
        }

        [HttpPost("flash")]
        [Authorize(Policy = Policies.IsAdmin)]
        public async Task<IActionResult> PostFlashDatabaseAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
            return Ok();
        }
    }
}
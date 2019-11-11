using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorApp.Models
{
    public class DatabaseRecord
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Message { get; set; }
    }
}

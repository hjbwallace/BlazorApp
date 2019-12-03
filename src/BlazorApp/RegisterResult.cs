using System.Collections.Generic;

namespace BlazorApp
{
    public class RegisterResult
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; } = new string[0];

        public static RegisterResult Fail(IEnumerable<string> errors)
        {
            return new RegisterResult
            {
                Successful = false,
                Errors = errors
            };
        }

        public static RegisterResult Success()
        {
            return new RegisterResult
            {
                Successful = true,
            };
        }
    }
}
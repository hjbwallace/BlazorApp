using System;

namespace BlazorApp
{
    public class LoginResult
    {
        public bool Successful { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
        public DateTime? TokenExpiry { get; set; }

        public static LoginResult Fail(string error)
        {
            return new LoginResult
            {
                Successful = false,
                Error = error,
                Token = null,
                TokenExpiry = null
            };
        }

        public static LoginResult Success(string token, DateTime tokenExpiry)
        {
            return new LoginResult
            {
                Successful = true,
                Error = null,
                Token = token,
                TokenExpiry = tokenExpiry
            };
        }
    }
}
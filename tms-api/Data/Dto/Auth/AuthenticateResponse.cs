using Data.Extensions;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Data.Dto.Auth
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse(UserForReturnLogin user, string token, string refreshToken)
        {
            this.User = user;
            this.Token = token;
            this.RefreshToken = refreshToken;
        }

        public UserForReturnLogin User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

    }
}

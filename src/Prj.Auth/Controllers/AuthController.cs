using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Prj.Auth.Controllers
{
    [Produces("application/json")]
    public class AuthController : Controller
    {
        private const string KEY = "SymmKey-2017-0123456789";

        [HttpPost]
        [Route("auth")]
        public IActionResult Auth([FromBody] AuthRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest();

            var expiration = DateTime.UtcNow.AddMonths(1);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, request.Email),
                new Claim(JwtRegisteredClaimNames.Exp, expiration.ToString("O")),
                new Claim(JwtRegisteredClaimNames.Aud, "http://localhost:9002"),
                new Claim(JwtRegisteredClaimNames.Iss, "PrjAuth"),
                new Claim("rol", "PortalUser")
            };

            var token = new JwtSecurityToken(
                issuer: "http://localhost:9001",
                audience: "http://localhost:9002",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiration,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY)), SecurityAlgorithms.HmacSha256)
            );

            return Ok(new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }
    }

    public class AuthRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
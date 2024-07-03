using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using superecommere.Models.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace superecommere.Services
{
    public class JWTService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<TblUser> _userManager;
        private readonly SymmetricSecurityKey _jwtKey;
        public JWTService(IConfiguration config,UserManager<TblUser> userManager) { 
            _config = config;
            _userManager = userManager;
            _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtConfig:secret"]));

        }
        public async Task<string> CreateJWT(TblUser user)
        {
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.UserName??""),
                new Claim(ClaimTypes.GivenName,user.FirstName),
                new Claim(ClaimTypes.Surname,user.LastName),
                //new Claim("","")

            };
            var roles = await _userManager.GetRolesAsync(user);
            userClaims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var creadentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(userClaims),
                Expires = DateTime.UtcNow.AddDays(int.Parse(_config["JwtConfig:ExpiresInDays"])),
                SigningCredentials = creadentials,
                Issuer = _config["JwtConfig:Issuer"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt=tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwt);

        }
    }
}

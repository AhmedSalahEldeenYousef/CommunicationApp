using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using communication.API.Data;
using communication.API.Dtos;
using communication.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace communication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _confg;

        //IConfiguration -> Using To Access AppSettingJson File
        public AuthController(IAuthRepository repo, IConfiguration confg)
        {
            _confg = confg;
            _repo = repo;

        }

        [HttpPost("regester")]
        public async Task<IActionResult> Register(UserForRegisterInputDto input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            input.Username = input.Username.ToLower();

            if (await _repo.UserExists(input.Username))
                return BadRequest("هذا المستخدم مسجل من قبل");
            var CreateUser = new User
            {
                UserName = input.Username
            };
            var CreatedUser = await _repo.Register(CreateUser, input.Password);
            return StatusCode(201);

        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(UserForLoginDto userForLogin)
        {
            var UserFromRepo = await _repo.Login(userForLogin.Username.ToLower(), userForLogin.Password);
            if (UserFromRepo == null) return Unauthorized();
            var Claims = new[]{
                new Claim(ClaimTypes.NameIdentifier,UserFromRepo.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, UserFromRepo.UserName)
            };
            //Using Jwt Secret Key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_confg.GetSection("AppSettings:Token").Value));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512);
            var TokenDesctriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = signingCredentials
            };

            var TokenHandler = new JwtSecurityTokenHandler();
            var Token = TokenHandler.CreateToken(TokenDesctriptor);
            return Ok(new
            {
                token = TokenHandler.WriteToken(Token)
            });
        }
    }
}
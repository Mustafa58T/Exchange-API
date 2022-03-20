using Exchange_API.Data;
using Exchange_API.Dto;
using Exchange_API.Helpers;
using Exchange_API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Exchange_API.Controllers
{
    [Authorize]
    [Route("/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly DataContext _context;

        public ApiController(DataContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("user/login")]
        public async Task<ActionResult<UserLoginDto>> Login([FromBody] LoginRequestDto body)
        {

            var findedUSer = await _context.User.FirstOrDefaultAsync(user => user.Email == body.email && user.Password == body.password);

            if (findedUSer == null)
            {
                return Unauthorized("Kullanıcı adı veya şifre yanlış.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.JWT_SECRET);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, findedUSer.ID.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return Ok(new UserLoginDto()
            {
                Jwt = token,
                User = findedUSer,
            });


        }


        [HttpGet("user/me")]
        public async Task<ActionResult<string>> Me()
        {
            var id = HttpContext.User.Identity.Name.ToString();

            var user = await _context.User.FindAsync(Convert.ToInt32(id));
            if (user == null)
                return BadRequest("kullanıcı yok");
            return Ok(user);
        }


        [HttpPut("user/update")]
        public async Task<ActionResult<List<User>>> UpdateUser(User user)
        {
            var dbUser = await _context.User.FindAsync(user.ID);
            if (dbUser == null)
                return BadRequest("User not found.");

            dbUser.Password = user.Password;

            await _context.SaveChangesAsync();

            return Ok(await _context.User.ToListAsync());
        }

        [HttpDelete("user/{id}")]
        public async Task<ActionResult<List<User>>> Delete(int id)
        {
            var dbUser = await _context.User.FindAsync(id);
            if (dbUser == null)
                return BadRequest("User not found.");

            _context.User.Remove(dbUser);
            await _context.SaveChangesAsync();

            return Ok(await _context.User.ToListAsync());
        }

        [AllowAnonymous]
        [HttpPost("user/register")]
        public async Task<ActionResult<List<RegisterRequestDto>>> Register(RegisterRequestDto body)
        {

            var user = new User()
            {
                Email = body.Email,
                Password = body.Password,
                FirstName = body.FirstName,
                LastName = body.LastName,
                Phone = body.Phone,
                PictureUrl = "picture",
            };
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return Ok(await _context.User.ToListAsync());
        }
    }
}

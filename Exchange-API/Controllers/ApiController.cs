using Exchange_API.Data;
using Exchange_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exchange_API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly DataContext _context;

        public ApiController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            return Ok(await _context.User.ToListAsync());
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {

            var hero = await _context.User.FindAsync(id);
            if (hero == null)
                return BadRequest("kullanıcı yok");
            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<List<User>>> AddHero(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return Ok(await _context.User.ToListAsync());
        }

        [HttpPut]
        public async Task<ActionResult<List<User>>> UpdateUser(User user)
        {
            var dbUser = await _context.User.FindAsync(user.ID);
            if (dbUser == null)
                return BadRequest("Hero not found.");

            dbUser.Password = user.Password;

            await _context.SaveChangesAsync();

            return Ok(await _context.User.ToListAsync());
        }

        [HttpDelete("user/{id}")]
        public async Task<ActionResult<List<User>>> Delete(int id)
        {
            var dbHero = await _context.User.FindAsync(id);
            if (dbHero == null)
                return BadRequest("Hero not found.");

            _context.User.Remove(dbHero);
            await _context.SaveChangesAsync();

            return Ok(await _context.User.ToListAsync());
        }

    }
}

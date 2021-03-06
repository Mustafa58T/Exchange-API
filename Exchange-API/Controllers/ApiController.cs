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
                    new Claim(ClaimTypes.Name, findedUSer.UserId.ToString())
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
            var dbUser = await _context.User.FindAsync(user.UserId);
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

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet("product")]

        public async Task<ActionResult<List<ProductRequestDto>>> GetProducts()
        {

            var Product = await _context.Product.ToListAsync();
            return Ok(Product);
        }

        [AllowAnonymous]
        [HttpGet("productfav/{userid}")]

        public async Task<ActionResult<List<Fav>>> GetFavProducts(int userid)
        {

            var list= _context.Fav
                .Where(x => x.UserId == userid)
                .ToList();
            
            if (list != null)
            {
                var productList =  _context.Product.Where(x => x.UserId == userid)
                .ToList();

                return Ok(productList);
                
            }
            else
            {
                var product = "";
                return Ok(product);
                
            }

        }
        [AllowAnonymous]
        [HttpGet("productdetail/{id}")]


        public async Task<ActionResult<List<ProductRequestDto>>> Product(int id)
        {

            var Product = await _context.Product.FindAsync(id);
            return Ok(Product);
        }

        [AllowAnonymous]
        [HttpGet("myproductdetail/{userId}")]


        public async Task<ActionResult<List<ProductRequestDto>>> MyProduct(int userId)
        {

            var Product = _context.Product.Where(
                x => x.UserId == userId)
                .ToList();
            
            return Ok(Product);
        }

        [AllowAnonymous]
        [HttpGet("productimage/{id}")]
        public async Task<ActionResult<List<ProductImageRequestDto>>> ProductImages(int id)
        {
            var list = _context.ProductImage.Where(
                x => x.ProductId == id)
                .ToList();

            //var ProductId = await _context.ProductImage.FindAsync(id);
            return Ok(list);
        }


        [AllowAnonymous]
        [HttpGet("productcomment/{id}")]
        public async Task<ActionResult<List<CommentRequestDto>>> ProductComment(int id)
        {

            var list = _context.Comment.Where(
                x => x.ProductId == id)
                .ToList();

            return Ok(list);
        }

        [AllowAnonymous]
        [HttpPost("productcommentI/{id}")]
        public async Task<ActionResult<List<CommentRequestDto>>> ProductCommentI(int id,CommentRequestDto body)
        {

            var comment = new Comment()
            {
                UserId = body.UserId,
                FirstName = body.FirstName,
                LastName = body.LastName,
                ProductId = id,
                Comments = body.Comments,
                CommentDate = body.CommentDate,
                CommentImage = body.CommentImage,
            };
            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }


        [AllowAnonymous]
        [HttpGet("favorite/{userid}/{productId}")]
        public async Task<ActionResult<List<Fav>>> Fav(int userid, int productId)
        {
           
            var list = _context.Fav
                .Where(x => x.UserId == userid)
                .Where(x => x.ProductId == productId)
                .ToList();

           
            return Ok(list);
        }

        [AllowAnonymous]
        [HttpPost("favoriteI/{userid}/{productId}/{isFav}")]
        public async Task<ActionResult<List<FavRequestDto>>> FavI(int userid, int productId,Boolean isFav)
        {
            //var favorite = new Fav()
            //{
            //    UserId = userid,
            //    ProductId = productId,
            //    IsFav = isFav,
            //};
            //_context.Fav
            //    .Where(x => x.UserId == userid)
            //    .Where(x => x.ProductId == productId)
            //    .UpdateFromQuery(x => new Fav { IsFav = isFav });
            //await _context.SaveChangesAsync();
            var favorite = new Fav()
            {
                UserId = userid,
                ProductId = productId,
            };
            if (isFav)
            {
                _context.Fav.Add(favorite);
            }
            else
            {
                _context.Fav
                    .Where(x => x.UserId == userid)
                     .Where(x => x.ProductId == productId).DeleteFromQuery();
            }
            await _context.SaveChangesAsync();

            return Ok(favorite);
        }
        
    }
}


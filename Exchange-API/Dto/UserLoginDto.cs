using Exchange_API.Model;

namespace Exchange_API.Dto
{
    public class UserLoginDto
    {
        public string Jwt { get; set; }
        public User User { get; set; }
    }
}

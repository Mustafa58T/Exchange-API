using System.Text.Json.Serialization;

namespace Exchange_API.Dto
{
    public class RegisterRequestDto
    {

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Swaps { get; set; }
        public string PictureUrl { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

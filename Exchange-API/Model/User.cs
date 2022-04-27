using System.Text.Json.Serialization;

namespace Exchange_API.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Swaps { get; set; }
        public string PictureUrl{ get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }

    }
}

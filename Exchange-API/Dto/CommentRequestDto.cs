using System.Text.Json.Serialization;

namespace Exchange_API.Model
{
    public class CommentRequestDto
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ProductId { get; set; }
        public string Comments { get; set; }
        public DateTime CommentDate { get; set; }
        public string CommentImage { get; set; }


    }
}

using System.ComponentModel.DataAnnotations;

namespace ICream.Models
{
    public class ContactM
    {
        public int Id { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}

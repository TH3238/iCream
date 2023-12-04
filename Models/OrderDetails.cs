using System.ComponentModel.DataAnnotations;

namespace ICream.Models
{
    public class OrderDetails
    {
        public int Id { get; set; } // Primary key

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string HebDate { get; set; }
        public string Day { get; set; }
        public string Holiday { get; set; }
        public string Weather { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Apt { get; set; }

        public string IceCreams { get; set; }
        public float Total { get; set; }
        
        
    }
}


using System;
using System.ComponentModel.DataAnnotations;

namespace ICream.Models
{
    public class LiveOrders
    {
        [Key]
        public int Id { get; set; } // Primary key

        public string Status { get; set; } = "In Process"; // Default value is "In Process"

        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
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

        public float Total { get; set; }

        public string IceCreams { get; set; }
       

        // Additional field for tracking live orders
        

        // Copy constructor to create a LiveOrders object from an OrderDetails object
        public LiveOrders(OrderDetails orderDetails)
        {
            OrderDate = orderDetails.Date;
            Name = orderDetails.Name;
            Email = orderDetails.Email;
            Phone = orderDetails.Phone;
            City = orderDetails.City;
            Street = orderDetails.Street;
            Apt = orderDetails.Apt;
            IceCreams = orderDetails.IceCreams;
            Total = orderDetails.Total;
            HebDate = orderDetails.HebDate;
            Day = orderDetails.Day;
            Holiday = orderDetails.Holiday;
            Weather = orderDetails.Weather;
          

            // Additional field for tracking live orders
            Status = "In Process";
        }

        // Empty constructor required for EF migrations
        public LiveOrders()
        {
        }
    }
}

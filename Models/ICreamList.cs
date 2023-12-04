using System.ComponentModel;

namespace ICream.Models
{
    public class ICreamList
    {
        public int Id { get; set; }

        [Description("The name of the ice cream")]
        [DisplayName("Description")]
        public string Description { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
    }
}

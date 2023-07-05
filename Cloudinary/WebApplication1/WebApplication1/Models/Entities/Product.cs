using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
    }
}

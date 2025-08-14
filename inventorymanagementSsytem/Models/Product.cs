using System.ComponentModel.DataAnnotations;

namespace inventorymanagementSsytem.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        public string Description { get; set; }

       
        public int Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockLevel { get; set; }

        public string ImageUrl { get; set; }
        public Product()
        {
            
        }

    }
}

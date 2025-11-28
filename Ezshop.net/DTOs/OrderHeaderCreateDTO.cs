using System.ComponentModel.DataAnnotations;

namespace EzShop.Api.DTOs
{
    public class OrderHeaderCreateDTO
    {
        [Required]
        public string PickUpName { get; set; }
        [Required]
        public string PickUpEmail { get; set; }
        [Required]
        public string PickUpPhoneNumber { get; set; }
        public decimal OrderTotal { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public int TotalItems { get; set; }

        public List<OrderDetailCreateDTO> OrderDetailCreateDTOs { get; set; } = new();
        [Required]
        public string UserId { get; set; }
    }
}

using EzShop.Api.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzShop.Api.DTOs
{
    public class OrderHeaderUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string PickUpName { get; set; }
        [Required]
        public string PickUpEmail { get; set; }
        [Required]
        public string PickUpPhoneNumber { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
   

    }
}

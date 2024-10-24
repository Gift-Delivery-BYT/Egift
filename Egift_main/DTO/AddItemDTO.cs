using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace Egift_main.DTO;

public class AddItemDTO
{
        int _ItemID { get; set; }
        [Required]
        [MaxLength(200)]
        string _name { get; set; }
        [Required]
        double _pricehold { get; set; }
        [Required]
        DateFormat _date_of_production { get; set; }
}
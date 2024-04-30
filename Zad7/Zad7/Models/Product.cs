using System.ComponentModel.DataAnnotations;

namespace Zad7.Models;

public class Product
{
    [Required]
    public int IdProduct { get; set; }
    [MaxLength(200)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
    [Required]
    [RegularExpression(@"^[0-9]{0,25}|[0-9]{0,23}.[0-9]{1,2}|[0-9]{0,24}.[0-9]{1}?",ErrorMessage = "This is a numeric(25,2) type of field")]
    public double Price { get; set; }
    
}
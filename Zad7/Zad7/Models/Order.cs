using System.ComponentModel.DataAnnotations;

namespace Zad7.Models;

public class Order
{
    
    [Required]
    public int IdOrder { get; set; }
    [Required]
    public int IdProduct { get; set; }
    [Required]
    public int Amount { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    public DateTime? FulfilledAt { get; set; }
}
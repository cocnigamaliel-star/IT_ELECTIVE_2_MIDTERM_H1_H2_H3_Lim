using System.ComponentModel.DataAnnotations;

namespace MusicStorePos.DTOs;

public class CheckoutFormDTO
{
    [Required(ErrorMessage = "Customer name is required.")]
    [StringLength(100, ErrorMessage = "Customer name cannot exceed 100 characters.")]
    public string CustomerName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string? CustomerEmail { get; set; }
}
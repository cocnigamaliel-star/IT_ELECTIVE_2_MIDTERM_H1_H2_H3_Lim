namespace MusicStorePos.Models;

public class Transaction
{
    public Guid TransactionId { get; set; } = Guid.NewGuid();
    public DateTime Date { get; set; } = DateTime.Now;
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerEmail { get; set; }
    public decimal TotalAmount { get; set; }
    public List<CartItem> PurchasedItems { get; set; } = new();
}
using System.Linq;

namespace MusicStorePos.Models;

public class ShoppingCart
{
    public List<CartItem> Items { get; set; } = new();
    public decimal GrandTotal => Items.Sum(i => i.Total);
}
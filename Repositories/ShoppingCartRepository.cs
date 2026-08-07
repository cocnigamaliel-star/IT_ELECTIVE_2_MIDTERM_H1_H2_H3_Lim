using MusicStorePos.Models;

namespace MusicStorePos.Repositories;

public static class ShoppingCartRepository
{
    public static ShoppingCart ActiveCart { get; set; } = new();
}
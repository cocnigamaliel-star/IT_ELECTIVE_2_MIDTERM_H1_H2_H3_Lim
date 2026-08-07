using MusicStorePos.Models; // Make sure this matches your project's namespace

namespace MusicStorePos.Repositories;

public static class ProductRepository
{
    // Seeded inventory matching the Music Store theme (8 products)
    public static List<Product> Products { get; set; } = new()
    {
        new Product { Id = 1, Name = "Fender Stratocaster Electric Guitar", Price = 799.99m, StockQuantity = 6 },
        new Product { Id = 2, Name = "Yamaha Acoustic Dreadnought Guitar", Price = 249.50m, StockQuantity = 10 },
        new Product { Id = 3, Name = "Roland 88-Key Digital Piano", Price = 650.00m, StockQuantity = 0 }, // Out of Stock
        new Product { Id = 4, Name = "Marshall 50W Guitar Amplifier", Price = 320.00m, StockQuantity = 4 },
        new Product { Id = 5, Name = "Shure Dynamic Vocal Microphone", Price = 99.00m, StockQuantity = 15 },
        new Product { Id = 6, Name = "Alesis 5-Piece Electronic Drum Kit", Price = 450.00m, StockQuantity = 3 },
        new Product { Id = 7, Name = "D'Addario Guitar Strings 3-Pack", Price = 18.99m, StockQuantity = 25 },
        new Product { Id = 8, Name = "Focusrite USB Audio Interface", Price = 179.99m, StockQuantity = 8 }
    };
}
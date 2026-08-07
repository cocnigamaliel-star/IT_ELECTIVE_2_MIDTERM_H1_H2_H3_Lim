using Microsoft.AspNetCore.Mvc;
using MusicStorePos.DTOs;
using MusicStorePos.Models;
using MusicStorePos.Repositories;

namespace MusicStorePos.Controllers;

public class ProductController : Controller
{
    // US-01: Product Browsing
    public IActionResult Index()
    {
        ViewBag.ActiveCartCount = ShoppingCartRepository.ActiveCart.Items.Sum(i => i.Quantity);
        return View(ProductRepository.Products);
    }

    // US-02: Add to Cart
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddToCart(AddToCartDTO dto)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Invalid quantity specified.";
            return RedirectToAction(nameof(Index));
        }

        var product = ProductRepository.Products.FirstOrDefault(p => p.Id == dto.ProductId);
        if (product == null)
        {
            TempData["ErrorMessage"] = "Selected product does not exist.";
            return RedirectToAction(nameof(Index));
        }

        var existingCartItem = ShoppingCartRepository.ActiveCart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
        int currentInCart = existingCartItem?.Quantity ?? 0;
        int requestedTotal = currentInCart + dto.Quantity;

        if (requestedTotal > product.StockQuantity)
        {
            TempData["ErrorMessage"] = $"Cannot add {dto.Quantity} units. Only {product.StockQuantity - currentInCart} remaining in stock.";
            return RedirectToAction(nameof(Index));
        }

        if (existingCartItem != null)
        {
            existingCartItem.Quantity = requestedTotal;
        }
        else
        {
            ShoppingCartRepository.ActiveCart.Items.Add(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = dto.Quantity
            });
        }

        TempData["SuccessMessage"] = $"Added '{product.Name}' to cart.";
        return RedirectToAction(nameof(Index));
    }
}
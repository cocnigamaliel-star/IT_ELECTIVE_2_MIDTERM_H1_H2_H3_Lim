using Microsoft.AspNetCore.Mvc;
using MusicStorePos.DTOs;
using MusicStorePos.Models;
using MusicStorePos.Repositories;

namespace MusicStorePos.Controllers;

public class CartController : Controller
{
    // US-03: View Active Cart
    public IActionResult Index()
    {
        return View(ShoppingCartRepository.ActiveCart);
    }

    // US-03: Update Quantity
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateQuantity(UpdateCartDTO dto)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Invalid quantity update.";
            return RedirectToAction(nameof(Index));
        }

        var cartItem = ShoppingCartRepository.ActiveCart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
        var product = ProductRepository.Products.FirstOrDefault(p => p.Id == dto.ProductId);

        if (cartItem == null || product == null)
        {
            TempData["ErrorMessage"] = "Item not found in cart or inventory.";
            return RedirectToAction(nameof(Index));
        }

        if (dto.Quantity > product.StockQuantity)
        {
            TempData["ErrorMessage"] = $"Cannot update quantity to {dto.Quantity}. Only {product.StockQuantity} available in stock.";
            return RedirectToAction(nameof(Index));
        }

        cartItem.Quantity = dto.Quantity;
        TempData["SuccessMessage"] = $"Updated quantity for '{product.Name}'.";
        return RedirectToAction(nameof(Index));
    }

    // US-04: Remove Item
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveItem(int productId)
    {
        var item = ShoppingCartRepository.ActiveCart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            ShoppingCartRepository.ActiveCart.Items.Remove(item);
            TempData["SuccessMessage"] = $"Removed '{item.ProductName}' from cart.";
        }

        return RedirectToAction(nameof(Index));
    }
}
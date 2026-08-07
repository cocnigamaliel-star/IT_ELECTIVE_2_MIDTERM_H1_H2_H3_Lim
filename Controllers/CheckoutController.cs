using Microsoft.AspNetCore.Mvc;
using MusicStorePos.DTOs;
using MusicStorePos.Models;
using MusicStorePos.Repositories;

namespace MusicStorePos.Controllers;

public class CheckoutController : Controller
{
    // US-05: Checkout Screen
    public IActionResult Index()
    {
        if (!ShoppingCartRepository.ActiveCart.Items.Any())
        {
            TempData["ErrorMessage"] = "Cannot proceed to checkout with an empty cart.";
            return RedirectToAction("Index", "Cart");
        }

        ViewBag.Cart = ShoppingCartRepository.ActiveCart;
        // Points directly to Checkout.cshtml
        return View("Checkout", new CheckoutFormDTO());
    }

    // US-05: Process Payment & Deduct Inventory
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ProcessCheckout(CheckoutFormDTO dto)
    {
        if (!ShoppingCartRepository.ActiveCart.Items.Any())
        {
            ModelState.AddModelError("", "Your shopping cart is empty.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Cart = ShoppingCartRepository.ActiveCart;
            // Points back to Checkout.cshtml if validation fails
            return View("Checkout", dto);
        }

        foreach (var item in ShoppingCartRepository.ActiveCart.Items)
        {
            var product = ProductRepository.Products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product == null || item.Quantity > product.StockQuantity)
            {
                ModelState.AddModelError("", $"Stock limit exceeded for '{item.ProductName}'. Please adjust your cart.");
                ViewBag.Cart = ShoppingCartRepository.ActiveCart;
                // Points back to Checkout.cshtml if stock error occurs
                return View("Checkout", dto);
            }
        }

        var transaction = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Date = DateTime.Now,
            CustomerName = dto.CustomerName,
            CustomerEmail = dto.CustomerEmail,
            TotalAmount = ShoppingCartRepository.ActiveCart.GrandTotal,
            PurchasedItems = new List<CartItem>(ShoppingCartRepository.ActiveCart.Items)
        };

        foreach (var item in ShoppingCartRepository.ActiveCart.Items)
        {
            var product = ProductRepository.Products.First(p => p.Id == item.ProductId);
            product.StockQuantity -= item.Quantity;
        }

        TransactionRepository.Transactions.Add(transaction);
        ShoppingCartRepository.ActiveCart.Items.Clear();

        TempData["SuccessMessage"] = "Transaction completed successfully!";
        return RedirectToAction(nameof(Details), new { id = transaction.TransactionId });
    }

    // US-06: Transaction History
    public IActionResult History()
    {
        var transactions = TransactionRepository.Transactions.OrderByDescending(t => t.Date).ToList();
        // Points directly to TransactionHistory.cshtml
        return View("TransactionHistory", transactions);
    }

    // US-06: Transaction Details
    public IActionResult Details(Guid id)
    {
        var transaction = TransactionRepository.Transactions.FirstOrDefault(t => t.TransactionId == id);
        if (transaction == null)
        {
            return NotFound();
        }

        // Points directly to TransactionDetails.cshtml
        return View("TransactionDetails", transaction);
    }
}
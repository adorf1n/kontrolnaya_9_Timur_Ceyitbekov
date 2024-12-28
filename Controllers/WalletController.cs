using Microsoft.AspNetCore.Mvc;
using WebApplication.Data;
using WebApplication.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace WebApplication.Controllers
{
    public class WalletController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WalletController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var wallet = _context.Wallets.FirstOrDefault(w => w.UserId == userId);
            if (wallet == null)
            {
                wallet = new Wallet { UserId = userId, Balance = 0 };
                _context.Wallets.Add(wallet);
                _context.SaveChanges();
            }
            return View(wallet);
        }

        [HttpPost]
        public IActionResult AddFunds(double amount)
        {
            var userId = _userManager.GetUserId(User);
            var wallet = _context.Wallets.FirstOrDefault(w => w.UserId == userId);
            if (wallet != null)
            {
                wallet.Balance += amount;
                _context.Transactions.Add(new Transaction
                {
                    WalletId = wallet.Id,
                    Amount = amount,
                    Date = DateTime.Now,
                    Type = "Deposit"
                });
                _context.SaveChanges();
                TempData["Message"] = "Пополнение счета успешно!";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult PayService(double amount)
        {
            var userId = _userManager.GetUserId(User);
            var wallet = _context.Wallets.FirstOrDefault(w => w.UserId == userId);
            if (wallet != null && wallet.Balance >= amount)
            {
                wallet.Balance -= amount;
                _context.Transactions.Add(new Transaction
                {
                    WalletId = wallet.Id,
                    Amount = amount,
                    Date = DateTime.Now,
                    Type = "Payment"
                });
                _context.SaveChanges();
                TempData["Message"] = "Оплата успешна!";
            }
            else
            {
                TempData["Message"] = "Недостаточно средств на счете!";
            }
            return RedirectToAction("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using TuitionManagement.Models;

namespace TuitionManagement.Controllers
{
    public class LSGDController : Controller
    {
        public readonly HpsvContext _context;
        public LSGDController(HpsvContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var transactions = _context.Transactions.ToList();
            int total = transactions.Count;
            decimal totalTransaction = transactions.Sum(t => t.Amount) ?? 0;

            ViewBag.total = total;
            ViewBag.totalTransaction = totalTransaction;

            return View(transactions);
        }

        public IActionResult Detail(string id)
        {
            var transaction = _context.Transactions.FirstOrDefault(t => t.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            ViewBag.total = _context.Transactions.Count();
            ViewBag.totalTransaction = _context.Transactions.Sum(t => t.Amount) ?? 0;

            return View(transaction);
        }

    }
}

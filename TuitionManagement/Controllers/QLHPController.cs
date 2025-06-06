using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TuitionManagement.Models;

namespace TuitionManagement.Controllers
{
    public class QLHPController : Controller
    {
        private readonly HpsvContext _context;
        public QLHPController(HpsvContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var tuitions = _context.Tuitions.ToList();

            decimal totalPaid = _context.Tuitions.Sum(t => t.AmountPaid) ?? 0;

            decimal totalDue = _context.Tuitions.Sum(t => t.AmountDue) ?? 0;

            ViewBag.totalPaid = totalPaid;
            ViewBag.totalDue = totalDue;

            return View(tuitions);
        }

        public IActionResult Sort()
        {

            return View();
        }
        [HttpGet]
        public IActionResult Sort(string sortOption)
        {
            var tuitions = _context.Tuitions.AsQueryable();

            switch (sortOption)
            {
                case "asc":
                    tuitions = tuitions.OrderBy(t => t.AmountDue);
                    break;
                case "desc":
                    tuitions = tuitions.OrderByDescending(t => t.AmountDue);
                    break;
                case "gte1m":
                    tuitions = tuitions.Where(t => t.AmountDue >= 1_000_000);
                    break;
                case "gte5m":
                    tuitions = tuitions.Where(t => t.AmountDue >= 5_000_000);
                    break;
                case "gt10m":
                    tuitions = tuitions.Where(t => t.AmountDue > 10_000_000);
                    break;
                default:
                    break;
            }

            return View("Index", tuitions.ToList());
        }
        [HttpGet]
        public IActionResult Status(string statusOption)
        {
            var tuitions = _context.Tuitions.AsQueryable();

            if (!string.IsNullOrEmpty(statusOption))
            {
                tuitions = tuitions.Where(t => t.Status != null && t.Status.ToLower() == statusOption.ToLower());
            }

            return View("Index", tuitions.ToList());
        }

    }
}

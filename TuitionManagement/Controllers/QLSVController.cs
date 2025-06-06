using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TuitionManagement.Models;

namespace TuitionManagement.Controllers
{
    public class QLSVController : Controller
    {
        public readonly HpsvContext _context;
        public QLSVController(HpsvContext context)
        {
            _context = context;
        }
        
         public IActionResult Index()
        {
            var students = _context.Students.ToList();

            int totalStudent = students.Count;
            decimal totalPaid = _context.Tuitions.Sum(t => t.AmountPaid) ?? 0;

            int totalDue = _context.Students
                .Count(s => s.Tuitions.Any(t => (t.AmountPaid ?? 0) < (t.AmountDue ?? 0)));

            ViewBag.totalPaid = totalPaid;
            ViewBag.totalStudent = totalStudent;
            ViewBag.totalDue = totalDue;

            return View(students);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        [HttpGet]
        public IActionResult Search(string searchString)
        {
            var students = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                students = students.Where(s =>
                    s.Id.Contains(searchString) ||
                    s.FullName.Contains(searchString) ||
                    s.Class.Contains(searchString) ||
                    s.Department.Contains(searchString) ||
                    s.Email.Contains(searchString) ||
                    (s.PhoneNumber != null && s.PhoneNumber.Contains(searchString))
                );
            }

            return View("Index", students.ToList());
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Student? student = _context.Students.Find(id);
            if (student != null)
            {
                return View(student);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Students.Update(student);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(student);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi cập nhật sinh viên: " + ex.Message);

            }
            return View(student);  
        }
        [HttpGet]
        public IActionResult DeleteStudent(string id)
        {
            var student = _context.Students
                .Include(s => s.Tuitions)
                .Include(s => s.Transactions)
                .FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            
            _context.Tuitions.RemoveRange(student.Tuitions);
            _context.Transactions.RemoveRange(student.Transactions);
            _context.Students.Remove(student);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}

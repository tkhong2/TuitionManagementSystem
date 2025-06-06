using Microsoft.AspNetCore.Mvc;
using TuitionManagement.Models;

namespace TuitionManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly HpsvContext _context;

        public AccountController(HpsvContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public IActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    HttpContext.Session.SetString("UserId", user.Id);
                    return RedirectToAction("Index", "QLSV");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không chính xác.");
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError(string.Empty, "Email đã được sử dụng.");
                    TempData["Message"] = "Đăng ký thất bại. Email đã được sử dụng.";
                    TempData["MessageType"] = "danger";
                }
                else
                {
                    var lastUser = _context.Users
                        .Where(u => u.Id.StartsWith("USER"))
                        .OrderByDescending(u => u.Id)
                        .FirstOrDefault();

                    int nextNumber = 1;
                    if (lastUser != null)
                    {
                        var numberPart = lastUser.Id.Substring(4);
                        if (int.TryParse(numberPart, out int lastNumber))
                        {
                            nextNumber = lastNumber + 1;
                        }
                    }

                    // Format lại ID mới
                    string newUserId = $"USER{nextNumber:D3}";

                    var user = new User
                    {
                        Id = newUserId,
                        Email = model.Email,
                        Password = model.Password,
                        Role = "Admin"
                    };
                    _context.Users.Add(user);
                    _context.SaveChanges();
                    TempData["Message"] = "Đăng ký thành công! Bạn có thể đăng nhập.";
                    TempData["MessageType"] = "success";
                    return RedirectToAction("Register", "Account");
                }
            }
            else
            {
                TempData["Message"] = "Đăng ký thất bại. Vui lòng kiểm tra lại thông tin.";
                TempData["MessageType"] = "danger";
            }
            return View(model);
        }
        public IActionResult Setting()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Index", "Account"); 

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return RedirectToAction("Index", "Account");

            return View(user);
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Account");
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Index", "Account");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return RedirectToAction("Index", "Account");

            if (user.Password != model.OldPassword)
            {
                ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng.");
                return View(model);
            }

            user.Password = model.NewPassword;
            _context.SaveChanges();

            TempData["Data"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("Setting");
        }

    }
}

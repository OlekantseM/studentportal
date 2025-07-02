using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace StudentPortal.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: Students/Register
        public IActionResult Register()
        {
            var model = new RegisterStudentViewModel
            {
                Student = new Student(),
                Courses = _context.Courses.ToList()
            };

            return View(model);
        }

        // ✅ POST: Students/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var student = model.Student;

                _context.Students.Add(student);
                _context.SaveChanges();

                // Link student to subjects of selected course
                var subjectIds = _context.CourseSubjects
                    .Where(cs => cs.CourseID == student.CourseID)
                    .Select(cs => cs.SubjectID)
                    .ToList();

                foreach (var subjectId in subjectIds)
                {
                    _context.StudentSubjects.Add(new StudentSubject
                    {
                        StudentID = student.StudentID,
                        SubjectID = subjectId
                    });
                }

                _context.SaveChanges();

                return RedirectToAction("Success");
            }

            // Repopulate course list if validation fails
            model.Courses = _context.Courses.ToList();
            return View(model);
        }

        // ✅ GET: Students/Success
        public IActionResult Success()
        {
            return View();
        }

        // ✅ GET: Students/Login
        public IActionResult Login()
        {
            return View();
        }

        // ✅ POST: Students/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var student = _context.Students.FirstOrDefault(s => s.Email == email);

            if (student == null)
            {
                ModelState.AddModelError("", "Email not found");
                return View();
            }

            if (student.PasswordHash != password) // Plain text for now
            {
                ModelState.AddModelError("", "Incorrect password");
                return View();
            }

            HttpContext.Session.SetInt32("StudentID", student.StudentID);
            return RedirectToAction("Dashboard");
        }

        // ✅ GET: Students/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("StudentID");
            return RedirectToAction("Login");
        }

        // ✅ GET: Students/Dashboard
        public IActionResult Dashboard()
        {
            int? studentId = HttpContext.Session.GetInt32("StudentID");
            if (studentId == null)
                return RedirectToAction("Login");

            var student = _context.Students
                .Include(s => s.StudentSubjects!)
                    .ThenInclude(ss => ss.Subject!)
                .Include(s => s.Grades!)
                    .ThenInclude(g => g.Assessment!)
                        .ThenInclude(a => a.Subject!)
                .Include(s => s.Grades!)
                    .ThenInclude(g => g.Assessment!)
                        .ThenInclude(a => a.Term!)
                .FirstOrDefault(s => s.StudentID == studentId);

            if (student == null)
                return RedirectToAction("Login");

            return View(student);
        }
    }
}

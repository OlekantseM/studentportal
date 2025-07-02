using Microsoft.AspNetCore.Mvc;
using StudentPortal.Data;
using System.Linq;

namespace StudentPortal.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();
            return View(courses);
        }
    }
}

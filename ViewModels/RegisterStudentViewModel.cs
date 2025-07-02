using System.Collections.Generic;
using StudentPortal.Models;

namespace StudentPortal.ViewModels
{
    public class RegisterStudentViewModel
    {
        public Student Student { get; set; } = new Student();
        public List<Course> Courses { get; set; } = new List<Course>();
    }
}

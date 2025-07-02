using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models
{
    public class Course
    {
        public int CourseID { get; set; }

        [Required]
        public string CourseName { get; set; } = string.Empty;

        public ICollection<CourseSubject> CourseSubjects { get; set; } = new List<CourseSubject>();
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}

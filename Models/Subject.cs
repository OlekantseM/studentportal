using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models
{
    public class Subject
    {
        public int SubjectID { get; set; }

        [Required]
        public string SubjectName { get; set; } = string.Empty;

        public ICollection<CourseSubject> CourseSubjects { get; set; } = new List<CourseSubject>();
        public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models
{
    public class Student
    {
        public int StudentID { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? Gender { get; set; }

        public string? GradeLevel { get; set; }

        public DateTime EnrollDate { get; set; } = DateTime.Now;

        public int CourseID { get; set; }
        public Course? Course { get; set; }

        // ✅ Remove nullable (?) and initialize with new List<>
        public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace StudentPortal.Models
{
    public class Grade
    {
        public int GradeID { get; set; }

        public int StudentID { get; set; }
        public Student? Student { get; set; }

        public int SubjectID { get; set; }
        public Subject? Subject { get; set; }

        [Required]
        public string Term { get; set; } = string.Empty;

        public decimal? Score { get; set; }

        public string? Comment { get; set; }

        // ✅ Add this to fix the error
        public int AssessmentID { get; set; }
        public Assessment Assessment { get; set; } = null!;
    }
}

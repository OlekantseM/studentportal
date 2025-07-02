using StudentPortal.Models;
using System.Collections.Generic;

namespace StudentPortal.ViewModels
{
    public class GradeViewModel
    {
        public int GradeID { get; set; }
        public int StudentID { get; set; }
        public int SubjectID { get; set; }

        public string Term { get; set; } = "";
        public string AssessmentName { get; set; } = ""; // ✅ New
        public int AssessmentID { get; set; }

        public decimal Score { get; set; }
        public string? Comment { get; set; }

        // For dropdowns
        public List<Term> Terms { get; set; } = new();
        public List<Assessment> Assessments { get; set; } = new(); // Will filter by Subject + Term
        public List<string> AssessmentNames { get; set; } = new() { "Assessment 1", "Assessment 2", "Exam" }; // ✅ New
    }
}

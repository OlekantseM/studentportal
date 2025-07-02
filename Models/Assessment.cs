namespace StudentPortal.Models
{
    public class Assessment
    {
        public int AssessmentID { get; set; }

        // Initialize string to empty to avoid null
        public string Name { get; set; } = string.Empty;

        public int TermID { get; set; }

        // Mark navigation properties as nullable or required
        public Term Term { get; set; } = null!; // using null-forgiving operator

        public double Weight { get; set; }

        public int SubjectID { get; set; }

        public Subject Subject { get; set; } = null!;

        // Initialize collection to empty list to avoid null
        public ICollection<Mark> Marks { get; set; } = new List<Mark>();
    }
}

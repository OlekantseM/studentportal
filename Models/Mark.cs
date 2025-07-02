namespace StudentPortal.Models
{
    public class Mark
    {
        public int MarkID { get; set; }

        public int StudentID { get; set; }
        public Student Student { get; set; } = null!;

        public int AssessmentID { get; set; }
        public Assessment Assessment { get; set; } = null!;

        public double Score { get; set; }  // e.g., 75
    }
}

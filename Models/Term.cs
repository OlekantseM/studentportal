namespace StudentPortal.Models
{
    public class Term
    {
        public int TermID { get; set; }

        // Initialize string to empty so it's never null
        public string Name { get; set; } = string.Empty;

        // Initialize collection to empty list to avoid null
        public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
    }
}

namespace StudentPortal.Models
{
    public class CourseSubject
    {
        public int CourseSubjectID { get; set; }

        public int CourseID { get; set; }
        public Course? Course { get; set; }

        public int SubjectID { get; set; }
        public Subject? Subject { get; set; }
    }
}

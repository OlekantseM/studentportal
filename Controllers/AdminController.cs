using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.ViewModels;
using System.Linq;

public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Helper method to check admin session
    private bool IsAdminLoggedIn()
    {
        return HttpContext.Session.GetString("IsAdmin") == "true";
    }

    // Redirect to login if not admin
    private IActionResult? CheckAdmin()
    {
        if (!IsAdminLoggedIn())
            return RedirectToAction("Login", "AdminAuth");

        return null;
    }

    // Show all students with their courses
    public IActionResult Students()
    {
        var redirect = CheckAdmin();
        if (redirect != null) return redirect;

        var students = _context.Students
            .Include(s => s.Course)
            .ToList();
        return View(students);
    }

    // GET: Show form to add grade — with summary overview
    public IActionResult AddGrade(int studentId, int? selectedSubjectId = null)
    {
        var redirect = CheckAdmin();
        if (redirect != null) return redirect;

        var student = _context.Students
            .Include(s => s.StudentSubjects!)
                .ThenInclude(ss => ss.Subject)
            .FirstOrDefault(s => s.StudentID == studentId);

        if (student == null)
            return NotFound();

        var studentSubjects = student.StudentSubjects.Select(ss => ss.Subject).ToList();
        var allTerms = _context.Terms.ToList();

        ViewBag.Subjects = studentSubjects;
        ViewBag.Terms = allTerms;
        ViewBag.AssessmentNames = new List<string> { "Assessment 1", "Assessment 2", "Exam" };
        ViewBag.SelectedSubjectId = selectedSubjectId;

        var grades = _context.Grades
            .Include(g => g.Assessment).ThenInclude(a => a.Subject)
            .Include(g => g.Assessment).ThenInclude(a => a.Term)
            .Where(g => g.StudentID == studentId)
            .ToList();

        ViewBag.ExistingGrades = grades;

        var summary = new List<dynamic>();

        foreach (var term in allTerms)
        {
            foreach (var subject in studentSubjects)
            {
                if (subject == null)
                    continue;

                var gradesInGroup = grades
                    .Where(g => g.Assessment.TermID == term.TermID && g.Assessment.SubjectID == subject.SubjectID)
                    .ToList();

                var assessment1 = gradesInGroup.FirstOrDefault(g => g.Assessment.Name == "Assessment 1")?.Score;
                var assessment2 = gradesInGroup.FirstOrDefault(g => g.Assessment.Name == "Assessment 2")?.Score;
                var exam = gradesInGroup.FirstOrDefault(g => g.Assessment.Name == "Exam")?.Score;

                decimal? finalMark = new[] { assessment1, assessment2, exam }
                    .Where(s => s.HasValue)
                    .Select(s => s!.Value)
                    .DefaultIfEmpty()
                    .Average();

                summary.Add(new
                {
                    Term = term.Name,
                    SubjectName = subject.SubjectName,
                    Assessment1 = assessment1,
                    Assessment2 = assessment2,
                    Exam = exam,
                    FinalMark = finalMark
                });
            }
        }

        ViewBag.SummaryOverview = summary;

        return View(student);
    }

    // POST: Save a new grade
    [HttpPost]
    public IActionResult AddGrade(int StudentID, int SubjectID, string Term, string AssessmentName, decimal Score, string Comment)
    {
        var redirect = CheckAdmin();
        if (redirect != null) return redirect;

        var assessment = _context.Assessments
            .Include(a => a.Subject)
            .Include(a => a.Term)
            .FirstOrDefault(a =>
                a.SubjectID == SubjectID &&
                a.Term.Name == Term &&
                a.Name == AssessmentName);

        if (assessment == null)
            return BadRequest("Matching assessment not found. Please ensure correct term and subject are selected.");

        var grade = new Grade
        {
            StudentID = StudentID,
            SubjectID = SubjectID,
            Term = Term,
            Score = Score,
            Comment = Comment,
            AssessmentID = assessment.AssessmentID
        };

        _context.Grades.Add(grade);
        _context.SaveChanges();

        return RedirectToAction("AddGrade", new { studentId = StudentID, selectedSubjectId = SubjectID });
    }

    // GET: Edit a specific grade
    public IActionResult EditGrade(int id)
    {
        var redirect = CheckAdmin();
        if (redirect != null) return redirect;

        var grade = _context.Grades
            .Include(g => g.Assessment).ThenInclude(a => a.Subject)
            .Include(g => g.Assessment).ThenInclude(a => a.Term)
            .FirstOrDefault(g => g.GradeID == id);

        if (grade == null) return NotFound();

        var vm = new GradeViewModel
        {
            GradeID = grade.GradeID,
            StudentID = grade.StudentID,
            SubjectID = grade.Assessment.SubjectID,
            Term = grade.Assessment.Term.Name,
            AssessmentName = grade.Assessment.Name,
            Score = grade.Score ?? 0,
            Comment = grade.Comment,
            Terms = _context.Terms.ToList(),
            AssessmentNames = new List<string> { "Assessment 1", "Assessment 2", "Exam" }
        };

        return View(vm);
    }

    // POST: Save updated grade
    [HttpPost]
    public IActionResult EditGrade(GradeViewModel model)
    {
        var redirect = CheckAdmin();
        if (redirect != null) return redirect;

        var assessment = _context.Assessments
            .Include(a => a.Term)
            .FirstOrDefault(a =>
                a.SubjectID == model.SubjectID &&
                a.Term.Name == model.Term &&
                a.Name == model.AssessmentName);

        if (assessment == null)
            return BadRequest("Matching assessment not found.");

        var grade = _context.Grades.FirstOrDefault(g => g.GradeID == model.GradeID);
        if (grade == null) return NotFound();

        grade.AssessmentID = assessment.AssessmentID;
        grade.Score = model.Score;
        grade.Comment = model.Comment;

        _context.SaveChanges();

        return RedirectToAction("AddGrade", new { studentId = model.StudentID, selectedSubjectId = model.SubjectID });
    }

    // GET: Confirm deletion
    public IActionResult DeleteGrade(int id)
    {
        var redirect = CheckAdmin();
        if (redirect != null) return redirect;

        var grade = _context.Grades
            .Include(g => g.Assessment).ThenInclude(a => a.Subject)
            .Include(g => g.Assessment).ThenInclude(a => a.Term)
            .FirstOrDefault(g => g.GradeID == id);

        if (grade == null) return NotFound();

        return View(grade);
    }

    // POST: Finalize deletion
    [HttpPost, ActionName("DeleteGrade")]
    public IActionResult DeleteGradeConfirmed(int id)
    {
        var redirect = CheckAdmin();
        if (redirect != null) return redirect;

        var grade = _context.Grades.FirstOrDefault(g => g.GradeID == id);
        if (grade == null) return NotFound();

        int studentId = grade.StudentID;
        int? subjectId = grade.SubjectID;

        _context.Grades.Remove(grade);
        _context.SaveChanges();

        return RedirectToAction("AddGrade", new { studentId = studentId, selectedSubjectId = subjectId });
    }
}

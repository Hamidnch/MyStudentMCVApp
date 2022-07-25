using lab4.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyStudentMCVApp.Controllers
{
    public class AcademicRecords2Controller : Controller
    {
        private readonly StudentRecordContext _context;

        public AcademicRecords2Controller(StudentRecordContext context)
        {
            _context = context;
        }

        // GET: AcademicRecords
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewBag.CourseSort = string.IsNullOrEmpty(sortOrder) ? "course_title_desc" : "";
            ViewBag.StudentSort = sortOrder == "student_name" ? "student_name_desc" : "student_name";

            var allRecords = _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .OrderBy(a => a.CourseCodeNavigation.Title)
                .ThenBy(a=> a.Student.Name);
            

            switch (sortOrder)
            {
                case "course_title_desc":
                    allRecords = allRecords
                        .OrderByDescending(a => a.CourseCodeNavigation.Title)
                        .ThenBy(a => a.Student.Name);
                    break;
                case "student_name":
                    allRecords = allRecords
                        .OrderBy(a => a.Student.Name)
                        .ThenBy(a => a.CourseCodeNavigation.Title);
                    break;
                case "student_name_desc":
                    allRecords = allRecords
                        .OrderByDescending(a => a.Student.Name)
                        .ThenBy(a => a.CourseCodeNavigation.Title);
                    break;
                default:
                    allRecords = allRecords
                        .OrderBy(a => a.CourseCodeNavigation.Title)
                        .ThenBy(a => a.Student.Name);
                    break;
            }

            var academicRecords = await allRecords.ToListAsync();
            return View(academicRecords);
        }
    }
}

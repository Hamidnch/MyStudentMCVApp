using AutoMapper;
using lab4.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyStudentMCVApp.Dtos;

namespace MyStudentMCVApp.Controllers
{
    public class AcademicRecordsController : Controller
    {
        private readonly StudentRecordContext _context;
        private readonly IMapper _mapper;

        public AcademicRecordsController(StudentRecordContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string sortOrder)
        {
            if (string.IsNullOrEmpty(sortOrder))
                sortOrder = "course_title";

            ViewBag.CourseSort = sortOrder == "course_title" ? "course_title_desc" : "course_title";
            ViewBag.StudentSort = sortOrder == "student_name" ? "student_name_desc" : "student_name";

            var allRecords = _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .OrderBy(a => a.CourseCodeNavigation.Title)
                .ThenBy(a => a.Student.Name);


            switch (sortOrder)
            {
                case "course_title_desc":
                    HttpContext.Session.SetString("sortOrder", "course_title_desc");
                    allRecords = allRecords
                        .OrderByDescending(a => a.CourseCodeNavigation.Title)
                        .ThenBy(a => a.Student.Name);
                    break;
                case "student_name":
                    HttpContext.Session.SetString("sortOrder", "student_name");
                    allRecords = allRecords
                        .OrderBy(a => a.Student.Name)
                        .ThenBy(a => a.CourseCodeNavigation.Title);
                    break;
                case "student_name_desc":
                    HttpContext.Session.SetString("sortOrder", "student_name_desc");
                    allRecords = allRecords
                        .OrderByDescending(a => a.Student.Name)
                        .ThenBy(a => a.CourseCodeNavigation.Title);
                    break;
                case "course_title":
                    goto default;
                default:
                    HttpContext.Session.SetString("sortOrder", "course_title");
                    allRecords = allRecords
                        .OrderBy(a => a.CourseCodeNavigation.Title)
                        .ThenBy(a => a.Student.Name);
                    break;
            }

            var academicRecords = await allRecords.ToListAsync();
            var academicRecordsDto = 
                academicRecords.Select(a => _mapper.Map<AcademicRecordDto>(a));
            return View(academicRecordsDto);
        }
        //// GET: AcademicRecords
        //public async Task<IActionResult> Index()
        //{
        //    var studentRecordContext = _context.AcademicRecords
        //        .Include(a => a.CourseCodeNavigation)
        //        .Include(a => a.Student);

        //    return View(await studentRecordContext.ToListAsync());
        //}

        // GET: AcademicRecords/Details/5
        public async Task<IActionResult> Details(string id)
        {
            //if (id == null || _context.AcademicRecords == null)
            //{
            //    return NotFound();
            //}

            var academicRecord = await _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (academicRecord == null)
            {
                return NotFound();
            }

            return View(academicRecord);
        }

        // GET: AcademicRecords/Create
        public IActionResult Create()
        {
            ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Title");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name");
            return View();
        }

        // POST: AcademicRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseCode,StudentId,Grade")] AcademicRecord academicRecord)
        {
            if (ModelState.IsValid)
            {
                if (!ExistAcademicRecord(academicRecord.StudentId, academicRecord.CourseCode))
                {
                    _context.Add(academicRecord);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.DuplicateRecord = "The record with this info already stored.";
                }
            }
            ViewData["CourseCode"] =
                new SelectList(_context.Courses, "Code", "Title", academicRecord.CourseCode);
            ViewData["StudentId"] =
                new SelectList(_context.Students, "Id", "Name", academicRecord.StudentId);
            return View(academicRecord);
        }

        // GET: AcademicRecords/Edit/5
        public async Task<IActionResult> Edit(string studentId, string courseCode)
        {
            //if (studentId == null || _context.AcademicRecords == null)
            //{
            //    return NotFound();
            //}

            var academicRecord = await _context.AcademicRecords.FindAsync(studentId, courseCode);
            if (academicRecord == null)
            {
                return NotFound();
            }

            ViewData["CourseCode"] =
                new SelectList(_context.Courses, "Code", "Title", academicRecord.CourseCode);
            ViewData["StudentId"] =
                new SelectList(_context.Students, "Id", "Name", academicRecord.StudentId);

            return View(academicRecord);
        }

        // POST: AcademicRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string studentId, [Bind("CourseCode,StudentId,Grade")] AcademicRecord academicRecord)
        {
            if (studentId != academicRecord.StudentId)
            {
                return NotFound();
            }

            if (academicRecord.Grade >= 100 || academicRecord.Grade <= 0)
            {
                ModelState.AddModelError("grade", "Must be between 0 and 100");
            }

            if (ModelState.IsValid)
            {
                //try
                //{
                _context.Update(academicRecord);
                await _context.SaveChangesAsync();
                //}
                // catch (DbUpdateConcurrencyException)
                // {
                // if (!AcademicRecordExists(academicRecord.StudentId))
                //{
                //return NotFound();
                // }
                // else
                //{
                //    throw;
                // }
                //}
                return RedirectToAction("Index");
            }

            ViewData["CourseCode"] = 
                new SelectList(_context.Courses, "Code", "Code", academicRecord.CourseCode);
            ViewData["StudentId"] = 
                new SelectList(_context.Students, "Id", "Id", academicRecord.StudentId);
            return View(academicRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAll(IFormCollection formCollection)
        {
            var sortOrder = HttpContext.Session.GetString("sortOrder");
            sortOrder ??= "course_name";
            var studentIds = formCollection["Item_StudentId"].ToString().Split(',');
            var courseIds = formCollection["item_CourseCode"].ToString().Split(',');
            var grades = formCollection["item.Grade"].ToString().Split(',');
            var errorList = new List<string>();

            for (int i = 0; i < studentIds.Length; i++)
            {
                if (int.Parse(grades[i]) >= 100 || int.Parse(grades[i]) <= 0)
                {
                    errorList.Add($"Grade for student id:{studentIds[i]} and course id:{courseIds[i]} Must be between 0 and 100");
                    //return RedirectToAction("Index", new { sortOrder = sortOrder });
                    TempData["Errors"] = errorList;
                    continue;
                }

                var academicRecord = new AcademicRecord()
                {
                    StudentId = studentIds[i],
                    CourseCode = courseIds[i],
                    Grade = int.Parse(grades[i])
                };

                _context.Update(academicRecord);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", new { sortOrder = sortOrder});
        }

        // GET: AcademicRecords/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            //if (id == null || _context.AcademicRecords == null)
            //{
            //    return NotFound();
            //}

            var academicRecord = await _context.AcademicRecords
                .Include(a => a.CourseCodeNavigation)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.StudentId == id);

            if (academicRecord == null)
            {
                return NotFound();
            }

            return View(academicRecord);
        }

        // POST: AcademicRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            //if (_context.AcademicRecords == null)
            //{
            //    return Problem("Entity set 'StudentRecordContext.AcademicRecords'  is null.");
            //}

            var academicRecord = await _context.AcademicRecords.FindAsync(id);
            if (academicRecord != null)
            {
                _context.AcademicRecords.Remove(academicRecord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademicRecordExists(string id)
        {
            return (_context.AcademicRecords?.Any(e => e.StudentId == id)).GetValueOrDefault();
        }

        
        [AcceptVerbs("GET", "POST")]
        public bool ExistAcademicRecord(string stdId, string crsId)
        {
            var existAcademicRecords = 
                _context.AcademicRecords.Where(ar=> ar.StudentId == stdId && ar.CourseCode == crsId);

            return existAcademicRecords.Any();
        }
    }
}

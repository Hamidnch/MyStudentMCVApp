namespace MyStudentMCVApp.Dtos
{
    public class AcademicRecordDto
    {
        public string CourseCode { get; set; } = null!;
        public string? CourseCodeNavigationTitle { get; set; }
        public string StudentId { get; set; } = null!;
        public string? StudentName { get; set; }
        public int? Grade { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lab4.DataAccess
{
    public partial class Course
    {
        public Course()
        {
            AcademicRecords = new HashSet<AcademicRecord>();
            StudentStudentNums = new HashSet<Student>();
        }

        public string Code { get; set; } = null!;
        [Required]
        [Display(Name = "Course title")]
        [StringLength(100, ErrorMessage = "Course title cannot be longer than {0} characters.")]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int? HoursPerWeek { get; set; }
        public decimal? FeeBase { get; set; }

        public virtual ICollection<AcademicRecord> AcademicRecords { get; set; }

        public virtual ICollection<Student> StudentStudentNums { get; set; }
    }
}

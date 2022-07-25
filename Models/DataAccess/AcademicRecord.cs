using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace lab4.DataAccess
{
    public partial class AcademicRecord
    {
        public string CourseCode { get; set; } = null!;
        public string StudentId { get; set; } = null!;
        public int? Grade { get; set; }

        [ValidateNever]
        public virtual Course CourseCodeNavigation { get; set; } = null!;
        [ValidateNever]
        public virtual Student Student { get; set; } = null!;
    }
}

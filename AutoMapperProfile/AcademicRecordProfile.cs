using AutoMapper;
using lab4.DataAccess;
using MyStudentMCVApp.Dtos;

namespace MyStudentMCVApp.AutoMapperProfile
{
    public class AcademicRecordProfile: Profile
    {
        public AcademicRecordProfile()
        {
            CreateMap<AcademicRecord, AcademicRecordDto>().ReverseMap();
        }
    }
}

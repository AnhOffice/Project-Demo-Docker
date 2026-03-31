using AutoMapper;
using StudentAPI.DTOs;
using StudentAPI.Model;

namespace StudentAPI.Mapper
{
    public class StudentMapper : Profile
    {
        public StudentMapper()
        {
            CreateMap<Student, ReadDTOs>();
            CreateMap<CreateDTOs, Student>();
            CreateMap<UpdateDTOs, Student>();
        }
    }
}

using AutoMapper;
using StudentAPI.DTOs;
using StudentAPI.Model;
using StudentAPI.Repository;

namespace StudentAPI.Service
{
    public class StudentService : IStudentService
    {
        private readonly ICommonRepository<Student, int> _commonRepository;
        private readonly IMapper _mapper;

        public StudentService(ICommonRepository<Student, int> commonRepository, IMapper mapper)
        {
            _commonRepository = commonRepository;
            _mapper = mapper;
        }

        public async Task<ReadDTOs> CreateAsync(CreateDTOs dto)
        {
            var student = _mapper.Map<Student>(dto);
            student.CreatedAt = DateTime.UtcNow;
            await _commonRepository.Add(student);
            return _mapper.Map<ReadDTOs>(student);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _commonRepository.GetById(id);
            if (student == null) return false;

            await _commonRepository.Delete(id);
            return true;
        }

        public async Task<IEnumerable<ReadDTOs>> GetAllAsync()
        {
            var students = await _commonRepository.GetAll();
            return _mapper.Map<IEnumerable<ReadDTOs>>(students);
        }

        public async Task<ReadDTOs> GetByIdAsync(int id)
        {
            var student = await _commonRepository.GetById(id);
            return student != null ? _mapper.Map<ReadDTOs>(student) : null;
        }

        public async Task<bool> UpdateAsync(int id, UpdateDTOs dto)
        {
            var student = await _commonRepository.GetById(id);
            if (student == null) return false;

            _mapper.Map(dto, student);
            await _commonRepository.Update(student);
            return true;
        }
    }
}

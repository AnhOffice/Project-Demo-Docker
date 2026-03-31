using StudentAPI.DTOs;

namespace StudentAPI.Service
{
    public interface IStudentService
    {
        Task<IEnumerable<ReadDTOs>> GetAllAsync();
        Task<ReadDTOs> GetByIdAsync(int id);
        Task<ReadDTOs> CreateAsync(CreateDTOs dto);
        Task<bool> UpdateAsync(int id, UpdateDTOs dto);
        Task<bool> DeleteAsync(int id);
    }
}

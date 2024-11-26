// ============================================================================
// Author: Ngoc To
// Created: Nov 26, 2024
//
// This class represents a Data Access Object (DAO) for the Call entity.
// It provides methods for interacting with the underlying data storage.
// ============================================================================
namespace HelpdeskDAL
{
    public class CallDAO
    {
        readonly IRepository<Call> _callRepository;
        public CallDAO()
        {
            _callRepository = new HelpdeskRepository<Call>();
        }
        public async Task<List<Call>> GetAll()
        {
            return await _callRepository.GetAll();
        }
        public async Task<Call> GetById(int id)
        {
            return (await _callRepository.GetOne(call => call.Id == id))!;
        }
        public async Task<List<Call>> GetByEmployeeId(int employeeId)
        {
            return (await _callRepository.GetSome(call => call.EmployeeId == employeeId))!;
        }
        public async Task<List<Call>> GetByProblemId(int problemId)
        {
            return (await _callRepository.GetSome(call => call.ProblemId == problemId))!;
        }
        public async Task<int> Add(Call newCall)
        {
            return (await _callRepository.Add(newCall)).Id;
        }

        public async Task<UpdateStatus> Update(Call updatedCall)
        {
            return await _callRepository.Update(updatedCall);
        }
        public async Task<int> Delete(int? id)
        {
            if (id.HasValue)
                return await _callRepository.Delete((int)id!);
            else
                throw new Exception("ID can't be null.");
        }
    }
}

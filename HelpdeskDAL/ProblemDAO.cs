// ============================================================================
// Author: Ngoc To
// Created: Oct 26, 2024
//
// This class represents a Data Access Object (DAO) for the Problem entity.
// It provides methods for interacting with the underlying data storage.
// ============================================================================

namespace HelpdeskDAL
{
    public class ProblemDAO
    {
        readonly IRepository<Problem> _repo;
        public ProblemDAO()
        {
            _repo = new HelpdeskRepository<Problem>();
        }

        public async Task<Problem> GetById(int id)
        {

            return (await _repo.GetOne(prob => prob.Id == id))!;
        }

        public async Task<List<Problem>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<Problem> GetByDescription(string desc)
        {
            return (await _repo.GetOne(prob => prob.Description == desc))!;
        }

        public async Task<int> Add(Problem newProblem)
        {
            return (await _repo.Add(newProblem)).Id;
        }

        public async Task<UpdateStatus> Update(Problem updatedProblem)
        {
            return await _repo.Update(updatedProblem);
        }

        public async Task<int> Delete(int? id)
        {
            if (id.HasValue)
                return await _repo.Delete((int)id!);
            else
                throw new Exception("ID can't be null.");
        }
    }
}

// ============================================================================
// Author: Ngoc To
// Created: Oct 26, 2024
//
// This class represents a Data Access Object (DAO) for the Employee entity.
// It provides methods for interacting with the underlying data storage.
// ============================================================================

namespace HelpdeskDAL
{
    public class EmployeeDAO
    {
        readonly IRepository<Employee> _repo;
        public EmployeeDAO()
        {
            _repo = new HelpdeskRepository<Employee>();
        }
        public async Task<Employee> GetByEmail(string email)
        {
            return (await _repo.GetOne(emp => emp.Email == email))!;
        }
        public async Task<Employee> GetByLastName(string lastName)
        {
            return (await _repo.GetOne(emp => emp.LastName == lastName))!;
        }

        public async Task<Employee> GetById(int id)
        {

            return (await _repo.GetOne(emp => emp.Id == id))!;
        }

        public async Task<List<Employee>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<int> Add(Employee newEmployee)
        {
            return (await _repo.Add(newEmployee)).Id;
        }

        public async Task<UpdateStatus> Update(Employee updatedEmployee)
        {
            return await _repo.Update(updatedEmployee);
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

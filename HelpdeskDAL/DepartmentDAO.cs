// ============================================================================
// Author: Ngoc To
// Created: Oct 26, 2024
//
// This class represents a Data Access Object (DAO) for the Department entity.
// It provides methods for interacting with the underlying data storage.
// ============================================================================

namespace HelpdeskDAL
{
    public class DepartmentDAO
    {
        readonly IRepository<Department> _repo;
        public DepartmentDAO()
        {
            _repo = new HelpdeskRepository<Department>();
        }
        public async Task<List<Department>> GetAll()
        {
            return await _repo.GetAll();
        }
    }
}

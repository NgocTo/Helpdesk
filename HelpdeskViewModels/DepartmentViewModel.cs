// ============================================================================
// Author: Ngoc To
// Created: Oct 26, 2024

// This class represents a ViewModel for the Department entity.
// It serves as a data transfer object between the data access layer and the presentation layer.
// ============================================================================

using HelpdeskDAL;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskViewModels
{
    public class DepartmentViewModel
    {
        private readonly DepartmentDAO _dao;
        public string? DepartmentName {  get; set; }
        public int Id { get; set; }
        public string? Timer { get; set; }

        public DepartmentViewModel()
        {
            _dao = new DepartmentDAO();
        }

        public async Task<List<DepartmentViewModel>> GetAll()
        {
            List<DepartmentViewModel> allVms = new();
            List<Department> allDepartments = new();

            try
            {
                allDepartments = await _dao.GetAll();
                foreach (Department dep in allDepartments)
                {
                    DepartmentViewModel depVm = new();
                    depVm.Id = dep.Id;
                    depVm.DepartmentName = dep.DepartmentName;
                    depVm.Timer = Convert.ToBase64String(dep.Timer!);
                    allVms.Add(depVm);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return allVms;
        }
    }
}

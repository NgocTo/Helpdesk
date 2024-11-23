// ============================================================================
// Author: Ngoc To
// Created: Oct 26, 2024

// This class represents a ViewModel for the Employee entity.
// It serves as a data transfer object between the data access layer and the presentation layer.
// ============================================================================

using HelpdeskDAL;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskViewModels
{
    public class EmployeeViewModel
    {
        private readonly EmployeeDAO _dao;
        public string? Title { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Phoneno { get; set; }
        public string? Timer { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? Id { get; set; }
        public bool? IsTech { get; set; }
        public string? StaffPicture64 { get; set; }
        // constructor
        public EmployeeViewModel()
        {
            _dao = new EmployeeDAO();
        }
        public async Task GetByEmail(string email)
        {
            try
            {
                Employee emp = await _dao.GetByEmail(email);
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer!);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }
        public async Task GetById(int id)
        {
            try
            {
                Employee emp = await _dao.GetById(id);
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer!);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }
        public async Task<List<EmployeeViewModel>> GetAll()
        {
            List<EmployeeViewModel> allVms = new();
            List<Employee> allEmployees = new();
            try
            {
                allEmployees = await _dao.GetAll();
                // we need to convert Employee instance to EmployeeViewModel because
                // the Web Layer isn't aware of the Domain class Employee
                foreach (Employee emp in allEmployees)
                {
                    EmployeeViewModel empVm = new()
                    {
                        Title = emp.Title,
                        Firstname = emp.FirstName,
                        Lastname = emp.LastName,
                        Phoneno = emp.PhoneNo,
                        Email = emp.Email,
                        Id = emp.Id,
                        DepartmentId = emp.DepartmentId,
                        DepartmentName = emp.Department.DepartmentName,
                        StaffPicture64 = emp.StaffPicture != null? Convert.ToBase64String(emp.StaffPicture!) : null,
                        Timer = Convert.ToBase64String(emp.Timer!)
                    };
                    allVms.Add(empVm);
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
        public async Task Add()
        {
            Id = -1;
            try
            {
                Employee emp = new()
                {
                    Title = this.Title,
                    FirstName = this.Firstname,
                    LastName = this.Lastname,
                    PhoneNo = this.Phoneno,
                    Email = this.Email,
                    DepartmentId = this.DepartmentId,
                    StaffPicture = this.StaffPicture64 != null ? Convert.FromBase64String(this.StaffPicture64!) : null
                };
                Id = await _dao.Add(emp);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }
        public async Task<int> Update()
        {
            int updateStatus;
            try
            {
                Employee emp = new()
                {
                    Title = this.Title,
                    FirstName = this.Firstname,
                    LastName = this.Lastname,
                    PhoneNo = this.Phoneno,
                    Email = this.Email,
                    Id = (int)this.Id!,
                    DepartmentId = this.DepartmentId,
                    StaffPicture = this.StaffPicture64 != null ? Convert.FromBase64String(this.StaffPicture64!) : null,
                    Timer = Convert.FromBase64String(Timer!)
                };
                updateStatus = -1; // start out with a failed state
                updateStatus = Convert.ToInt16(await _dao.Update(emp)); // overwrite status
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return updateStatus;
        }
        public async Task<int> Delete()
        {
            try
            {
                // dao will return # of rows deleted
                return await _dao.Delete(Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }
    }
}

using HelpdeskDAL;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskViewModels
{
    public class CallViewModel
    {
        private readonly CallDAO _dao;
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int TechId { get; set; }
        public int ProblemId { get; set; }
        public string? EmployeeName { get; set; }
        public string? ProblemDescription { get; set; }
        public string? TechName { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }
        public bool OpenStatus { get; set; }
        public string? Notes {  get; set; }
        public string? Timer { get; set; }
        public CallViewModel ()
        {
            _dao = new CallDAO ();
        }
        public async Task<List<CallViewModel>> GetAll()
        {
            List<CallViewModel> allVms = new();
            List<Call> allCalls = new();
            try
            {
                allCalls = await _dao.GetAll();
                foreach (Call call in allCalls)
                {
                    CallViewModel vm = new()
                    {
                        Id = call.Id,
                        EmployeeId = call.EmployeeId,
                        ProblemId = call.ProblemId,
                        TechId = call.TechId,
                        DateOpened = call.DateOpened,
                        DateClosed = call.DateClosed,
                        OpenStatus = call.OpenStatus,
                        Notes = call.Notes,
                        Timer = Convert.ToBase64String(call.Timer!)
                    };

                    ProblemDescription = call.Problem.Description;
                    EmployeeName = call.Employee.FirstName + " " + call.Employee.LastName;
                    TechName = call.Employee.FirstName + " " + call.Employee.LastName;

                    allVms.Add(vm);
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

        public async Task<List<CallViewModel>> GetByEmployeeId(int id)
        {
            List<CallViewModel> allVms = new();
            List<Call> allCalls = new();
            try
            {
                allCalls = await _dao.GetByEmployeeId(id);
                foreach (Call call in allCalls)
                {
                    CallViewModel vm = new()
                    {
                        Id = call.Id,
                        EmployeeId = call.EmployeeId,
                        ProblemId = call.ProblemId,
                        TechId = call.TechId,
                        DateOpened = call.DateOpened,
                        DateClosed = call.DateClosed,
                        OpenStatus = call.OpenStatus,
                        Notes = call.Notes,
                        Timer = Convert.ToBase64String(call.Timer!)
                    };

                    ProblemDescription = call.Problem.Description;
                    EmployeeName = call.Employee.FirstName + " " + call.Employee.LastName;
                    TechName = call.Employee.FirstName + " " + call.Employee.LastName;

                    allVms.Add(vm);
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
        public async Task<List<CallViewModel>> GetByProblemId(int id)
        {
            List<CallViewModel> allVms = new();
            List<Call> allCalls = new();
            try
            {
                allCalls = await _dao.GetByProblemId(id);
                foreach (Call call in allCalls)
                {
                    CallViewModel vm = new()
                    {
                        Id = call.Id,
                        EmployeeId = call.EmployeeId,
                        ProblemId = call.ProblemId,
                        TechId = call.TechId,
                        DateOpened = call.DateOpened,
                        DateClosed = call.DateClosed,
                        OpenStatus = call.OpenStatus,
                        Notes = call.Notes,
                        Timer = Convert.ToBase64String(call.Timer!)
                    };

                    ProblemDescription = call.Problem.Description;
                    EmployeeName = call.Employee.FirstName + " " + call.Employee.LastName;
                    TechName = call.Employee.FirstName + " " + call.Employee.LastName;
                    allVms.Add(vm);
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

        public async Task GetById(int id)
        {
            try
            {
                Call call = await _dao.GetById(id);

                Id = call.Id;
                EmployeeId = call.EmployeeId;
                ProblemId = call.ProblemId;
                TechId = call.TechId;
                DateOpened = call.DateOpened;
                DateClosed = call.DateClosed;
                OpenStatus = call.OpenStatus;
                Notes = call.Notes;
                Timer = Convert.ToBase64String(call.Timer!);

                ProblemDescription = call.Problem.Description;
                EmployeeName = call.Employee.FirstName + " " + call.Employee.LastName;
                TechName = call.Employee.FirstName + " " + call.Employee.LastName;
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Notes = "not found";
            }
            catch (Exception ex)
            {
                Notes = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task Add()
        {
            Id = -1;
            try
            {
                Call call = new()
                {
                    EmployeeId = this.EmployeeId,
                    ProblemId = this.ProblemId,
                    TechId = this.TechId,
                    OpenStatus = true,
                    DateOpened = DateTime.Now,
                    Notes = this.Notes!
                };
                Id = await _dao.Add(call);
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
            int updateStatus = -1;
            try
            {
                Call call = new()
                {
                    Id = this.Id,
                    EmployeeId = this.EmployeeId,
                    ProblemId = this.ProblemId,
                    TechId = this.TechId,
                    OpenStatus = this.OpenStatus,
                    DateOpened = this.DateOpened,
                    DateClosed = (this.DateClosed == null && !this.OpenStatus)? DateTime.Now : null,
                    Notes = this.Notes!,
                    Timer = Convert.FromBase64String(this.Timer!)
                };
                updateStatus = Convert.ToInt16(await _dao.Update(call));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
            return updateStatus;
        }
        public async Task<int> Delete(int id)
        {
            try
            {
                return await _dao.Delete(id);
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

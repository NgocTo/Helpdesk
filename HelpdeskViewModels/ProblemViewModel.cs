// ============================================================================
// Author: Ngoc To
// Created: Oct 26, 2024

// This class represents a ViewModel for the Problem entity.
// It serves as a data transfer object between the data access layer and the presentation layer.
// ============================================================================


using HelpdeskDAL;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskViewModels
{
    public class ProblemViewModel
    {
        private readonly ProblemDAO _dao;
        public string? Description { get; set; }
        public int? Id { get; set; }
        public string? Timer { get; set; }
        public ProblemViewModel()
        {
            _dao = new ProblemDAO();
        }

        public async Task<List<ProblemViewModel>> GetAll()
        {
            List<ProblemViewModel> allVms = new();
            List<Problem> allProblems = new();
            try
            {
                allProblems = await _dao.GetAll();
                foreach (Problem prob in allProblems)
                {
                    ProblemViewModel probVm = new()
                    {
                        Id = prob.Id,
                        Description = prob.Description,
                        Timer = Convert.ToBase64String(prob.Timer!)
                    };
                    allVms.Add(probVm);
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
                Problem prob = await _dao.GetById(id);
                Id = prob.Id;
                Description = prob.Description;
                Timer = Convert.ToBase64String(prob.Timer!);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Description = "not found";
            }
            catch (Exception ex)
            {
                Description = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task GetByDescription(string desc)
        {
            try
            {
                Problem prob = await _dao.GetByDescription(desc);
                Id = prob.Id;
                Description = prob.Description;
                Timer = Convert.ToBase64String(prob.Timer!);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Description = "not found";
            }
            catch (Exception ex)
            {
                Description = "not found";
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
                Problem prob = new()
                {
                    Description = this.Description
                };
                Id = await _dao.Add(prob);
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
                Problem prob = new()
                {
                    Description = this.Description,
                    Id = (int)this.Id!,
                    Timer = Convert.FromBase64String(Timer!)
                };
                updateStatus = -1; 
                updateStatus = Convert.ToInt16(await _dao.Update(prob));
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

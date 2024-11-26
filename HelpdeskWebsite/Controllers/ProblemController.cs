// ============================================================================
// Author: Ngoc To
// Created: Oct 26, 2024
//
// This controller sets the routes and handles HTTP requests related to the
// Problem entity.
// ============================================================================

using HelpdeskViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProblemController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                ProblemViewModel viewmodel = new();
                List<ProblemViewModel> allProblems = await viewmodel.GetAll();
                return Ok(allProblems);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                ProblemViewModel viewmodel = new();
                await viewmodel.GetById(id);
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
        [HttpGet("{desc}")]
        public async Task<IActionResult> GetByDescription(string desc)
        {
            ProblemViewModel viewmodel = new();
            await viewmodel.GetByDescription(desc);
            return Ok(viewmodel);
        }
        [HttpPost]
        public async Task<ActionResult> Post(ProblemViewModel viewmodel)
        {
            try
            {
                await viewmodel.Add();
                return viewmodel.Id > 1
                ? Ok(new { msg = "Report #" + viewmodel.Id + " added!" })
                : Ok(new { msg = "Report not added!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] ProblemViewModel viewmodel)
        {
            try
            {
                int retVal = await viewmodel.Update();
                return retVal switch
                {
                    1 => Ok(new { msg = "Report #" + viewmodel.Id + " updated!" }),
                    -1 => Ok(new { msg = "Report #" + viewmodel.Id + " not updated!" }),
                    -2 => Ok(new { msg = "Data is stale for report #" + viewmodel.Id + ", Report not updated!" }),
                    _ => Ok(new { msg = "Report #" + viewmodel.Id + " not updated!" }),
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ProblemViewModel viewmodel = new() { Id = id };
                return await viewmodel.Delete() == 1
                ? Ok(new { msg = "Report #" + id + " deleted!" })
                : Ok(new { msg = "Report #" + id + " not deleted!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
    }
}

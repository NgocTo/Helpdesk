using HelpdeskViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;

namespace HelpdeskWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                CallViewModel viewmodel = new();
                List<CallViewModel> allCalls = await viewmodel.GetAll();
                return Ok(allCalls);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                CallViewModel viewmodel = new();
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
        [HttpGet("employee/{id:int}")]
        public async Task<IActionResult> GetByEmployeeId(int id)
        {
            try
            {
                CallViewModel viewmodel = new();
                await viewmodel.GetByEmployeeId(id);
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
        [HttpGet("problem/{id:int}")]
        public async Task<IActionResult> GetByProblemId(int id)
        {
            try
            {
                CallViewModel viewmodel = new();
                await viewmodel.GetByProblemId(id);
                return Ok(viewmodel);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(CallViewModel viewmodel)
        {
            try
            {
                await viewmodel.Add();
                return viewmodel.Id > 1 ?
                    Ok(new { msg = "Call #" + viewmodel.Id + " added!" }) :
                    Ok(new { msg = "Call not added." });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                MethodBase.GetCurrentMethod()!.Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError); // something went wrong
            }
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CallViewModel viewmodel)
        {
            try
            {
                int val = await viewmodel.Update();
                return val switch
                {
                    1 => Ok(new { msg = "Call #" + viewmodel.Id + " updated!" }),
                    -1 => Ok(new { msg = "Call #" + viewmodel.Id + " not updated!" }),
                    -2 => Ok(new { msg = "Data is stale for call #" + viewmodel.Id + ", Call not updated!" }),
                    _ => Ok(new { msg = "Call #" + viewmodel.Id + " not updated!" }),
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
                CallViewModel viewmodel = new();
                return await viewmodel.Delete(id) == 1 ? 
                    Ok(new { msg = "Call #" + id + " deleted!" }) : 
                    Ok(new { msg = "Call #" + id + " not deleted!" });
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

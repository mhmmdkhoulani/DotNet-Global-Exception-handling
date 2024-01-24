using DotNetExcptionHandling.Interfaces;
using DotNetExcptionHandling.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetExcptionHandling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IDriverService _driverSerivce;

        public DriversController(IDriverService driverSerivce)
        {
            _driverSerivce = driverSerivce;
        }

        [HttpGet]
        public async Task<IActionResult> GetDrivers()
        {
            var drivers = await _driverSerivce.GetDriversAsync();
            return Ok(new ApiResponse<IEnumerable<Driver>>(drivers, "Drivers retrieved successfully"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriver(int id)
        {
            var driver = await _driverSerivce.GetDriverAsync(id);
            return Ok(new ApiResponse<Driver>(driver,"Driver retrived successfully!"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateDriver([FromBody] Driver model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiErrorResponse("Driver information is not valid", GetModelErrors()));
            
                
            var driver = await _driverSerivce.AddDriverAsync(model);
            return Ok(new ApiResponse<Driver>(driver, "Driver created successfully!"));  
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDriver([FromBody] Driver model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiErrorResponse("Driver information is not valid", GetModelErrors()));

            var driver = await _driverSerivce.UpdateDriverAsync(model);
            return Ok(new ApiResponse<Driver>(driver, "Driver updated successfully!"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(int id)
        {
            var driver = await _driverSerivce.DeleteDriver(id);
            return Ok(new ApiResponse());
        }

        private string[] GetModelErrors()
        {
            return ModelState
                .Where(x => x.Value.Errors.Any())
                .SelectMany(x => x.Value.Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();
        }
    }
}

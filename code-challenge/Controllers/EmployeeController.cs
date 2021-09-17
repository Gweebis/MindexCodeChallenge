using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;
        private readonly ICompensationService _compensationService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService, ICompensationService compensationService)
        {
            _logger = logger;
            _employeeService = employeeService;
            _compensationService = compensationService;
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpGet("{id}/DirectReports", Name = "getReportsById")]
        public IActionResult GetEmployeeReportsById(String id)
        {
            _logger.LogDebug($"Received employee get reports request for '{id}'");

            var employee = _employeeService.GetById(id);
            ReportingStructure report = new ReportingStructure(employee);

            foreach (Employee directReport in employee.DirectReports)
            {
                //go through each employee's direct reports in here - assuming we get passed employee and not employeeID?   
            }

            return Ok(report);//return reporting structure
        }

        //TODO: Save Compensation creation details
        [HttpPost("{id}/Compensation", Name = "createCompensation")]
        public IActionResult CreateCompensation(String id, [FromBody]Compensation comp)
        {
            _logger.LogDebug($"Received employee create compensation request for '{id}'");

            _compensationService.Create(comp);

            //send off newly created comp, utilizing the getCompensationById functionality to display confirmation of creation
            return CreatedAtRoute("getCompensation", new { id = comp.EmployeeId }, comp);

        }

        [HttpGet("{id}/Compensation", Name = "getCompensation")]
        public IActionResult GetCompensationById(String id)
        {
            _logger.LogDebug($"Received employee get compensation request for '{id}'");

            var compensation = _compensationService.GetById(id);
            if(compensation == null)
            {
                return NotFound();
            }

            return Ok(compensation);

        }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.Replace(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }
    }
}

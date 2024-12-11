using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Dtos.DepartmentDtos;
using DatabaseLab1.Domain.Dtos.EmployeeDtos;
using DatabaseLab1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseLab1.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;

    public EmployeesController(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
    }

    #region CRUD
    [HttpPost("create")]
    public async Task<IActionResult> Create(EmployeeCreateDto employeeDto)
    {
        var department = await _departmentRepository
            .GetByIdAsync(employeeDto.DepartmentId);

        if (department is null)
        {
            return NotFound();
        }

        var employeeEntity = employeeDto.ToEntity();
        var createResult = await _employeeRepository.CreateAsync(employeeEntity);

        if (!createResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("get-all")]
    public async Task<IEnumerable<Employee>> GetAll()
        => await _employeeRepository.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        return employee is null ? NotFound() : Ok(employee);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Remove(long id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee is null)
        {
            return NotFound();
        }

        var removeResult = await _employeeRepository.RemoveAsync(id);

        if (!removeResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(long id, EmployeeCreateDto employeeDto)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);

        if (employee is null)
        {
            return NotFound();
        }

        var updatedEmployeeData = employeeDto.ToEntity(id);
        var updateResult = await _employeeRepository.UpdateAsync(updatedEmployeeData);

        if (!updateResult)
        {
            return BadRequest();
        }

        return Ok();
    }
    #endregion

    [HttpGet("average-expense")]
    public async Task<IEnumerable<EmployeeAverageExpenseDto>> GetAverageExpensePerEmployee()
        => await _employeeRepository.GetAverageExpensePerEmployee();

    [HttpGet("count-in-departments")]
    public async Task<IEnumerable<DepartmentEmployeeCountDto>> GetDepartmentsEmployeeCountAboveAverage()
        => await _employeeRepository.GetDepartmentsEmployeeCountAboveAverage();
}

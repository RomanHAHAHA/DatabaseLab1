using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Dtos.DepartmentDtos;
using DatabaseLab1.Domain.Dtos.ExpenseDtos;
using DatabaseLab1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseLab1.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentsController(IDepartmentRepository repository)
    {
        _departmentRepository = repository;
    }

    #region CRUD
    [HttpPost("create")]
    public async Task<IActionResult> Create(DepartmentCreateDto departmentDto)
    {
        var departmentEntity = departmentDto.ToEntity();
        var createResult = await _departmentRepository.CreateAsync(departmentEntity);

        if (!createResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("get-all")]
    public async Task<IEnumerable<Department>> GetAll()
        => await _departmentRepository.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);

        return department is null ? NotFound() : Ok(department);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Remove(long id)
    {
        var department = await _departmentRepository.GetByIdAsync(id);

        if (department is null)
        {
            return NotFound();
        }

        var removeResult = await _departmentRepository.RemoveAsync(id);

        if (!removeResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(long id, DepartmentCreateDto departmentDto)
    {
        var actor = await _departmentRepository.GetByIdAsync(id);

        if (actor is null)
        {
            return NotFound();
        }

        var updatedDepartmentData = departmentDto.ToEntity(id);
        var updateResult = await _departmentRepository.UpdateAsync(updatedDepartmentData);

        if (!updateResult)
        {
            return BadRequest();
        }

        return Ok();
    }
    #endregion

    [HttpGet("expense-amount")]
    public async Task<IEnumerable<DepartmentExpenseDto>> GetExpenseAmountOfEachDepartment() 
        => await _departmentRepository.GetExpenseAmountOfEachDepartment();

    [HttpGet("employee-count")]
    public async Task<IEnumerable<DepartmentEmployeeCountDto>> GetEmployeeCountPerDepartment()
        => await _departmentRepository.GetEmployeeCountPerDepartment();

    [HttpGet("all-expenses")]
    public async Task<IEnumerable<DepartmentExpenseApprovalDto>> GetApprovedAndNotApprovedExpenses()
        => await _departmentRepository.GetApprovedAndNotApprovedExpenses();

    [HttpGet("total-expenses-data")]
    public async Task<IEnumerable<DepartmentTotalExpenseDto>> GetFilteredTotalExpensesPerDepartment()
        => await _departmentRepository.GetFilteredTotalExpensesPerDepartment();

    [HttpGet("expenses-above-threshold")]
    public async Task<IEnumerable<DepartmentExpenseDto>> GetDepartmentsWithExpensesAboveThreshold(
        decimal threshold)
        => await _departmentRepository.GetDepartmentsWithExpensesAboveThreshold(threshold);

    [HttpGet("above-average-employees")]
    public async Task<IEnumerable<Department>> GetDepartmentsAboveAverageEmployees()
        => await _departmentRepository.GetDepartmentsAboveAverageEmployees();
}

using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Dtos.ExpenseTypeDtos;
using DatabaseLab1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseLab1.API.Controllers;

[Route("api/expense-types")]
[ApiController]
public class ExpenseTypesController : ControllerBase
{
    private readonly IExpenseTypeRepository _repository;

    public ExpenseTypesController(IExpenseTypeRepository repository)
    {
        _repository = repository;
    }

    #region CRUD
    [HttpPost("create")]
    public async Task<IActionResult> Create(ExpenseTypeCreateDto expenseTypeDto)
    {
        var expenseTypeEntity = expenseTypeDto.ToEntity();
        var createResult = await _repository.CreateAsync(expenseTypeEntity);

        if (!createResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("get-all")]
    public async Task<IEnumerable<ExpenseType>> GetAll()
        => await _repository.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var expenseType = await _repository.GetByIdAsync(id);

        return expenseType is null ? NotFound() : Ok(expenseType);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Remove(long id)
    {
        var expenseType = await _repository.GetByIdAsync(id);

        if (expenseType is null)
        {
            return NotFound();
        }

        var removeResult = await _repository.RemoveAsync(id);

        if (!removeResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(long id, ExpenseTypeCreateDto expenseTypeDto)
    {
        var expenseType = await _repository.GetByIdAsync(id);

        if (expenseType is null)
        {
            return NotFound();
        }

        var updatedExpenseTypeData = expenseTypeDto.ToEntity(id);
        var updateResult = await _repository.UpdateAsync(updatedExpenseTypeData);

        if (!updateResult)
        {
            return BadRequest();
        }

        return Ok();
    }
    #endregion

    [HttpGet("average-limit-per-type")]
    public async Task<IEnumerable<ExpenseTypeCountDto>> GetAverageLimitPerExpenseType()
        => await _repository.GetAverageLimitPerExpenseType();

    [HttpGet("max-approved-per-type")]
    public async Task<IEnumerable<ExpenseTypeMaxDto>> GetMaxApprovedExpensesPerType()
        => await _repository.GetMaxApprovedExpensesPerType();

    [HttpGet("unused-by-department/{departmentId}")]
    public async Task<IEnumerable<ExpenseType>> GetUnusedExpenseTypesInDepartment(long departmentId)
        => await _repository.GetUnusedExpenseTypesInDepartment(departmentId);
}

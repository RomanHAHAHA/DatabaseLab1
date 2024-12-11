using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Dtos.ExpenseDtos;
using DatabaseLab1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseLab1.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseRepository _repository;

    public ExpensesController(IExpenseRepository repository)
    {
        _repository = repository;
    }

    #region CRUD
    [HttpPost("create")]
    public async Task<IActionResult> Create(ExpenseCreateDto expenseDto)
    {
        var expenseEntity = expenseDto.ToEntity();
        var createResult = await _repository.CreateAsync(expenseEntity);

        if (!createResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("get-all")]
    public async Task<IEnumerable<Expense>> GetAll()
        => await _repository.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var expense = await _repository.GetByIdAsync(id);

        return expense is null ? NotFound() : Ok(expense);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Remove(long id)
    {
        var expense = await _repository.GetByIdAsync(id);

        if (expense is null)
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
    public async Task<IActionResult> Update(long id, ExpenseCreateDto expenseDto)
    {
        var expense = await _repository.GetByIdAsync(id);

        if (expense is null)
        {
            return NotFound();
        }

        var updatedExpenseData = expenseDto.ToEntity(id);
        var updateResult = await _repository.UpdateAsync(updatedExpenseData);

        if (!updateResult)
        {
            return BadRequest();
        }

        return Ok();
    }
    #endregion

    [HttpGet("exceeding")]
    public async Task<IEnumerable<Expense>> GetExpensesExceedingTypeLimit()
        => await _repository.GetExpensesExceedingTypeLimit();

    [HttpGet("above-avg/{expenseTypeId}")]
    public async Task<IEnumerable<Expense>> GetExpensesAboveAverageForType(long expenseTypeId)
        => await _repository.GetExpensesAboveAverageForType(expenseTypeId);
}

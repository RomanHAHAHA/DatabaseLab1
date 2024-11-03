using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Dtos;
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

    [HttpGet("get-by-department-id")]
    public async Task<IEnumerable<Expense>> GetByDepartmentId()
        => await _repository.GetByDepartmentId();

    [HttpGet("get-by-expense-type-id")]
    public async Task<IEnumerable<Expense>> GetByExpenseTypeId()
        => await _repository.GetByExpenseTypeId();

    [HttpGet("get-by-amount")]
    public async Task<IEnumerable<Expense>> GetByAmount()
        => await _repository.GetByAmount();

    [HttpGet("get-by-date")]
    public async Task<IEnumerable<Expense>> GetByDate()
        => await _repository.GetByDate();

    [HttpGet("get-by-code-length")]
    public async Task<IEnumerable<Expense>> GetByCodeLength()
        => await _repository.GetByCodeLength();
}

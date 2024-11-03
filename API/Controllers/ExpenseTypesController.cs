using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Dtos;
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

    [HttpGet("by-limit-amount")]
    public async Task<IEnumerable<ExpenseType>> GetByLimitAmount()
        => await _repository.GetByLimitAmount();

    [HttpGet("by-description-length")]
    public async Task<IEnumerable<ExpenseType>> GetByDescriptionLength()
        => await _repository.GetByDescriptionLettersCount();

    [HttpGet("by-name-start")]
    public async Task<IEnumerable<ExpenseType>> GetByNameStart()
        => await _repository.GetByNameStart();
}

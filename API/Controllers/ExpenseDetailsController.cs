using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Dtos.ExpenseDetailsDtos;
using DatabaseLab1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseLab1.API.Controllers;

[Route("api/expense-details")]
[ApiController]
public class ExpenseDetailsController : ControllerBase
{
    private readonly IExpenseDetailsRepository _expenseDetailsRepository;
    private readonly IExpenseRepository _expenseRepository;

    public ExpenseDetailsController(
        IExpenseDetailsRepository expenseDetailsRepository,
        IExpenseRepository expenseRepository)
    {
        _expenseDetailsRepository = expenseDetailsRepository;
        _expenseRepository = expenseRepository;
    }

    #region CRUD
    [HttpPost("create")]
    public async Task<IActionResult> Create(ExpenseDetailsCreateDto expenseDetailsDto)
    {
        var expense = await _expenseRepository
            .GetByIdAsync(expenseDetailsDto.ExpenseDetailsId);

        if (expense is null)
        {
            return NotFound();
        }

        var detailsEntity = expenseDetailsDto.ToEntity();
        var createResult = await _expenseDetailsRepository
            .CreateAsync(detailsEntity);

        if (!createResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("get-all")]
    public async Task<IEnumerable<ExpenseDetails>> GetAll()
        => await _expenseDetailsRepository.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var expenseDetail = await _expenseDetailsRepository.GetByIdAsync(id);

        return expenseDetail is null ? NotFound() : Ok(expenseDetail);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Remove(long id)
    {
        var expenseDetail = await _expenseDetailsRepository.GetByIdAsync(id);

        if (expenseDetail is null)
        {
            return NotFound();
        }

        var removeResult = await _expenseDetailsRepository.RemoveAsync(id);

        if (!removeResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(
        long id, 
        ExpenseDetailsCreateDto expenseDetailsDto)
    {
        var expenseDetails = await _expenseDetailsRepository.GetByIdAsync(id);

        if (expenseDetails is null)
        {
            return NotFound();
        }

        var updatedExpenseDetailsData = expenseDetailsDto.ToEntity(id);
        var updateResult = await _expenseDetailsRepository.UpdateAsync(updatedExpenseDetailsData);

        if (!updateResult)
        {
            return BadRequest();
        }

        return Ok();
    }
    #endregion

    [HttpGet("approved-last-month")]
    public async Task<IEnumerable<ExpenseDetails>> GetApprovedExpensesLastMonth()
        => await _expenseDetailsRepository.GetApprovedExpensesLastMonth();
}

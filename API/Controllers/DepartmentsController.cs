using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.Domain.Dtos;
using DatabaseLab1.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseLab1.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentRepository _repository;

    public DepartmentsController(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(DepartmentCreateDto departmentDto)
    {
        var departmentEntity = departmentDto.ToEntity();
        var createResult = await _repository.CreateAsync(departmentEntity);

        if (!createResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("get-all")]
    public async Task<IEnumerable<Department>> GetAll()
        => await _repository.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(long id)
    {
        var department = await _repository.GetByIdAsync(id);

        return department is null ? NotFound() : Ok(department);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Remove(long id)
    {
        var department = await _repository.GetByIdAsync(id);

        if (department is null)
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
    public async Task<IActionResult> Update(long id, DepartmentCreateDto departmentDto)
    {
        var actor = await _repository.GetByIdAsync(id);

        if (actor is null)
        {
            return NotFound();
        }

        var updatedDepartmentData = departmentDto.ToEntity(id);
        var updateResult = await _repository.UpdateAsync(updatedDepartmentData);

        if (!updateResult)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet("by-employee-count")]
    public async Task<IEnumerable<Department>> GetByEmplpoyeeCount() 
        => await _repository.GetByEmployeeCount();

    [HttpGet("by-name-start")]
    public async Task<IEnumerable<Department>> GetByNameStart() 
        => await _repository.GetByNameStart();
}

using DatabaseLab1.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLab1.Domain.Dtos.EmployeeDtos;

public class EmployeeCreateDto
{
    [Required]
    [MinLength(5)]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MinLength(5)]
    [MaxLength(200)]
    public string Position { get; set; } = string.Empty;

    [Required]
    [Range(0, int.MaxValue)]
    public long DepartmentId { get; set; }

    public Employee ToEntity()
    {
        return new Employee
        {
            FullName = FullName,
            Position = Position,
            DepartmentId = DepartmentId
        };
    }

    public Employee ToEntity(long id)
    {
        var employee = ToEntity();
        employee.EmployeeId = id;

        return employee;
    }
}

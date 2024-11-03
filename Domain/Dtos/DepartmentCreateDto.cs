using DatabaseLab1.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLab1.Domain.Dtos;

public class DepartmentCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0, 1000000)]
    public int EmployeeCount { get; set; }

    public Department ToEntity()
    {
        return new Department
        {
            Name = Name,
            EmployeeCount = EmployeeCount
        };
    }

    public Department ToEntity(long id)
    {
        var department = ToEntity();
        department.DepartmentId = id;

        return department;
    }
}

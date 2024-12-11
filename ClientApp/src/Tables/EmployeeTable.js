import React from 'react';
import { Table, Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';

const EmployeeTable = ({ employees, handleEditEmployee, deleteEmployee, toggleDropdown, dropdownOpen }) => {
    // Проверяем, что employees является массивом, прежде чем использовать map
    const employeeList = Array.isArray(employees) ? employees : [];

    return (
        <Table striped bordered>
            <thead>
                <tr>
                    <th>EmployeeId</th>
                    <th>Full Name</th>
                    <th>Position</th>
                    <th>Department</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                {employeeList.map(employee => (
                    <tr key={employee.employeeId}>
                        <td>{employee.employeeId}</td>
                        <td>{employee.fullName}</td>
                        <td>{employee.position}</td>
                        <td>{employee.departmentId}</td> 
                        <td>
                            <Dropdown isOpen={dropdownOpen === employee.employeeId} toggle={() => toggleDropdown(employee.employeeId)}>
                                <DropdownToggle caret>
                                    Actions
                                </DropdownToggle>
                                <DropdownMenu>
                                    <DropdownItem onClick={() => handleEditEmployee(employee)}>Update</DropdownItem>
                                    <DropdownItem onClick={() => deleteEmployee(employee.employeeId)}>Delete</DropdownItem>
                                </DropdownMenu>
                            </Dropdown>
                        </td>
                    </tr>
                ))}
            </tbody>
        </Table>
    );
};

export default EmployeeTable;

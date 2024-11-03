import React from 'react';
import { Table, Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';

const DepartmentTable = ({ departments, handleEditDepartment, deleteDepartment, toggleDropdown, dropdownOpen }) => {
    return (
        <Table striped bordered>
            <thead>
                <tr>
                    <th>DepartmentId</th>
                    <th>Name</th>
                    <th>EmployeeCount</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                {departments.map(department => (
                    <tr key={department.departmentId}>
                        <td>{department.departmentId}</td>
                        <td>{department.name}</td>
                        <td>{department.employeeCount}</td>
                        <td>
                            <Dropdown isOpen={dropdownOpen === department.departmentId} toggle={() => toggleDropdown(department.departmentId)}>
                                <DropdownToggle caret>
                                    Actions
                                </DropdownToggle>
                                <DropdownMenu>
                                    <DropdownItem onClick={() => handleEditDepartment(department)}>Update</DropdownItem>
                                    <DropdownItem onClick={() => deleteDepartment(department.departmentId)}>Delete</DropdownItem>
                                </DropdownMenu>
                            </Dropdown>
                        </td>
                    </tr>
                ))}
            </tbody>
        </Table>
    );
};

export default DepartmentTable;

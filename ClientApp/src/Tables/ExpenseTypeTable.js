import React from 'react';
import { Table, Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';

const ExpenseTypeTable = ({ expenseTypes, handleEditExpenseType, deleteExpenseType, toggleDropdown, dropdownOpen }) => {
    return (
        <Table striped bordered>
            <thead>
                <tr>
                    <th>ExpenseTypeId</th>
                    <th>Name</th>
                    <th>Description</th>
                    <th>Limit Amount</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                {expenseTypes.map(expenseType => (
                    <tr key={expenseType.expenseTypeId}>
                        <td>{expenseType.expenseTypeId}</td>
                        <td>{expenseType.name}</td>
                        <td>{expenseType.description}</td>
                        <td>{expenseType.limitAmount}</td>
                        <td>
                            <Dropdown isOpen={dropdownOpen === expenseType.expenseTypeId} toggle={() => toggleDropdown(expenseType.expenseTypeId)}>
                                <DropdownToggle caret>
                                    Actions
                                </DropdownToggle>
                                <DropdownMenu>
                                    <DropdownItem onClick={() => handleEditExpenseType(expenseType)}>Update</DropdownItem>
                                    <DropdownItem onClick={() => deleteExpenseType(expenseType.expenseTypeId)}>Delete</DropdownItem>
                                </DropdownMenu>
                            </Dropdown>
                        </td>
                    </tr>
                ))}
            </tbody>
        </Table>
    );
};

export default ExpenseTypeTable;

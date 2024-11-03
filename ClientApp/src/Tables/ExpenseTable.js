import React from 'react';
import { Table, Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';

const ExpenseTable = ({ expenses, handleEditExpense, deleteExpense, toggleDropdown, dropdownOpen }) => {
    return (
        <Table striped bordered>
            <thead>
                <tr>
                    <th>ExpenseId</th>
                    <th>Code</th>
                    <th>ExpenseTypeId</th>
                    <th>DepartmentId</th>
                    <th>Amount</th>
                    <th>Date</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                {expenses.map(expense => (
                    <tr key={expense.expenseId}>
                        <td>{expense.expenseId}</td>
                        <td>{expense.code}</td>
                        <td>{expense.expenseTypeId}</td>
                        <td>{expense.departmentId}</td>
                        <td>{expense.amount}</td>
                        <td>{new Date(expense.date).toLocaleDateString()}</td>
                        <td>
                            <Dropdown isOpen={dropdownOpen === expense.expenseId} toggle={() => toggleDropdown(expense.expenseId)}>
                                <DropdownToggle caret>
                                    Actions
                                </DropdownToggle>
                                <DropdownMenu>
                                    <DropdownItem onClick={() => handleEditExpense(expense)}>Update</DropdownItem>
                                    <DropdownItem onClick={() => deleteExpense(expense.expenseId)}>Delete</DropdownItem>
                                </DropdownMenu>
                            </Dropdown>
                        </td>
                    </tr>
                ))}
            </tbody>
        </Table>
    );
};

export default ExpenseTable;

import React from 'react';
import { Table, Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';

const ExpenseDetailsTable = ({ expenseDetails, handleEditExpenseDetails, deleteExpenseDetails, toggleDropdown, dropdownOpen }) => {
    return (
        <Table striped bordered>
            <thead>
                <tr>
                    <th>ExpenseDetailsId</th>
                    <th>Notes</th>
                    <th>IsApproved</th>
                    <th>ApprovalDate</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                {expenseDetails.map(detail => (
                    <tr key={detail.expenseDetailsId}>
                        <td>{detail.expenseDetailsId}</td>
                        <td>{detail.notes}</td>
                        <td>{detail.isApproved ? 'Yes' : 'No'}</td>
                        <td>{new Date(detail.approvalDate).toLocaleDateString()}</td>
                        <td>
                            <Dropdown isOpen={dropdownOpen === detail.expenseDetailsId} toggle={() => toggleDropdown(detail.expenseDetailsId)}>
                                <DropdownToggle caret>
                                    Actions
                                </DropdownToggle>
                                <DropdownMenu>
                                    <DropdownItem onClick={() => handleEditExpenseDetails(detail)}>Update</DropdownItem>
                                    <DropdownItem onClick={() => deleteExpenseDetails(detail.expenseDetailsId)}>Delete</DropdownItem>
                                </DropdownMenu>
                            </Dropdown>
                        </td>
                    </tr>
                ))}
            </tbody>
        </Table>
    );
};

export default ExpenseDetailsTable;

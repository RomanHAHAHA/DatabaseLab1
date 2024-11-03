import React, { useEffect, useState } from 'react';
import ExpenseTable from '../Tables/ExpenseTable';
import ExpenseForm from '../Forms/ExpenseForm';

const Expenses = () => {
    const [expenses, setExpenses] = useState([]);
    const [expenseToEdit, setExpenseToEdit] = useState(null);
    const [dropdownOpen, setDropdownOpen] = useState(null);
    
    const toggleDropdown = (expenseId) => {
        setDropdownOpen(prev => (prev === expenseId ? null : expenseId));
    };

    const fetchAllExpenses = async () => {
        try {
            const response = await fetch('/api/expenses/get-all');
            if (!response.ok) throw new Error('Failed to fetch expenses');
            const data = await response.json();
            setExpenses(data);
        } catch (error) {
            console.error(error);
        }
    };

    const deleteExpense = async (id) => {
        try {
            const response = await fetch(`/api/expenses/delete/${id}`, { method: 'DELETE' });
            if (!response.ok) throw new Error('Failed to delete expense');
            fetchAllExpenses();
        } catch (error) {
            console.error(error);
        }
    };

    const handleEditExpense = (expense) => {
        setExpenseToEdit(expense);
    };

    // Функции для получения расходов по различным критериям
    const fetchByDepartmentId = async () => {
        try {
            const response = await fetch('/api/expenses/get-by-department-id');
            if (!response.ok) throw new Error('Failed to fetch expenses by department ID');
            const data = await response.json();
            setExpenses(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchByExpenseTypeId = async () => {
        try {
            const response = await fetch('/api/expenses/get-by-expense-type-id');
            if (!response.ok) throw new Error('Failed to fetch expenses by expense type ID');
            const data = await response.json();
            setExpenses(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchByAmount = async () => {
        try {
            const response = await fetch('/api/expenses/get-by-amount');
            if (!response.ok) throw new Error('Failed to fetch expenses by amount');
            const data = await response.json();
            setExpenses(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchByDate = async () => {
        try {
            const response = await fetch('/api/expenses/get-by-date');
            if (!response.ok) throw new Error('Failed to fetch expenses by date');
            const data = await response.json();
            setExpenses(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchByCodeLength = async () => {
        try {
            const response = await fetch('/api/expenses/get-by-code-length');
            if (!response.ok) throw new Error('Failed to fetch expenses by code length');
            const data = await response.json();
            setExpenses(data);
        } catch (error) {
            console.error(error);
        }
    };

    useEffect(() => {
        fetchAllExpenses();
    }, []);

    return (
        <div className="container mt-4">
            <div className="row">
                <div className="col-md-4">
                    <ExpenseForm
                        expenseToUpdate={expenseToEdit}
                        onExpenseCreated={fetchAllExpenses}
                        onExpenseUpdated={fetchAllExpenses}
                    />
                </div>
                <div className="col-md-8">
                    <h4>Expenses List</h4>
                    <div className="mb-3">
                        <div className="d-flex flex-column">
                            <button className="btn btn-primary mb-2" onClick={fetchByDepartmentId}>
                                Get Department ID = 8
                            </button>
                            <button className="btn btn-secondary mb-2" onClick={fetchByExpenseTypeId}>
                                Get Expense Type ID = 7
                            </button>
                            <button className="btn btn-success mb-2" onClick={fetchByAmount}>
                                Get Amount more than 500
                            </button>
                            <button className="btn btn-info mb-2" onClick={fetchByDate}>
                                Get Date more than 01.01.2023
                            </button>
                            <button className="btn btn-warning" onClick={fetchByCodeLength}>
                                Get Code Length more than 10
                            </button>
                        </div>
                    </div>
                    <ExpenseTable
                        expenses={expenses}
                        handleEditExpense={handleEditExpense}
                        deleteExpense={deleteExpense}
                        toggleDropdown={toggleDropdown}
                        dropdownOpen={dropdownOpen}
                    />
                </div>
            </div>
        </div>
    );
};

export default Expenses;

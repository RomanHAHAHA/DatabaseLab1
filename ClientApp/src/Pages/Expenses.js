import React, { useEffect, useState } from 'react';
import ExpenseTable from '../Tables/ExpenseTable';
import ExpenseForm from '../Forms/ExpenseForm';
import TableGenerator from '../components/TableGenerator'; 

const Expenses = () => {
    const [expenses, setExpenses] = useState([]); 
    const [queryResult, setQueryResult] = useState([]); 
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

    const fetchExpensesExceedingTypeLimit = async () => {
        try {
            const response = await fetch('/api/expenses/exceeding');
            if (!response.ok) throw new Error('Failed to fetch expenses exceeding type limit');
            const data = await response.json();
            setQueryResult(data); 
        } catch (error) {
            console.error(error);
        }
    };

    const fetchExpensesAboveAverageForType = async (expenseTypeId) => {
        try {
            const response = await fetch(`/api/expenses/above-avg/${expenseTypeId}`);
            if (!response.ok) throw new Error('Failed to fetch expenses above average for type');
            const data = await response.json();
            setQueryResult(data); 
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
                    <div className="mt-3">
                        <button 
                            className="btn btn-primary btn-block mb-2" 
                            onClick={fetchExpensesExceedingTypeLimit}
                        >
                            Отримати витрати, ліміт яких більше за встановлений(з таблиці ExpenseTypes)
                        </button>
                        <button 
                            className="btn btn-primary btn-block mb-2" 
                            onClick={() => {
                                const expenseTypeId = prompt('Enter Expense Type ID:'); 
                                if (expenseTypeId) fetchExpensesAboveAverageForType(expenseTypeId);
                            }}
                        >
                            Отримати витрати з певним типом, значення якиз більше за середнє
                        </button>
                    </div>
                </div>
                <div className="col-md-8">
                    <h4>Expenses List</h4>
                    <ExpenseTable
                        expenses={expenses}
                        handleEditExpense={handleEditExpense}
                        deleteExpense={deleteExpense}
                        toggleDropdown={toggleDropdown}
                        dropdownOpen={dropdownOpen}
                    />
                    {queryResult.length > 0 && (
                        <div className="mt-4">
                            <h5>Query Results</h5>
                            <TableGenerator data={queryResult} />
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default Expenses;

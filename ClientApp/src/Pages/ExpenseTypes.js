import React, { useEffect, useState } from 'react';
import ExpenseTypeTable from '../Tables/ExpenseTypeTable.js';
import ExpenseTypeForm from '../Forms/ExpenseTypeForm';
import TableGenerator from '../components/TableGenerator'; 

const ExpenseTypes = () => {
    const [expenseTypes, setExpenseTypes] = useState([]);
    const [additionalData, setAdditionalData] = useState([]);
    const [expenseTypeToEdit, setExpenseTypeToEdit] = useState(null);
    const [dropdownOpen, setDropdownOpen] = useState(null);

    const toggleDropdown = (expenseTypeId) => {
        setDropdownOpen(prev => (prev === expenseTypeId ? null : expenseTypeId));
    };

    const fetchAllExpenseTypes = async () => {
        try {
            const response = await fetch('/api/expense-types/get-all');
            if (!response.ok) throw new Error('Failed to fetch expense types');
            const data = await response.json();
            setExpenseTypes(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchAverageLimitPerExpenseType = async () => {
        try {
            const response = await fetch('/api/expense-types/average-limit-per-type');
            if (!response.ok) throw new Error('Failed to fetch average limit per expense type');
            const data = await response.json();
            setAdditionalData(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchMaxApprovedExpensesPerType = async () => {
        try {
            const response = await fetch('/api/expense-types/max-approved-per-type');
            if (!response.ok) throw new Error('Failed to fetch max approved expenses per type');
            const data = await response.json();
            setAdditionalData(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchUnusedExpenseTypesInDepartment = async () => {
        const departmentId = prompt('Enter Expense Type ID:');
        try {
            const response = await fetch(`/api/expense-types/unused-by-department/${departmentId}`);
            if (!response.ok) throw new Error('Failed to fetch unused expense types in department');
            const data = await response.json();
            setAdditionalData(data);
        } catch (error) {
            console.error(error);
        }
    };

    const deleteExpenseType = async (id) => {
        try {
            const response = await fetch(`/api/expense-types/delete/${id}`, { method: 'DELETE' });
            if (!response.ok) throw new Error('Failed to delete expense type');
            fetchAllExpenseTypes();
        } catch (error) {
            console.error(error);
        }
    };

    const handleEditExpenseType = (expenseType) => {
        setExpenseTypeToEdit(expenseType);
    };

    useEffect(() => {
        fetchAllExpenseTypes();
    }, []);

    return (
        <div className="container mt-4">
            <div className="row">
                <div className="col-md-4">
                    <ExpenseTypeForm
                        expenseTypeToUpdate={expenseTypeToEdit}
                        onExpenseTypeCreated={fetchAllExpenseTypes}
                        onExpenseTypeUpdated={fetchAllExpenseTypes}
                    />
                    <div className="mt-3">
                        <h5>Additional Queries</h5>
                        <button className="btn btn-primary btn-block" onClick={fetchAverageLimitPerExpenseType}>
                            Отримати всі типи витрат, з кількістю самих витрат, що прив'язані до цього типу
                        </button>
                        <button className="btn btn-secondary btn-block" onClick={fetchMaxApprovedExpensesPerType}>
                            Отримати типи витрат, що були одобрені з максимальним знаенням
                        </button>
                        <button
                            className="btn btn-info btn-block"
                            onClick={() => fetchUnusedExpenseTypesInDepartment()} 
                        >
                            Отримати типи витрат, що НЕ використовуються в певному відділені
                        </button>
                    </div>
                </div>
                <div className="col-md-8">
                    <h4>Expense Types List</h4>
                    <ExpenseTypeTable
                        expenseTypes={expenseTypes}
                        handleEditExpenseType={handleEditExpenseType}
                        deleteExpenseType={deleteExpenseType}
                        toggleDropdown={toggleDropdown}
                        dropdownOpen={dropdownOpen}
                    />
                    <div className="mt-4">
                        <h4>Query Results</h4>
                        <TableGenerator data={additionalData} />
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ExpenseTypes;

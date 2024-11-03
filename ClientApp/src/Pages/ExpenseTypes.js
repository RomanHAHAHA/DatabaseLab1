import React, { useEffect, useState } from 'react';
import ExpenseTypeTable from '../Tables/ExpenseTypeTable.js';
import ExpenseTypeForm from '../Forms/ExpenseTypeForm';

const ExpenseTypes = () => {
    const [expenseTypes, setExpenseTypes] = useState([]);
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

    const fetchByLimitAmount = async () => {
        try {
            const response = await fetch('/api/expense-types/by-limit-amount');
            if (!response.ok) throw new Error('Failed to fetch expense types by limit amount');
            const data = await response.json();
            setExpenseTypes(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchByDescriptionLength = async () => {
        try {
            const response = await fetch('/api/expense-types/by-description-length');
            if (!response.ok) throw new Error('Failed to fetch expense types by description length');
            const data = await response.json();
            setExpenseTypes(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchByNameStart = async () => {
        try {
            const response = await fetch('/api/expense-types/by-name-start');
            if (!response.ok) throw new Error('Failed to fetch expense types by name start');
            const data = await response.json();
            setExpenseTypes(data);
        } catch (error) {
            console.error(error);
        }
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
                </div>
                <div className="col-md-8">
                    <h4>Expense Types List</h4>
                    <div className="mb-3">
                        <button className="btn btn-primary me-2" onClick={fetchByLimitAmount}>
                            Get Limit Amount more 200
                        </button>
                        <button className="btn btn-secondary me-2" onClick={fetchByDescriptionLength}>
                            Get Description Length more 20
                        </button>
                        <button className="btn btn-success" onClick={fetchByNameStart}>
                            Get Name Start with 'a'
                        </button>
                    </div>
                    <ExpenseTypeTable
                        expenseTypes={expenseTypes}
                        handleEditExpenseType={handleEditExpenseType}
                        deleteExpenseType={deleteExpenseType}
                        toggleDropdown={toggleDropdown} 
                        dropdownOpen={dropdownOpen} 
                    />
                </div>
            </div>
        </div>
    );
};

export default ExpenseTypes;

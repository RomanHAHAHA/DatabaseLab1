import React, { useState, useEffect } from 'react';

const ExpenseTypeForm = ({ onExpenseTypeCreated, expenseTypeToUpdate, onExpenseTypeUpdated }) => {
    const [id, setId] = useState(null);
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [limitAmount, setLimitAmount] = useState(0);
    const [errors, setErrors] = useState({});

    useEffect(() => {
        if (expenseTypeToUpdate) {
            setId(expenseTypeToUpdate.expenseTypeId);
            setName(expenseTypeToUpdate.name);
            setDescription(expenseTypeToUpdate.description);
            setLimitAmount(expenseTypeToUpdate.limitAmount);
        }
    }, [expenseTypeToUpdate]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        const expenseType = {
            name: name,
            description: description,
            limitAmount: parseFloat(limitAmount),
        };

        const response = id 
            ? await fetch(`/api/expense-types/update/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(expenseType),
            })
            : await fetch('/api/expense-types/create', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(expenseType),
            });

        if (response.ok) {
            if (id) {
                onExpenseTypeUpdated();
            } else {
                onExpenseTypeCreated();
            }
            resetForm();
        } else {
            const errorData = await response.json();
            if (errorData.errors) {
                setErrors(errorData.errors);
            }
        }
    };

    const resetForm = () => {
        setId(null);
        setName('');
        setDescription('');
        setLimitAmount(0);
        setErrors({});
    };

    return (
        <form onSubmit={handleSubmit} className="p-3 bg-light shadow-sm rounded">
            {errors.general && (
                <div className="text-danger mb-3">
                    {errors.general.map((error, index) => (
                        <div key={index}>{error}</div>
                    ))}
                </div>
            )}
            <div className="mb-3">
                <label htmlFor="name" className="form-label">Expense Type Name</label>
                <input
                    type="text"
                    id="name"
                    className="form-control"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                />
                {errors['Name'] && (
                    <div className="text-danger">{errors['Name'].join(', ')}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="description" className="form-label">Description</label>
                <input
                    type="text"
                    id="description"
                    className="form-control"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                />
                {errors['Description'] && (
                    <div className="text-danger">{errors['Description'].join(', ')}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="limitAmount" className="form-label">Limit Amount</label>
                <input
                    type="number"
                    id="limitAmount"
                    className="form-control"
                    value={limitAmount}
                    onChange={(e) => setLimitAmount(e.target.value)}
                />
                {errors['LimitAmount'] && (
                    <div className="text-danger">{errors['LimitAmount'].join(', ')}</div>
                )}
            </div>
            <button type="submit" className="btn btn-primary">
                {id ? 'Update Expense Type' : 'Create Expense Type'}
            </button>
            <button type="button" className="btn btn-secondary ms-2" onClick={resetForm}>
                Reset
            </button>
        </form>
    );
};

export default ExpenseTypeForm;

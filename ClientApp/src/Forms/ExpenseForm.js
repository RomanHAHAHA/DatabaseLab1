import React, { useState, useEffect } from 'react';

const ExpenseForm = ({ onExpenseCreated, expenseToUpdate, onExpenseUpdated }) => {
    const [id, setId] = useState(null);
    const [code, setCode] = useState('');
    const [expenseTypeId, setExpenseTypeId] = useState(0);
    const [departmentId, setDepartmentId] = useState(0);
    const [amount, setAmount] = useState(0);
    const [date, setDate] = useState(''); 
    const [errors, setErrors] = useState({});

    useEffect(() => {
        if (expenseToUpdate) {
            setId(expenseToUpdate.expenseId);
            setCode(expenseToUpdate.code);
            setExpenseTypeId(expenseToUpdate.expenseTypeId);
            setDepartmentId(expenseToUpdate.departmentId);
            setAmount(expenseToUpdate.amount);
            setDate(expenseToUpdate.date); 
        }
    }, [expenseToUpdate]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        const expense = {
            expenseCode: code,
            expenseTypeId: parseInt(expenseTypeId),
            departmentId: parseInt(departmentId),
            amount: parseFloat(amount),
            date, 
        };

        const response = id 
            ? await fetch(`/api/expenses/update/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(expense),
            })
            : await fetch('/api/expenses/create', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(expense),
            });

        if (response.ok) {
            if (id) {
                onExpenseUpdated();
            } else {
                onExpenseCreated();
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
        setCode('');
        setExpenseTypeId(0);
        setDepartmentId(0);
        setAmount(0);
        setDate(''); 
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
                <label htmlFor="code" className="form-label">Expense Code</label>
                <input
                    type="text"
                    id="code"
                    className="form-control"
                    value={code}
                    onChange={(e) => setCode(e.target.value)}
                />
                {errors['ExpenseCode'] && (
                    <div className="text-danger">{errors['ExpenseCode'].join(', ')}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="expenseTypeId" className="form-label">Expense Type ID</label>
                <input
                    type="number"
                    id="expenseTypeId"
                    className="form-control"
                    value={expenseTypeId}
                    onChange={(e) => setExpenseTypeId(parseInt(e.target.value))}
                />
                {errors['ExpenseTypeId'] && (
                    <div className="text-danger">{errors['ExpenseTypeId'].join(', ')}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="departmentId" className="form-label">Department ID</label>
                <input
                    type="number"
                    id="departmentId"
                    className="form-control"
                    value={departmentId}
                    onChange={(e) => setDepartmentId(parseInt(e.target.value))}
                />
                {errors['DepartmentId'] && (
                    <div className="text-danger">{errors['DepartmentId'].join(', ')}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="amount" className="form-label">Amount</label>
                <input
                    type="number"
                    id="amount"
                    className="form-control"
                    value={amount}
                    onChange={(e) => setAmount(parseFloat(e.target.value))}
                />
                {errors['Amount'] && (
                    <div className="text-danger">{errors['Amount'].join(', ')}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="date" className="form-label">Date</label>
                <input
                    type="text"
                    id="date"
                    className="form-control"
                    value={date}
                    onChange={(e) => setDate(e.target.value)} 
                />
                {errors['Date'] && (
                    <div className="text-danger">{errors['Date'].join(', ')}</div>
                )}
            </div>
            <button type="submit" className="btn btn-primary">
                {id ? 'Update Expense' : 'Create Expense'}
            </button>
            <button type="button" className="btn btn-secondary ms-2" onClick={resetForm}>
                Reset
            </button>
        </form>
    );
};

export default ExpenseForm;

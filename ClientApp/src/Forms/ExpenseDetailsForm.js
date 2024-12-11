import React, { useState, useEffect } from 'react';
import Swal from 'sweetalert2';

const ExpenseDetailsForm = ({ onExpenseDetailCreated, expenseDetailsToUpdate, onExpenseDetailUpdated }) => {
    const [expenseDetailsId, setExpenseDetailsId] = useState(0);
    const [notes, setNotes] = useState('');
    const [isApproved, setIsApproved] = useState(false);
    const [approvalDate, setApprovalDate] = useState('');
    const [errors, setErrors] = useState({});

    useEffect(() => {
        console.log(expenseDetailsToUpdate);
        if (expenseDetailsToUpdate) {
            setExpenseDetailsId(expenseDetailsToUpdate.expenseDetailsId);
            setNotes(expenseDetailsToUpdate.notes);
            setIsApproved(expenseDetailsToUpdate.isApproved);
            setApprovalDate(expenseDetailsToUpdate.approvalDate);
        }
    }, [expenseDetailsToUpdate]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        const expenseDetails = {
            expenseDetailsId: parseInt(expenseDetailsId),
            notes,
            isApproved,
            approvalDate,
        };

        const response = expenseDetailsToUpdate
            ? await fetch(`/api/expense-details/update/${expenseDetailsId}`, {
                  method: 'PUT',
                  headers: { 'Content-Type': 'application/json' },
                  body: JSON.stringify(expenseDetails),
              })
            : await fetch('/api/expense-details/create', {
                  method: 'POST',
                  headers: { 'Content-Type': 'application/json' },
                  body: JSON.stringify(expenseDetails),
              });

        if (response.ok) {
            if (expenseDetailsToUpdate) {
                onExpenseDetailUpdated();
            } else {
                onExpenseDetailCreated();
            }
            resetForm();
        } else if (response.status === 404) {
            Swal.fire({
                icon: 'error',
                title: 'Expense not found',
                text: 'No expense found with this ID. Please check the ID and try again.',
            });
        } else {
            const errorData = await response.json();
            if (errorData.errors) {
                setErrors(errorData.errors);
            }
        }
    };

    const resetForm = () => {
        setExpenseDetailsId(0);
        setNotes('');
        setIsApproved(false);
        setApprovalDate('');
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
                <label htmlFor="expenseDetailsId" className="form-label">Expense Details ID</label>
                <input
                    type="number"
                    id="expenseDetailsId"
                    className="form-control"
                    value={expenseDetailsId}
                    onChange={(e) => setExpenseDetailsId(parseInt(e.target.value))}
                />
                {errors['ExpenseDetailsId'] && (
                    <div className="text-danger">{errors['ExpenseDetailsId'].join(', ')}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="notes" className="form-label">Notes</label>
                <textarea
                    id="notes"
                    className="form-control"
                    value={notes}
                    onChange={(e) => setNotes(e.target.value)}
                    minLength="5"
                    maxLength="500"
                />
                {errors['Notes'] && (
                    <div className="text-danger">{errors['Notes'].join(', ')}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="isApproved" className="form-label">Is Approved</label>
                <input
                    type="checkbox"
                    id="isApproved"
                    className="form-check-input"
                    checked={isApproved}
                    onChange={(e) => setIsApproved(e.target.checked)}
                />
                {errors['IsApproved'] && (
                    <div className="text-danger">{errors['IsApproved'].join(', ')}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="approvalDate" className="form-label">Approval Date</label>
                <input
                    type="text"
                    id="approvalDate"
                    className="form-control"
                    value={approvalDate}
                    onChange={(e) => setApprovalDate(e.target.value)}
                />
                {errors['ApprovalDate'] && (
                    <div className="text-danger">{errors['ApprovalDate'].join(', ')}</div>
                )}
            </div>
            <button type="submit" className="btn btn-primary">
                {expenseDetailsToUpdate ? 'Update Expense Details' : 'Create Expense Details'}
            </button>
            <button type="button" className="btn btn-secondary ms-2" onClick={resetForm}>
                Reset
            </button>
        </form>
    );
};

export default ExpenseDetailsForm;

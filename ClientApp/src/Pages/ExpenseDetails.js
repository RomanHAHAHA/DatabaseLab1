import React, { useEffect, useState } from 'react';
import ExpenseDetailsTable from '../Tables/ExpenseDetailsTable';
import ExpenseDetailsForm from '../Forms/ExpenseDetailsForm';
import TableGenerator from '../components/TableGenerator'; 

const ExpenseDetails = () => {
    const [expenseDetails, setExpenseDetails] = useState([]);
    const [expenseDetailToEdit, setExpenseDetailToEdit] = useState(null);
    const [dropdownOpen, setDropdownOpen] = useState(null);
    const [queryResult, setQueryResult] = useState([]); 

    const toggleDropdown = (expenseDetailsId) => {
        setDropdownOpen((prev) => (prev === expenseDetailsId ? null : expenseDetailsId));
    };

    const fetchAllExpenseDetails = async () => {
        try {
            const response = await fetch('/api/expense-details/get-all');
            if (!response.ok) throw new Error('Failed to fetch expense details');
            const data = await response.json();
            setExpenseDetails(data);
        } catch (error) {
            console.error(error);
        }
    };

    const deleteExpenseDetail = async (id) => {
        try {
            const response = await fetch(`/api/expense-details/delete/${id}`, { method: 'DELETE' });
            if (!response.ok) throw new Error('Failed to delete expense detail');
            fetchAllExpenseDetails();
        } catch (error) {
            console.error(error);
        }
    };

    const handleEditExpenseDetail = (expenseDetail) => {
        setExpenseDetailToEdit(expenseDetail);
    };

    const fetchApprovedExpensesLastMonth = async () => {
        try {
            const response = await fetch('/api/expense-details/approved-last-month');
            if (!response.ok) throw new Error('Failed to fetch approved expenses last month');
            const data = await response.json();
            setQueryResult(data); 
        } catch (error) {
            console.error(error);
        }
    };

    useEffect(() => {
        fetchAllExpenseDetails();
    }, []);

    return (
        <div className="container mt-4">
            <div className="row">
                <div className="col-md-4">
                    <ExpenseDetailsForm
                        expenseDetailsToUpdate={expenseDetailToEdit}
                        onExpenseDetailCreated={fetchAllExpenseDetails}
                        onExpenseDetailUpdated={fetchAllExpenseDetails}
                    />
                    <div className="mt-3">
                        <button
                            className="btn btn-primary btn-block"
                            onClick={fetchApprovedExpensesLastMonth}
                        >
                            Отримати деталі витрат, що були одобрені за останній місяць
                        </button>
                    </div>
                </div>
                <div className="col-md-8">
                    <h4>Expense Details List</h4>
                    <ExpenseDetailsTable
                        expenseDetails={expenseDetails}
                        handleEditExpenseDetails={handleEditExpenseDetail}
                        deleteExpenseDetails={deleteExpenseDetail}
                        toggleDropdown={toggleDropdown}
                        dropdownOpen={dropdownOpen}
                    />
                    <h4 className="mt-4">Query Results</h4>
                    <TableGenerator data={queryResult} />
                </div>
            </div>
        </div>
    );
};

export default ExpenseDetails;

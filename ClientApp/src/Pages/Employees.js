import React, { useEffect, useState } from 'react';
import EmployeeTable from '../Tables/EmployeeTable';
import EmployeeForm from '../Forms/EmployeeForm';
import TableGenerator from '../components/TableGenerator'; 

const Employees = () => {
    const [employees, setEmployees] = useState([]);
    const [employeeToEdit, setEmployeeToEdit] = useState(null);
    const [dropdownOpen, setDropdownOpen] = useState(null);
    const [queryResult, setQueryResult] = useState([]); 

    const toggleDropdown = (employeeId) => {
        setDropdownOpen((prev) => (prev === employeeId ? null : employeeId));
    };

    const fetchAllEmployees = async () => {
        try {
            const response = await fetch('/api/employees/get-all');
            if (!response.ok) throw new Error('Failed to fetch employees');
            const data = await response.json();
            setEmployees(data);
        } catch (error) {
            console.error(error);
        }
    };

    const deleteEmployee = async (id) => {
        try {
            const response = await fetch(`/api/employees/delete/${id}`, { method: 'DELETE' });
            if (!response.ok) throw new Error('Failed to delete employee');
            fetchAllEmployees();
        } catch (error) {
            console.error(error);
        }
    };

    const handleEditEmployee = (employee) => {
        setEmployeeToEdit(employee);
    };

    const fetchAverageExpensePerEmployee = async () => {
        try {
            const response = await fetch('/api/employees/average-expense');
            if (!response.ok) throw new Error('Failed to fetch average expense per employee');
            const data = await response.json();
            setQueryResult(data); 
        } catch (error) {
            console.error(error);
        }
    };

    const fetchDepartmentsEmployeeCountAboveAverage = async () => {
        try {
            const response = await fetch('/api/employees/count-in-departments');
            if (!response.ok) throw new Error('Failed to fetch departments with employee count above average');
            const data = await response.json();
            setQueryResult(data); 
        } catch (error) {
            console.error(error);
        }
    };

    useEffect(() => {
        fetchAllEmployees();
    }, []);

    return (
        <div className="container mt-4">
            <div className="row">
                <div className="col-md-4">
                    <EmployeeForm
                        employeeToUpdate={employeeToEdit}
                        onEmployeeCreated={fetchAllEmployees}
                        onEmployeeUpdated={fetchAllEmployees}
                    />
                    <div className="mt-3">
                        <button
                            className="btn btn-primary btn-block"
                            onClick={fetchAverageExpensePerEmployee}
                        >
                            Отримати назву відділу з середнім значенням витат на співробітника, сумою витат відділу та кількістю співробітників
                        </button>
                        <button
                            className="btn btn-secondary btn-block mt-2"
                            onClick={fetchDepartmentsEmployeeCountAboveAverage}
                        >
                            Отримати назви відділів, з кількістю співробітників, більшою за середню
                        </button>
                    </div>
                </div>
                <div className="col-md-8">
                    <h4>Employees List</h4>
                    <EmployeeTable
                        employees={employees}
                        handleEditEmployee={handleEditEmployee}
                        deleteEmployee={deleteEmployee}
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

export default Employees;

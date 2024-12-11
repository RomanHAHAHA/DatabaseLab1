import React, { useEffect, useState } from 'react';
import DepartmentTable from '../Tables/DepartmentTable';
import DepartmentForm from '../Forms/DepartmentForm';
import TableGenerator from '../components/TableGenerator';

const Departments = () => {
    const [departments, setDepartments] = useState([]); 
    const [data, setData] = useState([]); 
    const [departmentToEdit, setDepartmentToEdit] = useState(null);
    const [dropdownOpen, setDropdownOpen] = useState(null); 

    const toggleDropdown = (departmentId) => {
        setDropdownOpen((prev) => (prev === departmentId ? null : departmentId));
    };

    const fetchAllDepartments = async () => {
        try {
            const response = await fetch('/api/departments/get-all');
            if (!response.ok) throw new Error('Failed to fetch departments');
            const data = await response.json();
            setDepartments(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchData = async (endpoint) => {
        try {
            const response = await fetch(`/api/departments/${endpoint}`);
            if (!response.ok) throw new Error(`Failed to fetch data from ${endpoint}`);
            const result = await response.json();
            setData(result);
        } catch (error) {
            console.error(error);
        }
    };

    const deleteDepartment = async (id) => {
        try {
            const response = await fetch(`/api/departments/delete/${id}`, { method: 'DELETE' });
            if (!response.ok) throw new Error('Failed to delete department');
            fetchAllDepartments();
        } catch (error) {
            console.error(error);
        }
    };

    const handleEditDepartment = (department) => {
        setDepartmentToEdit(department);
    };

    useEffect(() => {
        fetchAllDepartments();
    }, []);

    return (
        <div className="container mt-4">
            <div className="row">
                <div className="col-md-4">
                    <DepartmentForm
                        departmentToUpdate={departmentToEdit}
                        onDepartmentCreated={fetchAllDepartments}
                        onDepartmentUpdated={fetchAllDepartments}
                    />
                </div>

                <div className="col-md-8">
                    <h4>Departments List</h4>
                    <DepartmentTable
                        departments={departments}
                        handleEditDepartment={handleEditDepartment}
                        deleteDepartment={deleteDepartment}
                        toggleDropdown={toggleDropdown}
                        dropdownOpen={dropdownOpen}
                    />
                </div>
            </div>

            <div className="row mt-4">
                <div className="col-md-4">
                    <h4>Fetch Data</h4>
                    <div className="d-flex flex-column">
                        <button
                            className="btn btn-primary mb-2"
                            onClick={() => fetchData('expense-amount')}
                        >
                            Отримати відділи з сумою їх витрат
                        </button>
                        <button
                            className="btn btn-primary mb-2"
                            onClick={() => fetchData('employee-count')}
                        >
                            Отримати кількість зареєстрованих (які внесені в БД) співробітників у кожному відділі
                        </button>
                        <button
                            className="btn btn-primary mb-2"
                            onClick={() => fetchData('all-expenses')}
                        >
                            Отримати кількість одобрених і неободрених витрат кожного відділу
                        </button>
                        <button
                            className="btn btn-primary mb-2"
                            onClick={() => fetchData('total-expenses-data')}
                        >
                            Отримати відділи з сумою їх витрат, кількістю одобрених витрат та максимальним лімітом
                        </button>
                        <button
                            className="btn btn-primary mb-2"
                            onClick={() => fetchData('expenses-above-threshold?threshold=699')}
                        >
                            Отримати відділи, сума витрат яких більше за поріг (699)
                        </button>
                        <button
                            className="btn btn-primary mb-2"
                            onClick={() => fetchData('above-average-employees')}
                        >
                            Отримати відділи, кількість всіх(незареєстрованих і зареєстрованих) співробітників яких більше середнього
                        </button>
                    </div>
                </div>
                <div className="col-md-8">
                    <h4>Query Results</h4>
                    <TableGenerator data={data} />
                </div>
            </div>
        </div>
    );
};

export default Departments;

import React, { useEffect, useState } from 'react';
import DepartmentTable from '../Tables/DepartmentTable.js';
import DepartmentForm from '../Forms/DepartmentForm';

const Departments = () => {
    const [departments, setDepartments] = useState([]);
    const [departmentToEdit, setDepartmentToEdit] = useState(null);
    const [dropdownOpen, setDropdownOpen] = useState(null);
    
    const toggleDropdown = (actorId) => {
        setDropdownOpen(prev => (prev === actorId ? null : actorId));
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

    const fetchByEmployeeCount = async () => {
        try {
            const response = await fetch('/api/departments/by-employee-count');
            if (!response.ok) throw new Error('Failed to fetch departments by employee count');
            const data = await response.json();
            setDepartments(data);
        } catch (error) {
            console.error(error);
        }
    };

    const fetchByNameStart = async () => {
        try {
            const response = await fetch('/api/departments/by-name-start');
            if (!response.ok) throw new Error('Failed to fetch departments by name start');
            const data = await response.json();
            setDepartments(data);
        } catch (error) {
            console.error(error);
        }
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
                    <div className="mb-3">
                        <button className="btn btn-primary me-2" onClick={fetchByEmployeeCount}>
                            Get Employee Count more 500 
                        </button>
                        <button className="btn btn-secondary" onClick={fetchByNameStart}>
                            Get Departments Name Start with 'o'
                        </button>
                    </div>
                    <DepartmentTable
                        departments={departments}
                        handleEditDepartment={handleEditDepartment}
                        deleteDepartment={deleteDepartment}
                        toggleDropdown={toggleDropdown}
                        dropdownOpen={dropdownOpen}
                    />
                </div>
            </div>
        </div>
    );
};

export default Departments;

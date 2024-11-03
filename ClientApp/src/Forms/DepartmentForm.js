import React, { useState, useEffect } from 'react';

const DepartmentForm = ({ onDepartmentCreated, departmentToUpdate, onDepartmentUpdated }) => {
    const [id, setId] = useState(null);
    const [name, setName] = useState('');
    const [employeeCount, setEmployeeCount] = useState(0);
    const [errors, setErrors] = useState({});

    useEffect(() => {
        if (departmentToUpdate) {
            setId(departmentToUpdate.departmentId);
            setName(departmentToUpdate.name);
            setEmployeeCount(departmentToUpdate.employeeCount);
        }
    }, [departmentToUpdate]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        const department = {
            name: name,
            employeeCount: parseInt(employeeCount),
        };

        const response = id 
            ? await fetch(`/api/departments/update/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(department),
            })
            : await fetch('/api/departments/create', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(department),
            });

        if (response.ok) {
            if (id) {
                onDepartmentUpdated();
            } else {
                onDepartmentCreated();
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
        setEmployeeCount(0);
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
                <label htmlFor="name" className="form-label">Department Name</label>
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
                <label htmlFor="employeeCount" className="form-label">Employee Count</label>
                <input
                    type="number"
                    id="employeeCount"
                    className="form-control"
                    value={employeeCount}
                    onChange={(e) => setEmployeeCount(e.target.value)}
                />
                {errors['EmployeeCount'] && (
                    <div className="text-danger">{errors['EmployeeCount'].join(', ')}</div>
                )}
            </div>
            <button type="submit" className="btn btn-primary">
                {id ? 'Update Department' : 'Create Department'}
            </button>
            <button type="button" className="btn btn-secondary ms-2" onClick={resetForm}>
                Reset
            </button>
        </form>
    );
};

export default DepartmentForm;

import React, { useState, useEffect } from 'react';
import Swal from 'sweetalert2';

const EmployeeForm = ({ onEmployeeCreated, employeeToUpdate, onEmployeeUpdated }) => {
    const [id, setId] = useState(null);
    const [fullName, setFullName] = useState('');
    const [position, setPosition] = useState('');
    const [departmentId, setDepartmentId] = useState(0);
    const [errors, setErrors] = useState({});

    useEffect(() => {
        if (employeeToUpdate) {
            setId(employeeToUpdate.employeeId);
            setFullName(employeeToUpdate.fullName);
            setPosition(employeeToUpdate.position);
            setDepartmentId(employeeToUpdate.departmentId); 
        }
    }, [employeeToUpdate]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        const employee = {
            fullName,
            position,
            departmentId, 
        };

        const response = id 
            ? await fetch(`/api/employees/update/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(employee),
            })
            : await fetch('/api/employees/create', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(employee),
            });

        if (response.ok) {
            if (id) {
                onEmployeeUpdated();
            } else {
                onEmployeeCreated();
            }
            resetForm();
        } else if (response.status === 404) {
            Swal.fire({
                icon: 'error',
                title: 'Departments not found',
                text: 'No department found with this ID. Please check the ID and try again.',
            });
        } else {
            const errorData = await response.json();
            if (errorData.errors) {
                setErrors(errorData.errors);
            }
        }
    };

    const resetForm = () => {
        setId(null);
        setFullName('');
        setPosition('');
        setDepartmentId('');
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
                <label htmlFor="fullName" className="form-label">Full Name</label>
                <input
                    type="text"
                    id="fullName"
                    className="form-control"
                    value={fullName}
                    onChange={(e) => setFullName(e.target.value)}
                />
                {errors['FullName'] && (
                    <div className="text-danger">{errors['FullName'].join(', ')}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="position" className="form-label">Position</label>
                <input
                    type="text"
                    id="position"
                    className="form-control"
                    value={position}
                    onChange={(e) => setPosition(e.target.value)}
                />
                {errors['Position'] && (
                    <div className="text-danger">{errors['Position'].join(', ')}</div>
                )}
            </div>
            <div className="mb-3">
                <label htmlFor="departmentId" className="form-label">Department ID</label>
                <input
                    type="number"
                    id="departmentId"
                    className="form-control"
                    value={departmentId}
                    onChange={(e) => setDepartmentId(e.target.value)}
                />
                {errors['DepartmentId'] && (
                    <div className="text-danger">{errors['DepartmentId'].join(', ')}</div>
                )}
            </div>
            <button type="submit" className="btn btn-primary">
                {id ? 'Update Employee' : 'Create Employee'}
            </button>
            <button type="button" className="btn btn-secondary ms-2" onClick={resetForm}>
                Reset
            </button>
        </form>
    );
};

export default EmployeeForm;

import React from 'react';
import { Table } from 'reactstrap';

class TableGenerator extends React.Component {
    render() {
        const { data } = this.props;

        if (!Array.isArray(data) || data.length === 0) {
            return <p>No data available.</p>;
        }

        const headers = Object.keys(data[0]);

        return (
            <Table striped bordered>
                <thead>
                    <tr>
                        {headers.map((header, index) => (
                            <th key={index}>{header}</th>
                        ))}
                    </tr>
                </thead>
                <tbody>
                    {data.map((row, rowIndex) => (
                        <tr key={rowIndex}>
                            {headers.map((header, index) => (
                                <td key={index}>
                                    {typeof row[header] === 'boolean'
                                        ? row[header] ? 'Yes' : 'No'
                                        : row[header] === '' 
                                        ? 'NULL' 
                                        : row[header]}
                                </td>
                            ))}
                        </tr>
                    ))}
                </tbody>
            </Table>
        );
    }
}

export default TableGenerator;

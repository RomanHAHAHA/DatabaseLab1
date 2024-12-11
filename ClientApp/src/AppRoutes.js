import Departments from "./Pages/Departments";
import ExpenseDetails from "./Pages/ExpenseDetails";
import Expenses from "./Pages/Expenses";
import ExpenseTypes from "./Pages/ExpenseTypes";
import Employees from "./Pages/Employees"

const AppRoutes = [
  {
    index: true,
    element: <Departments />
  },
  {
    path: '/expenses',
    element: <Expenses />
  },
  {
    path: '/expense-types',
    element: <ExpenseTypes />
  },
  {
    path: '/expense-details',
    element: <ExpenseDetails />
  },
  {
    path: '/employees',
    element: <Employees />
  },
];

export default AppRoutes;

import Departments from "./Pages/Departments";
import Expenses from "./Pages/Expenses";
import ExpenseTypes from "./Pages/ExpenseTypes";

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
];

export default AppRoutes;

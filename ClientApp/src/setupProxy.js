const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:1993';

const context =  [
  "/api/departments/get-all",
  "/api/departments/create",
  "/api/departments/delete",
  "/api/departments/update",
  "/api/departments/expense-amount",
  "/api/departments/employee-count",
  "/api/departments/all-expenses",
  "/api/departments/total-expenses-data",
  "/api/departments/expenses-above-threshold",
  "/api/departments/above-average-employees",
 
  "/api/expenses/update",
  "/api/expenses/delete",
  "/api/expenses/create",
  "/api/expenses/get-all",
  "/api/expenses/exceeding",
  "/api/expenses/above-avg",

  "/api/expense-types/update",
  "/api/expense-types/delete",
  "/api/expense-types/create",
  "/api/expense-types/get-all",
  "/api/expense-types/average-limit-per-type",
  "/api/expense-types/max-approved-per-type",
  "/api/expense-types/unused-by-department",

  "/api/employees/update",
  "/api/employees/delete",
  "/api/employees/create",
  "/api/employees/get-all",
  "/api/employees/average-expense",
  "/api/employees/count-in-departments",

  "/api/expense-details/update",
  "/api/expense-details/delete",
  "/api/expense-details/create",
  "/api/expense-details/get-all",
  "/api/expense-details/approved-last-month",

  "/api/reports/generate-report"
];

module.exports = function(app) {
  const appProxy = createProxyMiddleware(context, {
    proxyTimeout: 10000,
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  });

  app.use(appProxy);
};

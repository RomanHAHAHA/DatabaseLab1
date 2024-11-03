const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:1993';

const context =  [
  "/api/departments/get-all",
  "/api/departments/create",
  "/api/departments/delete",
  "/api/departments/update",
  "/api/departments/by-employee-count",
  "/api/departments/by-name-start",
 
  "/api/expenses/update",
  "/api/expenses/delete",
  "/api/expenses/create",
  "/api/expenses/get-all",
  "/api/expenses/get-by-department-id",
  "/api/expenses/get-by-expense-type-id",
  "/api/expenses/get-by-amount",
  "/api/expenses/get-by-date",
  "/api/expenses/get-by-code-length",

  "/api/expense-types/update",
  "/api/expense-types/delete",
  "/api/expense-types/create",
  "/api/expense-types/get-all",
  "/api/expense-types/by-limit-amount",
  "/api/expense-types/by-description-length",
  "/api/expense-types/by-name-start",
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

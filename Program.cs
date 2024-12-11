using DatabaseLab1.API.Middlewares;
using DatabaseLab1.API.Options;
using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.DB.Repositories.Default;
using DatabaseLab1.DB.Repositories.Logging;
using DatabaseLab1.Services.Implementations;
using DatabaseLab1.Services.Interfaces;
using DinkToPdf.Contracts;
using DinkToPdf;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.Decorate<IDepartmentRepository, LoggingDepartmentRepository>();

builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.Decorate<IExpenseRepository, LoggingExpenseRepository>();

builder.Services.AddScoped<IExpenseTypeRepository, ExpenseTypeRepository>();
builder.Services.Decorate<IExpenseTypeRepository, LoggingExpenseTypesRepository>();

builder.Services.AddScoped<IExpenseDetailsRepository, ExpenseDetailsRepository>();
builder.Services.Decorate<IExpenseDetailsRepository, LoggingExpenseDetailsRepository>();

builder.Services.AddScoped<IEmployeeRepository, EmployeesRepository>();
builder.Services.Decorate<IEmployeeRepository, LoggingEmployeeRepository>();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IReportService, ReportService>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddSingleton<IPdfGenerator, PdfGenerator>();
builder.Services.AddSingleton<IConverter, SynchronizedConverter>(sp =>
{
    return new SynchronizedConverter(new PdfTools());
});

builder.Services
    .Configure<DbOptions>(builder.Configuration
    .GetSection(nameof(DbOptions)));

var app = builder.Build();

app.UseMiddleware<ValidationLoggingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;

app.Run();

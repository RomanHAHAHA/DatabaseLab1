using DatabaseLab1.API.Middlewares;
using DatabaseLab1.API.Options;
using DatabaseLab1.DB.Interfaces;
using DatabaseLab1.DB.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); 
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>(); 
builder.Services.AddScoped<IExpenseTypeRepository, ExpenseTypeRepository>();

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

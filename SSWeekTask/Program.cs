using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Serilog;

using SSWeekTask.Data;
using SSWeekTask.Data.Repositories;
using SSWeekTask.Data.Repositories.Interfaces;
using SSWeekTask.Services;
using SSWeekTask.Services.Interfaces;
using SSWeekTask.Services.Mappers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDataMapper, DataMapper>();

var connectionString = builder.Configuration["ConnectionStrings:DbConnStr"];
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptions =>
    {
        mySqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);
    });
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<ITeacherService, TeacherService>();
builder.Services.AddTransient<ICourseService, CourseService>();

builder.Services.AddTransient<IStudentRepository, StudentRepository>();
builder.Services.AddTransient<ITeacherRepository, TeacherRepository>();
builder.Services.AddTransient<ICourseRepository, CourseRepository>();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Week task api", Version = "v1" });
});

Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Application has started.");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start.");
}

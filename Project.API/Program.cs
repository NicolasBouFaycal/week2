using UMS.Infrastructure.EmailService;
using Microsoft.EntityFrameworkCore;
using UMS.Domain;
using UMS.Infrastructure.EmailServiceAbstraction;
using UMS.Persistence;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.OData;
using UMS.Application.Abstraction;
using UMS.Application.Service;
using MediatR;
using UMS.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using UMS.Application.Handlers;
using UMS.Application.Commands;
using UMS.Common.CustomHandler;
using UMS.Common.Aauthorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddOData(options =>
options.Select().Filter().OrderBy());



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(connectionString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddAuthentication("CookieAuthentication")
    .AddCookie("CookieAuthentication", config =>
    {
        config.Cookie.Name = "UserLoginCookie";
        config.LoginPath = "";
        config.AccessDeniedPath = "";
    }) ;
builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("UserPolicy", policyBuilder =>
    {
        policyBuilder.UserRequireCustomClaim(ClaimTypes.Email);
    });
});
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddTransient<IRequestHandler<CreateCourseCommand, ActionResult<Course>>, CreateCourseHandler>();
builder.Services.AddTransient<IRequestHandler<GetAllClassesForStudent, ActionResult<List<TeacherPerCourse>>>, GetAllCoursesForStudentHandler>();
builder.Services.AddTransient<IRequestHandler<StudentEnrollToCourseCommand, ActionResult<ClassEnrollment>>, StudentEnrollToCourseHandler>();
builder.Services.AddTransient<IRequestHandler<InserSessionTimeCommand, ActionResult<SessionTime>>, InserSessionTimeHandler>();
builder.Services.AddTransient<IRequestHandler<GetAllCourses, ActionResult<List<Course>>>, GetAllCoursesHandler>();
builder.Services.AddTransient<IRequestHandler<GetAllTeacherPerCourse, ActionResult<List<TeacherPerCourse>>>, GetAllTeacherPerCourseHandler>();
builder.Services.AddTransient<IRequestHandler<GetAllSessionTime, ActionResult<List<SessionTime>>>, GetAllSessionTimeHandler>();
builder.Services.AddTransient<IRequestHandler<AssignTeacherToCourseCommand, ActionResult<TeacherPerCourse>>, AssignTeacherToCourseHandler>();
builder.Services.AddTransient<IRequestHandler<AssignTeacherPerCoursePerSessionTimeCommand, ActionResult<TeacherPerCoursePerSessionTime>>, AssignTeacherPerCoursePerSessionTimeHandler>();
builder.Services.AddTransient<IRequestHandler<LoginCommand, Task<ActionResult<string>>>, LoginHandler>();

builder.Services.AddScoped<IAuthorizationHandler, PoliciesAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
builder.Services.AddTransient<IStudentsHelper, StudentsHelper>();
builder.Services.AddTransient<ITeachersHelper, TeachersHelper>();
builder.Services.AddTransient<IAdminHelper, AdminsHelper>();
builder.Services.AddTransient<IFirebaseHelper, FirebasesHelper>();


builder.Services.AddControllersWithViews();

builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(2);
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.UseAuthentication();



/*app.UseMvc(routeBuilder =>
{
    routeBuilder.EnableDependencyInjection();
    routeBuilder.Select().OrderBy().Filter();
});*/
app.UseRouting();
app.UseAuthorization();




app.MapControllers();

app.Run();

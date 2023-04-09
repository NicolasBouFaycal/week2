using UMS.Infrastructure.EmailService;
using Microsoft.EntityFrameworkCore;
using UMS.Infrastructure.EmailServiceAbstraction;
using UMS.Persistence;
using Microsoft.AspNetCore.OData;
using UMS.Application.Abstraction;
using UMS.Application.Service;
using MediatR;
using UMS.Application.Aauthorization;
using UMS.Common.Abstraction;
using UMS.API.uploadImg;
using UMS.API.Middleware;
using System.Reflection;
using UMS.Application.Commands;
using Microsoft.AspNetCore.Mvc;
using UMS.Domain;
using UMS.Application.Queries;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Intersoft.Crosslight;
using Scrutor;
using UMS.API.Handler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
/*builder.Services.Scan(scan =>
    scan.FromAssembliesOf(typeof(IRequestHandler<,>))
        .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
        .AsImplementedInterfaces()
        .WithTransientLifetim
*/
// Add services to the container.

/*builder.Services.AddControllers().AddOData(options =>
options.Select().Filter().OrderBy());*/

builder.Services.AddControllers(options =>
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.RequireHttpsAttribute()))
    .AddOData(options =>
        options.Select().Filter().OrderBy())
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(connectionString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DDD Architecture", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer",

    });
    c.AddSecurityDefinition("RefreshToken", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert refresh token",
        Name = "Refresh",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "RefreshToken",

    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        },
          {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="RefreshToken"
                }
            },
            new string[]{}
        }
    });

});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = "https://securetoken.google.com/dddprojet-5943b";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "https://securetoken.google.com/dddprojet-5943b",
        ValidateAudience = true,
        ValidAudience = "dddprojet-5943b",
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("AIzaSyC8-YnOCMAZqK4DAuTIltT-nkkbWziTTuI"))
    };

});

builder.Services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUploadImgHelper, InsertProfilePicHelper>();


/*builder.Services.AddAuthentication("CookieAuthentication")
    .AddCookie("CookieAuthentication", config =>
    {
        config.Cookie.Name = "UserLoginCookie";
    }) ;*/

/*builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("UserPolicy", policyBuilder =>
    {
        policyBuilder.UserRequireCustomClaim(ClaimTypes.Email);
    });
});*/


/*builder.Services.AddTransient<IRequestHandler<CoursesCommand, ActionResult<Course>>, CreateCourseHandler>();
builder.Services.AddTransient<IRequestHandler<AllClassesForStudent, List<TeacherPerCourse>>, AllCoursesForStudentHandler>();
builder.Services.AddTransient<IRequestHandler<StudentEnrollToCoursesCommand, string>, StudentEnrollToCoursesHandler>();
builder.Services.AddTransient<IRequestHandler<InserSessionTimeCommand, ActionResult<SessionTime>>, InserSessionTimeHandler>();
builder.Services.AddTransient<IRequestHandler<AllCourses, ActionResult<List<Course>>>, AllCoursesHandler>();
builder.Services.AddTransient<IRequestHandler<AllTeacherPerCourse, ActionResult<List<TeacherPerCourse>>>, AllTeacherPerCourseHandler>();
builder.Services.AddTransient<IRequestHandler<AllSessionTime, ActionResult<List<SessionTime>>>, AllSessionTimeHandler>();
builder.Services.AddTransient<IRequestHandler<AssignTeacherToCourseCommand, ActionResult<TeacherPerCourse>>, AssignTeacherToCourseHandler>();
builder.Services.AddTransient<IRequestHandler<AssignTeacherPerCoursePerSessionTimeCommand, ActionResult<TeacherPerCoursePerSessionTime>>, AssignTeacherPerCoursePerSessionTimeHandler>();
builder.Services.AddTransient<IRequestHandler<LoginCommand, Task<ActionResult<string>>>, LoginHandler>();*/

builder.Services.AddHandlersWithTransientLifetime(
    typeof(CreateCourseHandler),
    typeof(AllCoursesForStudentHandler),
    typeof(StudentEnrollToCoursesHandler),
    typeof(InserSessionTimeHandler),
    typeof(AllCoursesHandler),
    typeof(AllTeacherPerCourseHandler),
    typeof(AllSessionTimeHandler),
    typeof(AssignTeacherToCourseHandler),
    typeof(AssignTeacherPerCoursePerSessionTimeHandler),
    typeof(LoginHandler)
);



//builder.Services.AddScoped<IAuthorizationHandler, PoliciesAuthorizationHandler>();
builder.Services.AddTransient<IStudentsHelper, StudentsService>();
builder.Services.AddTransient<ITeachersHelper, TeachersService>();
builder.Services.AddTransient<IAdminsHelper, AdminsService>();
builder.Services.AddTransient<IAuthenticationHelper, AuthenticationService>();


builder.Services.AddControllersWithViews();

/*builder.Services.ConfigureApplicationCookie(options => {
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(2);
    options.SlidingExpiration = true;
});*/

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MY API");
    });

}
app.MapControllers();
app.UseHttpsRedirection();

app.UseAuthentication();



/*app.UseMvc(routeBuilder =>
{
    routeBuilder.EnableDependencyInjection();
    routeBuilder.Select().OrderBy().Filter();
});*/
app.UseMiddleware<TokenExpirationMiddleware>();

app.UseRouting();
app.UseAuthorization();




app.MapControllers();

app.Run();

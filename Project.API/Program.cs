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
using UMS.Application.Queries;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Serialization;
using UMS.Common.Shema;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Exceptions;
using UMS.Application.Handler;

;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

builder.Services.AddControllers(options =>
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.RequireHttpsAttribute()))
    .AddOData(options =>
        options.Select().Filter().OrderBy())
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(connectionString));

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
var handlerTypes = new Type[]
{
    typeof(CreateCourseHandler),
    typeof(AllCoursesForStudentHandler),
    typeof(StudentEnrollToCoursesHandler),
    typeof(SessionTimeHandler),
    typeof(AllCoursesHandler),
    typeof(AllTeacherPerCourseHandler),
    typeof(AllSessionTimeHandler),
    typeof(TeacherToCourseHandler),
    typeof(TeacherPerCoursePerSessionTimeHandler),
    typeof(LoginHandler),
    typeof(AllClassEnrollmentsHandler),
    typeof(AttendanceHandler),
    typeof(AllStudentAttendanceHandler),
    typeof(AttendanceTrackingHandler)

};
builder.Services.AddMyServices(handlerTypes);



builder.Services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUploadImgHelper, InsertProfilePicHelper>();
builder.Services.AddScoped<IShemaHelper, ShemaService>();
builder.Services.AddTransient<IStudentsHelper, StudentsService>();
builder.Services.AddTransient<ITeachersHelper, TeachersService>();
builder.Services.AddTransient<IAdminsHelper, AdminsService>();
builder.Services.AddTransient<IAuthenticationHelper, AuthenticationService>();

//ConfigureLogging();
builder.Host.UseSerilog();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllersWithViews();
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
app.UseMiddleware<TokenExpirationMiddleware>();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();

/*void ConfigureLogging()
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile(
            $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
            optional: true)
        .Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
        .Enrich.WithProperty("Environment", environment)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
{
    return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
    };
}*/
/*builder.Host.UseSerilog((context, configuration) =>
{
    configuration.Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(
        new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]))
        {
            IndexFormat=
        });
});*/
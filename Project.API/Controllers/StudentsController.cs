using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Npgsql;
using System.Text;
using UMS.Application;
using UMS.Application.Commands;
using UMS.Application.DTO;
using UMS.Application.Queries;
using UMS.Common.Abstraction;
using UMS.Domain.LinqModels;
using UMS.Infrastructure.EmailServiceAbstraction;
using UMS.Persistence;

namespace UMS.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentsController : ControllerBase
    {
       
        private readonly IEmailService _emailService;
        private readonly IMediator _mediator;
        private readonly IUploadImgHelper _uploadImgHelper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<StudentsController> _logger;
        private readonly IMapper _mapper ;
        private readonly MyDbContext _context;
        public readonly IShemaHelper _shemaService;





        public StudentsController(IShemaHelper shemaService,MyDbContext context,IMapper mapper,ILogger<StudentsController> logger, IConfiguration configuration,IUploadImgHelper uploadImgHelper, IEmailService emailService,  IMediator mediator)
        {
            
            _emailService = emailService;
            _mediator = mediator;   
            _uploadImgHelper = uploadImgHelper;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _context = context;
            _shemaService = shemaService;
        }
        [HttpGet("AllClassesForStudent")]
        [EnableQuery]
        public async Task<ActionResult<List<TeacherPerCourseDTO>>> AllClassesForStudent()
        {
            _logger.LogInformation("Get All Courses execute");
            HttpClient client = new HttpClient();
            string url =String.Format("https://localhost:7273/Student-Service/AllClassesForStudent?userId={0}", Uid.uid);
            HttpResponseMessage response =  client.GetAsync(url).Result;  
            if(response.IsSuccessStatusCode)
            {
                
                return await response.Content.ReadFromJsonAsync<List<TeacherPerCourseDTO>>();
            }

            throw new Exception("Internal Service Error");
        }
        [HttpGet("AllStudentAttendance")]
        [EnableQuery]
        public async Task<ActionResult<List<StudentAttendance>>> AllStudentAttendance()
        {
            HttpClient client = new HttpClient();
            string url = String.Format("https://localhost:7273/Student-Service/AllStudentAttendance?userId={0}", Uid.uid);
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {

                return await response.Content.ReadFromJsonAsync<List<StudentAttendance>>();
            }

            throw new Exception("Internal Service Error");
        }

        [HttpPost("StudentEnrollToCourses")]
        public async Task<ActionResult<string>> StudentEnrollToCourses([FromBody] StudentToCourseDTO Id)
        {
            //var userid = Request.Headers["Authentication"];
            string json = JsonConvert.SerializeObject(Id);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            string url = String.Format("https://localhost:7273/Student-Service/StudentEnrollToCourses?userId={0}", Uid.uid);
            HttpResponseMessage response = await client.PostAsync(url, content);



            if (response.IsSuccessStatusCode)
            {

                return new JsonResult(response.Content.ReadAsStringAsync().Result);
            }

            throw new Exception("You can not be enrolled in this class because you are enrolled in");
        }

        [HttpPost("UploadImage")]
        public async Task<ActionResult<string>> UploadImageAsync([FromForm] UploadImg obj)
        {

            HttpClient client = new HttpClient();
            string url = String.Format("https://localhost:7273/Student-Service/UploadImage?userId={0}", Uid.uid);

            var form = new MultipartFormDataContent();

            form.Add(new StreamContent(obj.file.OpenReadStream()), "file", obj.file.FileName);

            HttpResponseMessage response = await client.PostAsync(url, form);



            if (response.IsSuccessStatusCode)
            {

                return new JsonResult(response.Content.ReadAsStringAsync().Result);
            }

            throw new Exception("You can not be enrolled in this class because you are enrolled in");

        }
    }
}


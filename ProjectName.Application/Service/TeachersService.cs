using UMS.Application.Abstraction;
using UMS.Domain;
using UMS.Persistence;
using UMS.Domain.Models;
using UMS.Common.Abstraction;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using UMS.Domain.LinqModels;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace UMS.Application.Service
{
    public class TeachersService : ITeachersHelper
    {
        public readonly MyDbContext _context;
        public readonly IShemaHelper _shemaService;
        public readonly IConnection _connect;
        public TeachersService(IConnection connection, MyDbContext context, IShemaHelper shemaService)
        {
            _context = context;
            _shemaService = shemaService;
            _connect = connection;
        }

        public TeacherPerCoursePerSessionTime TeacherPerCoursePerSessionTime(int teacherPerCourseId, int sessionTimeId)
        {
            try
            {
                var userid = Uid.uid;
                var branch = _shemaService.getBranch(userid);
                var conn = _context.Database.GetDbConnection() as NpgsqlConnection;
                _shemaService.setShema(conn, branch);
                TeacherPerCoursePerSessionTime SessionCourse = new TeacherPerCoursePerSessionTime();
                SessionCourse.Id = 2;
                SessionCourse.TeacherPerCourseId = teacherPerCourseId;
                SessionCourse.SessionTimeId = sessionTimeId;

                if (SessionCourse == null)
                    throw new InvalidOperationException("Insert Data");
                _context.Add(SessionCourse);
                _context.SaveChanges();
                conn.Close();
                return SessionCourse;
            }
            catch (Exception ex)
            {
                throw new Exception("connection not find");

            }
        }

        public TeacherPerCourse TeacherToCourse(int courseId)
        {
            try
            {
                var userid = Uid.uid;
                var branch = _shemaService.getBranch(userid);
                var conn = _context.Database.GetDbConnection() as NpgsqlConnection;
                _shemaService.setShema(conn, branch);
                TeacherPerCourse course = new TeacherPerCourse();
                course.Id = 3;
                course.CourseId = courseId;
                course.TeacherId = (from s in _context.Users where (s.KeycloakId == userid) select s.Id).FirstOrDefault();

                if (course == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(course);
                _context.SaveChanges();
                conn.Close();
                return course;
            }
            catch (Exception ex)
            {
                throw new Exception("connection not fond");

            }
        }

        public List<Course> AllCourses()
        {
            /*var courses = _context.Courses
            .Where(c => !_context.TeacherPerCourses
                .Any(tpc => tpc.CourseId == c.Id))
            .ToList();*/

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            var userid = Uid.uid;
            var branch = _shemaService.getBranch(userid);
            var conn = _context.Database.GetDbConnection() as NpgsqlConnection;
            _shemaService.setShema(conn, branch);
            var courses2 = _context.Courses
                .Where(course => !course.TeacherPerCourses.Any())
                .ToList();

            if (courses2.Count > 0)
            {
                conn.Close();
                return courses2;
            }
            throw new Exception("NUll");
        }

        public List<SessionTime> AllSessionTime()
        {
            var userid = Uid.uid;
            var branch = _shemaService.getBranch(userid);
            var conn = _context.Database.GetDbConnection() as NpgsqlConnection;
            _shemaService.setShema(conn, branch);
            var getTeacherId = (from t in _context.Users where t.KeycloakId == userid select t.Id).FirstOrDefault();

            var Scourses = _context.SessionTimes
                .Where(st => !st.TeacherPerCoursePerSessionTimes.Any())
                .ToList();


            if (Scourses.Count > 0)
            {
                conn.Close();
                return Scourses;
            }
            throw new Exception("NUll");

        }

        public List<TeacherPerCourse> AllTeacherPerCourse()
        {
            var userid = Uid.uid;
            var branch = _shemaService.getBranch(userid);
            var conn = _context.Database.GetDbConnection() as NpgsqlConnection;
            _shemaService.setShema(conn, branch);
            var getTeacherId = (from t in _context.Users where t.KeycloakId == userid select t.Id).FirstOrDefault();

            var thcourses = _context.TeacherPerCourses
                .Where(tp => tp.TeacherId == getTeacherId && !tp.TeacherPerCoursePerSessionTimes.Any())
                .ToList();

            if (thcourses.Count > 0)
            {
                conn.Close();
                return thcourses;
            }
            throw new Exception("NUll");
        }

        public SessionTime SessionTime(DateTime StartTimeYYYY_MM_DD, DateTime EndTimeYYYY_MM_DD, int Duration)
        {
            try
            {
                var userid = Uid.uid;
                var branch = _shemaService.getBranch(userid);
                var conn = _context.Database.GetDbConnection() as NpgsqlConnection;
                _shemaService.setShema(conn, branch);
                SessionTime tm = new SessionTime();
                tm.StartTime = StartTimeYYYY_MM_DD;
                tm.EndTime = EndTimeYYYY_MM_DD;
                tm.Duration = Duration;
                tm.Id = 6;
                if (tm == null)
                    throw new InvalidOperationException("INsert Data");
                _context.Add(tm);
                _context.SaveChanges();
                conn.Close();
                return tm;
            }
            catch (Exception ex)
            {
                throw new Exception("connection not find");

            }
        }

        public List<ClassEnrollment> AllClassEnrollments()
        {
            var branch = _shemaService.getBranch(Uid.uid);
            var conn = _context.Database.GetDbConnection() as NpgsqlConnection;
            _shemaService.setShema(conn, branch);
            /*
            var factory = new ConnectionFactory { Uri = new Uri("amqp://guest:guest@localhost:5672") };
            using var connection = factory.CreateConnection();*/
            using var channel = _connect.CreateModel();
            var message = "";
            //demo-queue
            /* channel.QueueDeclare("StudentEnrollToCourses",
                 durable: true,
                 exclusive: false,
                 autoDelete: false,
                 arguments: null);
             var consumer = new EventingBasicConsumer(channel);
             consumer.Received += (sender, e) =>
             {
                 var body = e.Body.ToArray();
                 var message = Encoding.UTF8.GetString(body);
                 Console.WriteLine(message);
             };
             channel.BasicConsume("StudentEnrollToCourses", true,consumer);*/


            channel.ExchangeDeclare("StudentEnrollToCourses-exchange", ExchangeType.Direct);
            channel.QueueDeclare("StudentEnrollToCourses",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            //queue name,exchange Name,Routing Key Name 
            channel.QueueBind("StudentEnrollToCourses", "StudentEnrollToCourses-exchange", "StudentEnrollToCourses.init");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };
            channel.BasicConsume("StudentEnrollToCourses", true, consumer);
            if (message != "")
            {
                var cl = JsonConvert.DeserializeObject<ClassEnrollment>(message);
                ClassEnrollment classenrollment = new ClassEnrollment();
                classenrollment.Id = 4;
                classenrollment.StudentId = cl.StudentId;
                classenrollment.ClassId = cl.ClassId;
                _context.Add(classenrollment);
                _context.SaveChanges();

            }

            var getAllStudentsInTeacherClasses = (from c in _context.ClassEnrollments
                                                  join tc in _context.TeacherPerCourses
                                                  on c.ClassId equals tc.Id
                                                  join u in _context.Users
                                                  on tc.TeacherId equals u.Id
                                                  where u.KeycloakId == Uid.uid
                                                  select c).ToList();
            conn.Close();

            return getAllStudentsInTeacherClasses;

        }

        public Attendance Attendance(Attendance at)
        {
            var branch = _shemaService.getBranch(Uid.uid);
            var conn = _context.Database.GetDbConnection() as NpgsqlConnection;
            _shemaService.setShema(conn, branch);
            Attendance attendance = new Attendance();
            attendance.StudentCourseId = at.StudentCourseId;
            attendance.AttendanceStatus = at.AttendanceStatus;
            attendance.Date = at.Date;
            _context.Add(attendance);
            _context.SaveChanges();
            conn.Close();
            return attendance;

        }
    }
}

/* var Scourses = _context.SessionTimes
 .Where(st => !_context.TeacherPerCoursePerSessionTimes
     .Any(tpcpst => tpcpst.SessionTimeId == st.Id))
 .ToList();*/



/*var thcourses = _context.TeacherPerCourses
.Where(tpc => !_context.TeacherPerCoursePerSessionTimes
    .Any(tpcpst => tpcpst.TeacherPerCourseId == tpc.Id && tpc.TeacherId != getTeacherId))
.ToList();*/
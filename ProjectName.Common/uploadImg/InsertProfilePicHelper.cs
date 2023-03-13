using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using UMS.Common.Abstraction;
using UMS.Domain;
using UMS.Persistence;

namespace UMS.API.uploadImg
{
    public class InsertProfilePicHelper :IUploadImgHelper
    {
        public static IWebHostEnvironment? _environment;
        public readonly MyDbContext _context;
        public InsertProfilePicHelper()
        {

        }
        public InsertProfilePicHelper(IWebHostEnvironment environment, MyDbContext context)
        {

            _environment = environment;
            _context = context;

        }
        public ActionResult<string> UploadProfile(ControllerBase controllerBase, [FromForm] UploadImg obj)
        {
            string path;
            try
            {
                if (obj.file.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                    }
                    using (FileStream fileStream = System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + obj.file.FileName))
                    {
                        obj.file.CopyTo(fileStream);
                        fileStream.Flush();
                        path = "\\Upload\\" + obj.file.FileName;
                    }

                    var userid = controllerBase.Request.Cookies["UserId"];
                    var user = _context.Users.FirstOrDefault(r => r.KeycloakId == userid);
                    if (user != null)
                    {
                        user.ImagePath = path;
                        _context.SaveChanges();
                    }
                    return "\\Upload\\" + obj.file.FileName;
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

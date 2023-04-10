using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using UMS.Common.Abstraction;
using UMS.Domain;
using UMS.Domain.Models;
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
        public  string UploadProfileAsync(UploadImg obj)
        {
            /* string path;
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
             }*/
            // Check if a file was uploaded
            if (obj.file == null || obj.file.Length == 0)
            {
                throw new Exception("No file was uploaded.");
            }

            // Get the path to the wwwroot folder
            string wwwRootPath = _environment.WebRootPath;

            // Generate a unique file name
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.file.FileName);

            // Combine the wwwroot path and the file name to get the full path to save the file
            string filePath = Path.Combine(wwwRootPath, fileName);

            // Save the file to the wwwroot folder
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                 obj.file.CopyTo(stream);
            }
            string path = Path.Combine(_environment.WebRootPath, fileName);
            var userid = Uid.uid;
            var user = _context.Users.FirstOrDefault(r => r.KeycloakId == userid);
            if (user != null)
            {
                user.ImagePath = path;
                _context.SaveChanges();
            }
            return path;
        }
    }
}

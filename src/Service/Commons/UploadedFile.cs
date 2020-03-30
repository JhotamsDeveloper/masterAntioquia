using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Model;
using Model.DTOs;
using Persisten.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Service.Commons
{
    public interface IUploadedFile
    {
        string UploadedFileImage(string value, IFormFile file);
        string UploadedFileImage(IFormFile value);
        List<string> UploadedMultipleFileImage(IEnumerable<IFormFile> files);
        Boolean DeleteConfirmed(string imgModel);
    }

    public class UploadedFile : IUploadedFile
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;

        public UploadedFile(IHostingEnvironment hostingEnvironment,
                    ApplicationDbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        public string UploadedFileImage(IFormFile file)
        {

            string uniqueFileName = null;

            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images\\Places");
            uniqueFileName = "Place-" + Guid.NewGuid().ToString() + "." + Path.GetExtension(file.FileName).Substring(1);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }


        public string UploadedFileImage(string value, IFormFile file)
        {
            string uniqueFileName = null;

            if (value != null)
            {
                var _deleteFile = DeleteUpload(value);

                if (_deleteFile)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images\\Places");
                    uniqueFileName = "Place-" + Guid.NewGuid().ToString() + "." + Path.GetExtension(file.FileName).Substring(1);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                }
            }

            return uniqueFileName;
        }

        public List<string> UploadedMultipleFileImage(IEnumerable<IFormFile> files)
        {
            //Nombre de archivo único
            List<string> uniqueFileName = new List<string>();
            int _contador = 0;

            foreach (var file in files)
            {
                _contador++;
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images\\Places");
                uniqueFileName.Add("Place-" + Guid.NewGuid().ToString() + "." + Path.GetExtension(file.FileName).Substring(1));
                string filePath = Path.Combine(uploadsFolder, uniqueFileName[_contador-1]);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
        public Boolean DeleteConfirmed(string imgModel)
        {
            return DeleteUpload(imgModel);
        }

        private Boolean DeleteUpload(string imgModel)
        {

            imgModel = Path.Combine(_hostingEnvironment.WebRootPath, "images\\Places", imgModel);
            FileInfo fileInfo = new FileInfo(imgModel);

            if (fileInfo != null)
            {
                System.IO.File.Delete(imgModel);
                fileInfo.Delete();

                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

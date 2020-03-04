using Microsoft.AspNetCore.Hosting;
using Model.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Service.Commons
{
    public class UploadedFile
    {

        private readonly IHostingEnvironment _hostingEnvironment;


        public UploadedFile(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        public string UploadedFileCoverPage(PlaceCreateDto model)
        {
                string uniqueFileName = null;

                if (model.CoverPage != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images\\CoverPages");
                    uniqueFileName = "CoverPage_" + Guid.NewGuid().ToString() + "." + Path.GetExtension(model.CoverPage.FileName).Substring(1);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.CoverPage.CopyTo(fileStream);
                    }
                }
                return uniqueFileName;
            
        }

        public string UploadedFileLogo(PlaceCreateDto model)
        {
            string uniqueFileName = null;

            if (model.Logo != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images\\Logos");
                uniqueFileName = "Logo_" + model.Logo.Name + Guid.NewGuid().ToString() + "." + Path.GetExtension(model.Logo.FileName).Substring(1);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.CoverPage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

    }
}

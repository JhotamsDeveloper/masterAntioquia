﻿using Microsoft.AspNetCore.Hosting;
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
        List<string> UploadedMultipleFileImage(IEnumerable<IFormFile> files, List<string> value);
        bool UploadedMultipleFileImage(List<string> value);
        Boolean DeleteConfirmed(string imgModel);
    }

    public class UploadedFile : IUploadedFile
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public UploadedFile(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public string UploadedFileImage(IFormFile file)
        {

            string uniqueFileName;

            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images\\images");
            uniqueFileName = "images-" + Guid.NewGuid().ToString() + "." + Path.GetExtension(file.FileName).Substring(1);
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

            if (value != null && file != null)
            {
                var _deleteFile = DeleteUpload(value);
            }

            if (file != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images\\images");
                uniqueFileName = "images-" + Guid.NewGuid().ToString() + "." + Path.GetExtension(file.FileName).Substring(1);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);

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
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images\\images");
                uniqueFileName.Add("images-" + Guid.NewGuid().ToString() + "." + Path.GetExtension(file.FileName).Substring(1));
                string filePath = Path.Combine(uploadsFolder, uniqueFileName[_contador-1]);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        public List<string> UploadedMultipleFileImage(IEnumerable<IFormFile> files, List<string> value)
        {
            //Nombre de archivo único
            List<string> uniqueFileName = new List<string>();

            if (value.Count != 0)
            {
                DeleteUpload(value);
            }

            if (files != null)
            {

                int _contador = 0;

                foreach (var file in files)
                {
                    _contador++;
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images\\images");
                    uniqueFileName.Add("images-" + Guid.NewGuid().ToString() + "." + Path.GetExtension(file.FileName).Substring(1));
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName[_contador - 1]);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
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

            imgModel = Path.Combine(_hostingEnvironment.WebRootPath, "images\\images", imgModel);
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

        private Boolean DeleteUpload(List<string> imgModel)
        {
            for (int i = 0; i < imgModel.Count; i++)
            {
                imgModel[i] = Path.Combine(_hostingEnvironment.WebRootPath, $"images\\images", imgModel[i]);
                FileInfo fileInfo = new FileInfo(imgModel[i]);

                if (fileInfo != null)
                {
                    System.IO.File.Delete(imgModel[i]);
                    fileInfo.Delete();

                }

            }

            return true;
        }

        public bool UploadedMultipleFileImage(List<string> value)
        {
            Boolean _status = false;

            if (value.Count != 0)
            {
               DeleteUpload(value);
               _status = true;
            }

            return _status;
        }
    }
}

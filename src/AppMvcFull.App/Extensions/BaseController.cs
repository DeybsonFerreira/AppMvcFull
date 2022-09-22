using AppMvcFull.App.ViewModels;
using AppMvcFull.Business.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AppMvcFull.App.Extensions
{
    public class BaseController : Controller
    {
        private readonly INotification _notification;
        private readonly IWebHostEnvironment _env;
        protected BaseController(INotification notification, IWebHostEnvironment env)
        {
            _notification = notification;
            _env = env;
        }

        protected bool CheckValidation()
        {
            return !_notification.HasNotification();
        }

        public async Task<bool> UploadImage(IFormFile file, string imgPrefix, string folderType)
        {
            if (file.Length <= 0) return false;
            decimal imgCurrent = decimal.Round(file.Length / 1024M / 1024M, 2);
            decimal maxMegaByte = 3M;
            if (imgCurrent > maxMegaByte)
            {
                ModelState.AddModelError("", $"Imagem contém {imgCurrent}MB não pode ser maior que {maxMegaByte}MB");
                return false;
            }

            Uri resourcePath = new($"{Request.Scheme}://{Request.Host}/");
            string folderPath = Path.Combine(_env.ContentRootPath, "wwwroot/Files", folderType);
            try
            {

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                folderPath += $"/{imgPrefix}";

                if (System.IO.File.Exists(folderPath))
                    System.IO.File.Delete(folderPath);

                await using FileStream fs = new(folderPath, FileMode.CreateNew);
                await file.CopyToAsync(fs);
            }
            catch (IOException ex)
            {
                ModelState.AddModelError("", $"Erro ao criar imagem <Log> ${ex.Message}");
                _ = ($"Error on create file on {resourcePath} | {ex.Message}");
                return false;
            }

            return true;
        }
        public async Task<bool> SaveProductImage(ProductViewModel modelView)
        {
            if (modelView.ImageUpload != null)
            {
                string imgPrefix = $"{modelView.Id}.jpg";
                var createFolder = await UploadImage(modelView.ImageUpload, imgPrefix, "Product");
                if (createFolder)
                    modelView.Image = imgPrefix;

                return createFolder;
            }
            return true;
        }
    }
}

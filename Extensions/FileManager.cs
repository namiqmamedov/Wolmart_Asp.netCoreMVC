using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Wolmart.Ecommerce.Models;

namespace Wolmart.Ecommerce.Extensions
{
    public static class FileManager
    {
        public static bool CheckContentType(this IFormFile file, string contentType)
        {
            return file.ContentType == contentType;
        }

        public static bool CheckFileLength(this IFormFile file, double fileLength)
        {
            return ((double)file.Length / 1024) > fileLength; // double kesriynen vermesi ucun
        }

        public async static Task<string> CreateAsync(this IFormFile file, IWebHostEnvironment env, params string[] folders)
        {
            string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            string path = Path.Combine(env.WebRootPath);

            foreach (string folder in folders)  
            {
                path = Path.Combine(path, folder); 
            }

            path = Path.Combine(path,fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}

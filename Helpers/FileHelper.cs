using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Wolmart.Ecommerce.Helpers
{
    public class FileHelper
    {
       public static void DeleteFile(IWebHostEnvironment env, string fileName, params string[] folders)
       {
            string path = Path.Combine(env.WebRootPath);

            foreach (string folder in folders)
            {
                path = Path.Combine(path, folder);
            }

            path = Path.Combine(path, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}

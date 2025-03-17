using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.Helpers
{
    public class ImageHelper
    {
        public static async Task<string> SaveBase64ImageAsync(string base64String, string fileName)
        {
            try
            {
                if (base64String.Contains(","))
                    base64String = base64String.Split(',')[1];

                byte[] imageBytes = Convert.FromBase64String(base64String);
                string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads\" + fileName;
                await File.WriteAllBytesAsync(downloadsPath, imageBytes);
                return downloadsPath;
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        public static void OpenImage(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    Console.WriteLine("File not found!");
                }
            }
            catch { }
        }
    }
}

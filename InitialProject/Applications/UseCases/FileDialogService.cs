using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Applications.UseCases
{
    public class FileDialogService
    {
        public static List<string> GetImagePaths(string initialDirectory, string pathRoot)
        {
            var imagePaths = new List<string>();

            var openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.FilterIndex = 1;
            openFileDialog.Filter = "Image Files|*.jpg;*.png;*.bmp|All Files|*.*"; //.BaseDirectory
            openFileDialog.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, initialDirectory);
            openFileDialog.RestoreDirectory = false;

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    string relativePath = fileName.Replace(openFileDialog.InitialDirectory, "").TrimStart('\\');
                    if (!string.IsNullOrEmpty(relativePath))
                    {

                        var imagePath = "../..";
                        string image = Path.Combine(pathRoot, relativePath).Replace('\\', '/');


                        imagePaths.Add(imagePath+image);
                    }
                }
            }

            return imagePaths;
        }
    }
}

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace EscapeMines.Services
{
    public class FileDialogOpener : IFileDialogOpener
    {
   
        public string GetFilePathFromDialog()
        {
            string path = string.Empty;
            Thread t = new Thread(() => path = OpenDialogForGettingPath());
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();
            return path;
        }

        private string OpenDialogForGettingPath()
		{
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }

                return string.Empty;
            }
        }
    }
}

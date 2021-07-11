using Infrastructure.Abstractions;
using Microsoft.Win32;

namespace Editor.Desktop.Services
{
    public class WinFileDialog : IFileDialog
    {
        public string ShowFileDialog()
        {
            var fileDialog = new OpenFileDialog();
            var result = fileDialog.ShowDialog();

            if (!result.Value)
            {
                return string.Empty;
            }

            return fileDialog.FileName;
        }
    }
}
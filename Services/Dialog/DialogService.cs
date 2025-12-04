using System.Windows.Forms;

namespace Services.Dialog
{
    public class DialogService : IDialogService
    {
        public bool SaveFile()
        {
            return false;
        }

        public string SelectFolder()
        {
            var dialog = new FolderBrowserDialog()
            {
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Description = "Select Folder",
            };

            return dialog.ShowDialog() == DialogResult.OK
                ? dialog.SelectedPath
                : string.Empty;
        }
    }
}

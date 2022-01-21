using System.IO;
using System.Windows.Forms;

namespace ControleDeEventos
{
    class FileActions
    {
        public static bool CreateFile(string content, string filename, string for_path = "")
        {
            string FilePath = Path.Combine(for_path, filename);
            try
            {
                using (StreamWriter writer = File.CreateText(FilePath))
                { writer.Write(content); }
                return File.Exists(for_path);
            }
            catch (IOException)
            {
                return false;
            }
        }
        public static bool TransferFile(string from_path, string filename, string for_path = "")
        {
            ResolveIfPathFilesExists();

            string FilePath;
            string AppPath = Application.StartupPath;
            
            if (for_path.Length > 0)
            { FilePath = Path.Combine(for_path, filename); }
            else 
            { FilePath = Path.Combine(AppPath, $"Files\\{filename}"); }

            try
            {
                FileInfo file = new FileInfo(from_path);
                FileInfo newFile = file.CopyTo(FilePath);
                return newFile.Exists;
            }
            catch (FileNotFoundException){ return false; }
        }
        public static bool DeleteFile(string path)
        {
            string NameFile = Path.GetFileName(path);
            FileInfo file = new FileInfo(NameFile);
            
            if (file.Exists == true)
            {
                file.Delete();
                return file.Exists;
            }
            return file.Exists;
        }

        private static void ResolveIfPathFilesExists()
        {
            if (!Directory.Exists("Files"))
            { Directory.CreateDirectory(Path.Combine(Application.StartupPath, @"\Files")); }
        }
    }
}

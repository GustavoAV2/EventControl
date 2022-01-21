using System;
using System.IO;
using System.Collections.Generic;

namespace ControleDeEventos
{
    class Validations
    {
        public static bool ValidateName(string name)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(name, "^[a-zA-Z0-9\x20]+$"))
            {  return true; }
            return false;

        }
        public static bool ValidateDate(DateTime event_date)
        {
            DateTime now = DateTime.Now;
            if (now.Date < event_date.Date)
            { return true; }
            return false;
        }
        public static bool ValidateEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            { return false; }
        }
        public static bool ValidatePhoneNumber(string number)
        {
            try
            {
               return System.Text.RegularExpressions.Regex.Match(number, @"^(\+[0-9]{9})$").Success;
            }
            catch
            { return false; }
        }
        public static bool ValidatTypeFile(string pathFile)
        {
            List<string> ValidExt = new List<string>() { "pdf", "docx", "xlsx", "csv" };
            string FileName = Path.GetFileName(pathFile);
            string Ext = FileName.Split('.')[1];

            foreach (var value in ValidExt)
            {
                if (Ext == value)
                { return true; }
            }
            return false;
        }
        public static bool ValidatIfFileExists(string pathFile)
        {
            try
            {
                FileInfo file = new FileInfo(pathFile);
                return file.Exists;
            }
            catch (FileNotFoundException) { return false; }
        }
    }
}

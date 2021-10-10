using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeEventos
{
    class Client
    {
        private string _email;
        private string _name;
        private string _phone;
        public int ID { get; set; }
        public string Name {
            get { return _name; }
            set { _name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.Trim()); }
        }
        public string Email {
            get { return _email; }
            set { _email = value.Trim(); }
        }
        public string Phone {
            get { return _phone; }
            set { _phone = value.Trim(); }
        }
    
        public Client()
        {
            Name = "";
            Email = "";
            Phone = "";
        }
        public Client(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }
        public Client(int id, string name, string email, string phone) : this(name, email, phone)
        {
            ID = id;
            Name = name;
            Email = email;
            Phone = phone;
        }
    }
}

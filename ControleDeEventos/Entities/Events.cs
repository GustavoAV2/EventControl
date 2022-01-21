using System;
using System.Globalization;
using System.Collections.Generic;

namespace ControleDeEventos
{
    class Events
    {
        private string _local;
        private string _type;
        private string _file;
        public List<Client> Clients = new List<Client>();
        public int ID { get; }
        public double Signal { get; set; }
        public double Price { get; set; }
        public DateTime DateOfEvent { get; set; }
        public DateTime ContractDate { get; }
        public string PathFile
        {
            get { return _file; }
            set { _file = value.Trim(); }
        }
        public string Local { 
            get { return _local; } 
            set { _local = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.Trim()); }
        }
        public string FirstClient
        { get { 
               if (Clients.Count > 0)
               { return Clients[0].Name; }
                return "";
            } }
        public string SecondClient
        { get 
            { 
                if (Clients.Count > 1)
                { return Clients[1].Name; }
                return "";
            } 
        }
        public string Type
        {
            get { return _type; }
            set { _type = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.Trim()); }
        }
     
        //public Package Package;
        public bool Active;
        public Events()
        {
            Price = 0;
            Signal = 0;
            PathFile = "";
            Active = true;
            ContractDate = DateTime.Today;
        }
        public Events(double signal, double price, string local, DateTime date_of_event, string type, string path_file = "", int id = 0)
        {
            ID = id;
            Type = type;
            Price = price;
            Local = local;
            Active = true;
            Signal = signal;
            PathFile = path_file;
            DateOfEvent = date_of_event;
            ContractDate = DateTime.Today;
        }
        public Events(double signal, double price, string local, DateTime date_of_event, Client[] clients, string type, string path_file = "", int id = 0) : this(signal, price, local, date_of_event, type, path_file, id)
        { SetClients(clients); }

        public Events(double signal, double price, string local, DateTime date_of_event, DateTime contract_date, Client[] clients, string type, string path_file = "", int id = 0) : this(signal, price, local, date_of_event, clients, type, path_file, id)
        { ContractDate = contract_date; }

        public void SetClients(Client[] clients)
        {
            if (clients.Length > 1)
            {Type = "Wedding";}
            else
            {Type = "Party";}

            Clients.Clear();
            Clients.AddRange(clients);
        }
    }
}

using System;
using System.Data;
using System.Collections.Generic;

namespace ControleDeEventos
{
    class Controller
    {
        private Database _data = new Database();
        internal int InsertClient(Client client, int eventId = 0)
        {
            string columns = "T_NAME, T_EMAIL, N_PHONE";
            string values = $"'{client.Name}','{client.Email}','{client.Phone}'";

            if (eventId > 0)
            {
                columns += ", FK_EVENT";
                values += $", {eventId}";
            }
            return _data.SetQuery($"INSERT INTO client ({columns}) VALUES({values});");
        }
        internal bool UpdateClient(Client client, int id)
        {
            string columns = $"T_NAME='{client.Name}',T_EMAIL='{client.Email}',N_PHONE='{client.Phone}'";
            string command = $"UPDATE client SET {columns} WHERE N_ID='{id}';";
            int ex = _data.SetQuery(command);
            if (ex > 0)
            { return true; }
            return false;
        }
        internal bool DeleteClientById(int id)
        {
            string command = $"DELETE FROM client WHERE N_ID='{id}';";
            int ex = _data.SetQuery(command);
            if (ex > 0)
            { return true; }
            return false;
        }
        internal Client GetClientByID(int id)
        {
            string columns = "N_ID,T_NAME,T_EMAIL,N_PHONE";
            string command = $"SELECT {columns} FROM client WHERE N_ID = '{id}';";
            DataTable dt = _data.GetQuery(command);
            Client client = new Client(
                int.Parse(dt.Rows[0].ItemArray[0].ToString()),
                dt.Rows[0].ItemArray[1].ToString(),
                dt.Rows[0].ItemArray[2].ToString(),
                dt.Rows[0].ItemArray[3].ToString()
                );
            return client;
        }
        internal DataTable GetClientByEventId(int eventId)
        {
            string command = $"SELECT * FROM client WHERE FK_EVENT = '{eventId}';";
            return _data.GetQuery(command);
        }
        internal int InsertEvent(Events obj_event, List<string> listIds = null)
        {
            string columns = "N_PRICE, N_SIGNAL, DATE_OF_EVENT, DATE_CONTRACT, T_LOCAL, T_TYPE, T_PATH_FILE";
            string values = $"'{obj_event.Price}','{obj_event.Signal}','{obj_event.DateOfEvent}'," +
                $"'{obj_event.ContractDate}','{obj_event.Local}','{obj_event.Type}','{obj_event.PathFile}'";

            if (listIds != null)
            {
                if (listIds.Count > 1) {
                    columns += ", FIRST_CLIENT, SECOND_CLIENT";
                    values += $",'{listIds[0]}','{listIds[1]}'";
                }
                else{
                    columns += ", FIRST_CLIENT";
                    values += $", '{listIds[0]}'";
                }
            }
            return _data.SetQuery($"INSERT INTO event ({columns}) " + $"VALUES ({values});");
        }
        internal bool UpdateEvent(Events ev, int id)
        {
            string columns = $"N_PRICE={ev.Price},N_SIGNAL={ev.Signal},DATE_OF_EVENT='{ev.DateOfEvent}'," +
                $"T_LOCAL='{ev.Local}',FIRST_CLIENT={ev.Clients[0].ID},T_PATH_FILE='{ev.PathFile}'";
            if (ev.Clients.Count > 1)
            {
                columns += $",SECOND_CLIENT={ev.Clients[1].ID}";
            }
            string command = $"UPDATE event SET {columns} WHERE N_ID = {id}";
            int ex = _data.SetQuery(command);
            if (ex > 0)
            { return true; }
            return false;
        }
        internal bool DeleteEventById(int id)
        {
            string command = $"DELETE FROM event WHERE N_ID = '{id}';";
            int ex = _data.SetQuery(command);
            if (ex > 0)
            { return true; }
            return false;
        }
        internal List<Events> GetEventsByDate(DateTime date_of_event)
        {
            string columns = "N_SIGNAL,N_PRICE,T_LOCAL,DATE_OF_EVENT," +
                "DATE_CONTRACT,T_TYPE,FIRST_CLIENT,SECOND_CLIENT,T_PATH_FILE,N_ID";
            string command = $"SELECT {columns} FROM event " +
                $"WHERE DATE_OF_EVENT = '{date_of_event}';";
            DataTable dt = _data.GetQuery(command);

            return ConvertDataTableToEvents(dt);
        }
        internal Events GetEventByID(int id)
        {
            string columns = "N_SIGNAL,N_PRICE,T_LOCAL,DATE_OF_EVENT," +
                "DATE_CONTRACT,T_TYPE,FIRST_CLIENT,SECOND_CLIENT,T_PATH_FILE,N_ID";
            string command = $"SELECT {columns} FROM event WHERE N_ID = '{id}';";
            DataTable dt = _data.GetQuery(command);

            return ConvertDataTableToEvents(dt)[0];
        }
        internal List<Events> GetEventsByLocalEvent(string local)
        {
            string columns = "N_SIGNAL,N_PRICE,T_LOCAL,DATE_OF_EVENT," +
                "DATE_CONTRACT,T_TYPE,FIRST_CLIENT,SECOND_CLIENT,T_PATH_FILE,N_ID";
            DataTable dataTable = _data.GetQuery($"SELECT {columns} FROM event WHERE T_LOCAL LIKE '%{local}%';;");

            if (dataTable.Rows.Count > 0)
            {
                return ConvertDataTableToEvents(dataTable);
            }
            return new List<Events>();
        }
        internal List<Events> GetEventsByClientName(string name)
        {
            string columns = "FK_EVENT";
            string columnsEvent = "N_SIGNAL,N_PRICE,T_LOCAL,DATE_OF_EVENT," +
                "DATE_CONTRACT,T_TYPE,FIRST_CLIENT,SECOND_CLIENT,T_PATH_FILE,N_ID";
            DataTable dt = _data.GetQuery($"SELECT {columns} FROM client WHERE T_NAME LIKE '%{name}%';");

            if (dt.Rows.Count > 0)
            {
                string first_id = dt.Rows[0].ItemArray[0].ToString();
                string command = $"SELECT {columnsEvent} FROM event WHERE N_ID={first_id}";

                for (int i=1; i < dt.Rows.Count; i++)
                {
                    string id = dt.Rows[i].ItemArray[0].ToString();
                    command += $" OR N_ID={id}";
                }

                DataTable dataTable = _data.GetQuery(command);
                return ConvertDataTableToEvents(dataTable);
            }
            return new List<Events>();
        }
        internal List<Events> GetEvents() 
        {
            string columns = "N_SIGNAL,N_PRICE,T_LOCAL,DATE_OF_EVENT," +
                "DATE_CONTRACT,T_TYPE,FIRST_CLIENT,SECOND_CLIENT,T_PATH_FILE,N_ID";
            DataTable dt = _data.GetQuery($"SELECT {columns} FROM event;");

            return ConvertDataTableToEvents(dt);
        }
        private List<Events> ConvertDataTableToEvents(DataTable dt)
        {
            List<Events> ListEvents = new List<Events>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Client[] ListClients;

                if (dt.Rows[i].ItemArray[6].GetType().Name != "DBNull")
                {
                    Client cli1 = GetClientByID(int.Parse(dt.Rows[i].ItemArray[6].ToString()));
                    if (dt.Rows[i].ItemArray[5].ToString() == "Wedding")
                    {
                        Client cli2 = GetClientByID(int.Parse(dt.Rows[i].ItemArray[7].ToString()));
                        ListClients = new Client[2] { cli1, cli2 };
                    }
                    else
                    { ListClients = new Client[1] { cli1 }; }
                }
                else
                { ListClients = new Client[2]; }

                ListEvents.Add(new Events(
                    double.Parse(dt.Rows[i].ItemArray[0].ToString()),
                    double.Parse(dt.Rows[i].ItemArray[1].ToString()),
                    dt.Rows[i].ItemArray[2].ToString(),
                    DateTime.Parse(dt.Rows[i].ItemArray[3].ToString()),
                    DateTime.Parse(dt.Rows[i].ItemArray[4].ToString()),
                    ListClients,
                    dt.Rows[i].ItemArray[5].ToString(),
                    dt.Rows[i].ItemArray[8].ToString(),
                    int.Parse(dt.Rows[i].ItemArray[9].ToString())
                ));
            }
            return ListEvents;
        }
    }
}

using System;
using System.Data;
using System.Collections.Generic;

namespace ControleDeEventos
{
    class Actions
    {
        private Controller Control = new Controller();

        public Events GetEventsById(int id)
        {
            Events Event = Control.GetEventByID(id);
            return Event;
        }
        public List<Events> GetEventsByClientName(string name)
        {
            return Control.GetEventsByClientName(name);
        }
        public List<Events> GetEventsByLocalEvent(string local)
        {
            return Control.GetEventsByLocalEvent(local);
        }
        public List<Events> GetEventsByDate(DateTime date_of_event)
        {
            return Control.GetEventsByDate(date_of_event);
        }
        public List<Events> GetEvents()
        {
            return Control.GetEvents();
        }
        public bool InsertEvent(Events _event)
        {
            Control.InsertEvent(_event);
            int EventID = GetEventsByDate(_event.DateOfEvent)[0].ID;

            if (_event.PathFile.Length > 0)
            {
                string NameFile = System.IO.Path.GetFileName(_event.PathFile);
                string ext = NameFile.Split('.')[1];
                string NewFileName = $"{EventID}_DocumentEvent.{ext}";
                FileActions.TransferFile(_event.PathFile, NewFileName);
                _event.PathFile = NewFileName;
            }

            for (int i=0; i<_event.Clients.Count; i++ )
            { Control.InsertClient(_event.Clients[i], EventID); }
            
            DataTable searchDb = Control.GetClientByEventId(EventID);
            
            for (int i = 0; i < searchDb.Rows.Count; i++)
            { _event.Clients[i].ID = int.Parse(searchDb.Rows[i].ItemArray[0].ToString()); }

            return Control.UpdateEvent(_event, EventID);
        }
        public bool UpdateEvent(Events Event)
        {
            Control.UpdateClient(Event.Clients[0], Event.Clients[0].ID);
            if (Event.Clients.Count > 1)
            {
                Control.UpdateClient(Event.Clients[1], Event.Clients[1].ID);
            }
            return Control.UpdateEvent(Event, Event.ID);
        }
        public bool DeleteEvent(Events _event)
        {
            bool ex =  Control.DeleteEventById(_event.ID);
            for (int i = 0; i < _event.Clients.Count; i++)
            {
                Control.DeleteClientById(_event.Clients[i].ID);
            }
            return ex;
        }
        public bool ExportEventFile(Events _event, string for_path)
        {
            string NewFileName;
            string PathFile = System.IO.Path.GetFileName(_event.PathFile);

            string[] l = for_path.Split('\\');
            string file = l[l.Length - 1];
            string[] fileAndExt = file.Split('.');
            string ForPath = for_path.Replace(file, "");

            if (fileAndExt.Length > 1)
            { file = fileAndExt[0]; }

            string ext = PathFile.Split('.')[1];
            NewFileName = $"{file}.{ext}";

            string fromPath = "Files\\" + _event.PathFile;
            return FileActions.TransferFile(fromPath, NewFileName, ForPath);
        }
    }
}

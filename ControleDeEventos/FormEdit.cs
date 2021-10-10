using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControleDeEventos
{
    public partial class FormEdit : Form
    {
        private Events Event;
        private Actions ActionObject = new Actions();
        private bool ChangeFile = false;
        string newFile;

        public FormEdit(int id)
        {
            InitializeComponent();
            Event = ActionObject.GetEventsById(id);
            textPathFile.Enabled = false;
            textPathFile.Text = Event.PathFile;
        }
        private void FormEdit_Load(object sender, EventArgs e)
        {
            if (Event.Type == "Wedding")
            { boxEventsOptions.SelectedIndex = 0; }
            else
            { 
                boxEventsOptions.SelectedIndex = 1;
                labelInputHusband.Text = "Cliente";
                labelInputWife.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                textWife.Visible = false;
                textBox7.Visible = false;
                textBox6.Visible = false;
            }
            boxEventsOptions.Enabled = false;

            textBox1.Text = Event.Local;
            dateTimePicker.Value = Event.DateOfEvent;
            valueSignal.Value = (decimal)Event.Signal;
            valueTotal.Value = (decimal)Event.Price;

            textHusband.Text = Event.Clients[0].Name;
            textBox4.Text = Event.Clients[0].Email;
            textBox5.Text = Event.Clients[0].Phone;

            if (boxEventsOptions.SelectedIndex == 0)
            {
                textWife.Text = Event.Clients[1].Name;
                textBox7.Text = Event.Clients[1].Email;
                textBox6.Text = Event.Clients[1].Phone;
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        { this.Close(); }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            bool verify = true;
            if (ChangeFile == true)
            {
                string NameFile = System.IO.Path.GetFileName(newFile);
                string ext = NameFile.Split('.')[1];
                string NewFileName = $"{Event.ID}_DocumentEvent.{ext}";

                if (FileActions.TransferFile(newFile, NewFileName))
                { 
                    FileActions.DeleteFile(Event.PathFile);
                    Event.PathFile = NewFileName;
                }
                else
                {
                    MessageBox.Show("Não foi possivel localizar o arquivo " + NameFile);
                    verify = false;
                }
            }
            if (verify)
            {
                bool exc = ActionObject.UpdateEvent(Event);
                if (exc == true)
                { MessageBox.Show("Atualizado com sucesso!"); }
                else
                { MessageBox.Show("Não foi possível atualizar este evento!"); }
            }
        }
        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            Event.DateOfEvent = dateTimePicker.Value;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Event.Local = textBox1.Text;
        }
        private void textHusband_TextChanged(object sender, EventArgs e)
        {
            Event.Clients[0].Name = textHusband.Text;
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            Event.Clients[0].Email = textBox4.Text;
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            Event.Clients[0].Phone = textBox5.Text;
        }
        private void textWife_TextChanged(object sender, EventArgs e)
        {
            Event.Clients[1].Name = textWife.Text;
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            Event.Clients[1].Email = textBox7.Text;
        }
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            Event.Clients[1].Phone = textBox6.Text;
        }
        private void valueTotal_ValueChanged(object sender, EventArgs e)
        {
            Event.Price = (double)valueTotal.Value;
            double value = Event.Price - Event.Signal;
            txtMoney.Text = "R$ " + value.ToString() + ",00";
        }
        private void valueSignal_ValueChanged(object sender, EventArgs e)
        {
            Event.Signal = (double)valueSignal.Value;
            double value = Event.Price - Event.Signal;
            txtMoney.Text = "R$ " + value.ToString() + ",00";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All files (*.*)|*.*"; // file types, that will be allowed to upload
            dialog.Multiselect = false; // allow/deny user to upload more than one file at a time
            if (dialog.ShowDialog() == DialogResult.OK) // if user clicked OK
            {
                newFile = dialog.FileName; // get name of file
                textPathFile.Text = Event.PathFile;
            }
        }
    }
}

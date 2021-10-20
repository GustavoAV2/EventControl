using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ControleDeEventos
{
    public partial class Form1 : Form
    {
        List<Events> ListEvents { get; set; }
        Actions Action = new Actions(); 
        Client Client1 = new Client();
        Client Client2 = new Client();
        Events EventSelected;
        Events Event = new Events();

        public Form1()
        {
            InitializeComponent();
            comboBoxSearch.SelectedIndex = 0;
            boxEventsOptions.SelectedIndex = 0;
            Event.DateOfEvent = dateTimePicker.Value.Date;
            textPathFile.Enabled = false;
            textBox4.Visible = false;
            textBox5.Visible = false;
            textBox6.Visible = false;
            textBox7.Visible = false;

            SearchInput(comboBoxSearch.SelectedItem.ToString());
            ListEvents = Action.GetEvents();

            if (ListEvents.Count > 0)
            { SetCellContent(ListEvents[0]); }

            dataGridView1.DataSource = ListEvents;
            dataGridView1.RowHeadersVisible = false;
            DefinedDataGridView();
        }

        private void Form1_Load(object sender, EventArgs e){ }

        private void DefinedDataGridView()
        {
            dataGridView1.Columns[0].HeaderText = "Data do Evento";
            dataGridView1.Columns[0].DataPropertyName = "DateOfEvent";
            dataGridView1.Columns[1].HeaderText = "Local do Evento";
            dataGridView1.Columns[1].DataPropertyName = "Local";
            dataGridView1.Columns[2].HeaderText = "Sinal";
            dataGridView1.Columns[2].DataPropertyName = "Signal";
            dataGridView1.Columns[3].HeaderText = "Preço Total";
            dataGridView1.Columns[3].DataPropertyName = "Price";
            dataGridView1.Columns[4].HeaderText = "Cliente";
            dataGridView1.Columns[4].DataPropertyName = "FirstClient";

            dataGridView1.Columns.Add("ColumnClient", "Cliente (2)");
            dataGridView1.Columns.Add("ContractDate", "Data do Contrato");
            dataGridView1.Columns[5].DataPropertyName = "SecondClient";
            dataGridView1.Columns[6].DataPropertyName = "ContractDate";
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Event.DateOfEvent = dateTimePicker.Value;
        }

        private void textHusband_TextChanged(object sender, EventArgs e)
        {
            Client1.Name = textHusband.Text;
        }

        private void textWife_TextChanged(object sender, EventArgs e)
        {
            Client2.Name = textWife.Text;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                EventSelected = dataGridView1.SelectedRows[0].DataBoundItem as Events;
                SetCellContent(EventSelected);
            }
        }

        private void boxEventsOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (boxEventsOptions.SelectedItem.ToString() == "Festa")
            {
                this.labelInputHusband.Text = "Cliente:";
                this.labelInputWife.Visible = false;
                this.textWife.Visible = false;

                checkContactClient2.Checked = false;
                checkContactClient2.Visible = false;
            }
            else if (boxEventsOptions.SelectedItem.ToString() == "Casamento")
            {
                this.labelInputHusband.Text = "Marido:";
                this.labelInputWife.Visible = true;
                this.textWife.Visible = true;
                checkContactClient2.Visible = true;
            }
        }

        private void valueSignal_ValueChanged(object sender, EventArgs e)
        {
            Event.Price = (double)valueTotal.Value;
            Event.Signal = (double)valueSignal.Value;
            double num = Event.Price - Event.Signal;
            txtMoney.Text = "R$ " + num.ToString() + ",00";
        }

        private void valueTotal_ValueChanged(object sender, EventArgs e)
        {
            Event.Price = (double)valueTotal.Value;
            Event.Signal = (double)valueSignal.Value;
            double num = Event.Price - Event.Signal;
            txtMoney.Text = "R$ " + num.ToString() + ",00";
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
             Client1.Email = textBox4.Text;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Event.Local = textBox1.Text;
        }

        private void checkContactClient1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkContactClient1.Checked)
            {
                label10.Visible = true;
                label9.Visible = true;
                textBox4.Visible = true;
                textBox5.Visible = true;
            }
            else
            {
                label10.Visible = false;
                label9.Visible = false;
                textBox4.Visible = false;
                textBox5.Visible = false;
            }
        }
        private void checkContactClient2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkContactClient2.Checked)
            {
                label4.Visible = true;
                label5.Visible = true;
                textBox7.Visible = true;
                textBox6.Visible = true;
            }
            else
            {
                label4.Visible = false;
                label5.Visible = false;
                textBox7.Visible = false;
                textBox6.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool validate = true;
            Client[] listClients;

            if (Client2.Name.Length > 0)
            {
                validate = ValidateClient(Client2);
                listClients = new Client[] { Client1, Client2 }; 
            }
            else
            { listClients = new Client[] { Client1 }; }

            if (validate)
            {
                if (ValidateClient(Client1) && ValidateEvent(Event))
                {
                    if (Action.GetEventsByDate(Event.DateOfEvent).Count == 0 && validate)
                    {
                        Event.SetClients(listClients);
                        Action.InsertEvent(Event);
                        MessageBox.Show("Evento Cadastrado!");
                    }
                    else
                    {  MessageBox.Show("Já existe um evento cadastrado nesta DATA!"); }
                }
            }
        }
        private bool ValidateEvent(Events _event)
        {
            if (!Validations.ValidateDate(_event.DateOfEvent))
            {
                MessageBox.Show("DATA DO EVENTO invalida!\n A data é anterior ao dia de hoje.");
                return false;
            }
            else if (!Validations.ValidateName(_event.Local))
            {
                MessageBox.Show("LOCAL DO EVENTO invalido!\n Preencha e não utilize caracteres especiais.");
                return false;
            }
            else if (_event.Signal < 1 | _event.Price < 1)
            {
                MessageBox.Show("O SINAL e o VALOR não podem ser nulos!");
                return false;
            }
            return true;
        }
        private bool ValidateClient(Client client)
        {
            if (!Validations.ValidateName(client.Name))
            {
                MessageBox.Show("NOME invalido!\n Preencha e não utilize caracteres especiais.");
                return false;
            }

            if (client.Email.Length > 0)
            {
                if (!Validations.ValidateEmail(client.Email))
                {
                    MessageBox.Show("E-MAIL invalido!\n Verifique a ortografia do e-mail.");
                    return false;
                }
            }

            if (client.Phone.Length > 0)
            {
                //else if (!Validations.ValidatePhoneNumber(client.Phone))
                //{
                //    MessageBox.Show("Telefone invalido!\n Verifique a ortografia do e-mail.");
                //    return false;
                //}
            }
            return true;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            Client1.Phone = textBox5.Text;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            Client2.Email = textBox7.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            Client2.Phone = textBox6.Text;
        }

        private void textHusband_TextChanged_1(object sender, EventArgs e)
        {
            Client1.Name = textHusband.Text;
        }

        private void textWife_TextChanged_1(object sender, EventArgs e)
        {
            Client2.Name = textWife.Text;
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            Event.DateOfEvent = dateTimePicker.Value.Date;
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            Event.Local = textBox1.Text;
        }
        private void tabPage2_Click(object sender, EventArgs e)
        {
            comboBoxSearch.SelectedIndex = 0;
            SearchInput(comboBoxSearch.SelectedItem.ToString());

            ListEvents = Action.GetEvents();
            if (ListEvents.Count > 0)
            { SetCellContent(ListEvents[0]); }
            dataGridView1.DataSource = ListEvents;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EventSelected = dataGridView1.SelectedRows[0].DataBoundItem as Events;
            const string message = "Deseja realmente deletar esse evento?";
            const string caption = "Form Closing";
            var result = MessageBox.Show(
                message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question
                );

            if (result == DialogResult.Yes){
                Action.DeleteEvent(EventSelected);
                dataGridView1.DataSource = Action.GetEvents();
                MessageBox.Show("Deletado com sucesso!");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            EventSelected = dataGridView1.SelectedRows[0].DataBoundItem as Events;
            FormEdit WindowUpdate = new FormEdit(EventSelected.ID);
            WindowUpdate.Show();
        }
        private void SetCellContent(Events _event)
        {
            double rest = _event.Price - _event.Signal;

            lblLocal.Text = "Local: " + _event.Local;
            if (_event.Type == "Wedding")
            { lblTypeOfEvent.Text = "Tipo do Evento: Casamento"; }
            else
            { lblTypeOfEvent.Text = "Tipo do Evento: Festa"; }

            if (_event.Clients.Count > 0)
            {
                lblClient1.Text = "Nome: " + _event.Clients[0].Name;
                lblClient1Email.Text = "E-mail: " + _event.Clients[0].Email;
                lblClient1Phone.Text = "Tel: " + _event.Clients[0].Phone;

                if (_event.Clients.Count > 1)
                {
                    lblClient2.Text = "Nome: " + _event.Clients[1].Name;
                    lblClient2Email.Text = "E-mail: " + _event.Clients[1].Email;
                    lblClient2Phone.Text = "Tel: " + _event.Clients[1].Phone;
                }
                else
                {
                    lblClient2.Text = "";
                    lblClient2Email.Text = "";
                    lblClient2Phone.Text = "";
                }
            }

            lblRestore.Text = "Faltante: R$" + rest.ToString();
            lblSignal.Text = "Sinal: R$" + _event.Signal.ToString();
            lblTotal.Text = "Total: R$" + _event.Price.ToString();
            lblEventDate.Text = "Data do Evento:" + _event.DateOfEvent.ToShortDateString();
            lblContractDate.Text = "Data do Contrato:" + _event.ContractDate.ToShortDateString();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            const string msg = "Deseja realmente limpar TODOS os campos?";
            const string caption = "Form Closing";
            var result = MessageBox.Show(msg, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Event = new Events();
                textBox1.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textWife.Text = "";
                valueTotal.Value = 0;
                valueSignal.Value = 0;
                textHusband.Text = "";
                textPathFile.Text = "";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchInput(comboBoxSearch.SelectedItem.ToString());
        }

        private void SearchInput(string search = "Todos")
        {
            if (search == "Todos")
            {
                textBoxSearch.Text = "";
                textBoxSearch.Visible = true;
                textBoxSearch.Enabled = false;
                timePickerSearch.Visible = false;
                timePickerSearch.Enabled = false;
            }
            else if (search == "Nome" || search == "Local")
            {
                textBoxSearch.Enabled = true;
                textBoxSearch.Visible = true;
                timePickerSearch.Visible = false;
                timePickerSearch.Enabled = false;
            }
            else
            {
                textBoxSearch.Enabled = false;
                textBoxSearch.Visible = false;
                timePickerSearch.Visible = true;
                timePickerSearch.Enabled = true;
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (comboBoxSearch.Text == "Data do Evento")
            {  dataGridView1.DataSource = Action.GetEventsByDate(timePickerSearch.Value.Date); }
            else if (comboBoxSearch.Text == "Nome")
            { dataGridView1.DataSource = Action.GetEventsByClientName(textBoxSearch.Text); }
            else if (comboBoxSearch.Text == "Local")
            { dataGridView1.DataSource = Action.GetEventsByLocalEvent(textBoxSearch.Text); }
            else
            { dataGridView1.DataSource = Action.GetEvents(); }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All files (*.*)|*.*"; // file types, that will be allowed to upload
            dialog.Multiselect = false; // allow/deny user to upload more than one file at a time
            if (dialog.ShowDialog() == DialogResult.OK) // if user clicked OK
            {
                if (Validations.ValidatTypeFile(dialog.FileName))
                {
                    Event.PathFile = dialog.FileName; // get name of file
                    textPathFile.Text = Event.PathFile;
                }
                else
                { MessageBox.Show("O arquivo deve ser do tipo Excel ou PDF."); }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            Events _event = dataGridView1.SelectedRows[0].DataBoundItem as Events;

            //define o titulo
            dialog.Title = "Salvar Arquivo";

            //Define as extensões permitidas
            dialog.Filter = "All files (*.*)|*.*";
            
            //define o indice do filtro
            dialog.FilterIndex = 0;

            //Atribui um valor vazio ao nome do arquivo
            string ext = _event.PathFile.Split('.')[1];
            dialog.FileName = _event.Local.Replace(" ", "_").Trim() + $"_{DateTime.Now.ToString("ddMMyyyy")}" + $".{ext}";

            //Define a extensão padrão como .txt
            //dialog.DefaultExt = ".txt";
            
            //define o diretório padrão
            dialog.InitialDirectory = @"c:\Documents";
            
            //restaura o diretorio atual antes de fechar a janela
            dialog.RestoreDirectory = true;

            //Abre a caixa de dialogo e determina qual botão foi pressionado
            DialogResult result = dialog.ShowDialog();

            //Se o ousuário pressionar o botão Salvar
            string error_message = "Não foi possivel completar a transferência.\r\nVerifique o nome e extensão do arquivo.";
            try
            {
                if (result == DialogResult.OK)
                {
                    if (Action.ExportEventFile(_event, dialog.FileName))
                    { MessageBox.Show("Transferência CONCLUIDA!"); }
                    else
                    { MessageBox.Show(error_message); }
                }
                else
                { MessageBox.Show("Transferência CANCELADA!"); }
            }
            catch
            { MessageBox.Show(error_message); }
        }
    }
}

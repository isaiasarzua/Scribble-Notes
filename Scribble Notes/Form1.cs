using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scribble_Notes
{
    public partial class Form1 : Form
    {
        DataTable table;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            table = new DataTable();
            table.Columns.Add("Notes", typeof(String));
            table.Columns.Add("Time", typeof(DateTime));

            dataGridView1.DataSource = table;

            dataGridView1.Columns["Notes"].Width = 125;
            dataGridView1.Columns["Time"].Width = 72;
        }

        private void SaveNote()
        {
            //Check for null or whitespace to prevent saving empty notes.
            if (string.IsNullOrWhiteSpace(textNote.Text))
            {
                return;
            }

            DataRow newRow = table.NewRow();

            newRow["Notes"] = textNote.Text;
            newRow["Time"] = DateTime.Now;

            table.Rows.Add(newRow);

            textNote.Clear();
            labelTimeSaved.Text = "";
            labelSavedNotesTip.Hide();
        }

        private void ClearNoteText()
        {
            textNote.Clear();
            labelTimeSaved.Text = "";
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            //Check for null or whitespace...
            if (string.IsNullOrWhiteSpace(textNote.Text))
            {
                ClearNoteText();
            }
            else
            {
                //...if text is not null, ask user if they want to save their text before starting a new one.
                string msgBoxContent = "You are creating a new note without saving your current one. Would you like to save this note?";

                string msgBoxTitle = "Warning";

                var msgBoxResult = MessageBox.Show(msgBoxContent, msgBoxTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (msgBoxResult == DialogResult.Yes)
                {
                    SaveNote();
                }
                else if (msgBoxResult == DialogResult.No)
                {
                    ClearNoteText();
                }
                else
                {
                    return;
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveNote();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            //Check with user before deleting note.
            string msgBoxContent = "Are you sure you want to delete this note?";

            string msgBoxTitle = "Warning";

            var msgBoxResult = MessageBox.Show(msgBoxContent, msgBoxTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (msgBoxResult == DialogResult.Yes)
            {
                int index = dataGridView1.CurrentCell.RowIndex;

                table.Rows[index].Delete();

                ClearNoteText();
            }
            else if (msgBoxResult == DialogResult.No)
            {
                return;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Retrieve note and date from datatable
            int index = dataGridView1.CurrentCell.RowIndex;
            DateTime date = table.Rows[index].Field<DateTime>(1);

            if (index > -1)
            {
                textNote.Text = table.Rows[index].ItemArray[0].ToString();
                labelTimeSaved.Text = "This note was saved " + date.DayOfWeek + " " + date.ToShortDateString() + " at " + date.ToShortTimeString() + ".";
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InNumbers
{
    public partial class ManageUserCapacity : Form
    {
        public ManageUserCapacity()
        {
            InitializeComponent();
            LoadEmployee();
            InitGrid();
        }

        private void LoadEmployee()
        {
            cmbEmployee.DataSource = Common.LoadEmployee(true);
            cmbEmployee.SelectedIndex = 0;
            cmbEmployee.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void InitGrid()
        {
            dgwDaysOff.Rows.Clear();
            Thread.Sleep(500);

            dgwDaysOff.ColumnCount = 2;
            dgwDaysOff.Columns[0].Name = "id";
            dgwDaysOff.Columns[1].Name = "Day Off";
            dgwDaysOff.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgwDaysOff.Columns[0].Visible = false;


            if (cmbEmployee.SelectedIndex > 0)
            {
                DataTable dt = Common.DataReturn("SELECT * FROM Capacity WHERE Employee = " + ((InNumbers.Common.ComboboxItem)cmbEmployee.SelectedItem).Value +
                                                                  " AND DayOff >= Date() ORDER BY DayOff");
                foreach (DataRow row in dt.Rows)
                {
                    string[] gridRow = new string[] { row["Id"].ToString(),
                                                  row["DayOff"].ToString().Split(' ')[0]
                    };
                    dgwDaysOff.Rows.Add(gridRow);
                }
            }
        }

        private void BtnAddCapacity_Click(object sender, EventArgs e)
        {
            if (cmbEmployee.SelectedIndex < 1)
            {
                MessageBox.Show("Please select employee", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Check for past date
            if (DateTime.Compare(monthCalendar1.SelectionRange.Start, DateTime.Today) < 0)
            {
                MessageBox.Show("Please select today or future date", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

                DataTable dt = Common.DataReturn("SELECT * FROM Capacity WHERE Employee = " + ((InNumbers.Common.ComboboxItem)cmbEmployee.SelectedItem).Value +
                                                                   " AND DayOff >= Date()");

            foreach (DataRow row in dt.Rows)
            {
                if (DateTime.Compare(monthCalendar1.SelectionRange.Start, (System.DateTime)row["DayOff"]) == 0)
                {
                    MessageBox.Show("Duplicate date, please select another date", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            try
            {
                OleDbCommand cmd = null;
                cmd = new OleDbCommand("INSERT INTO Capacity (Employee, DayOff) " +
                                                  "VALUES(@Employee, @DayOff)", Common.FileConnection);
                cmd.Parameters.Add("@Employee", OleDbType.VarChar).Value = ((InNumbers.Common.ComboboxItem)cmbEmployee.SelectedItem).Value;
                cmd.Parameters.Add("@DayOff", OleDbType.Date).Value = monthCalendar1.SelectionRange.Start;

                cmd.ExecuteNonQuery();
                InitGrid();
            }
            catch (Exception ex)
            {
                string m = ex.Message;
            }
        }

        private void CmbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitGrid();
        }

        private void BtnDeleteCapacity_Click(object sender, EventArgs e)
        {
            if (dgwDaysOff.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select any day off to delete", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    var confirmResult = MessageBox.Show("Delete this date ??",
                                     "Confirm Deletion!!",
                                     MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    { 
                         OleDbCommand cmd = new OleDbCommand("DELETE * FROM Capacity WHERE id = " + dgwDaysOff.SelectedRows[0].Cells[0].Value, Common.FileConnection);
                        cmd.ExecuteNonQuery();
                        InitGrid();
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Common.FileConnection.Close();
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

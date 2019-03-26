using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;


namespace InNumbers
{
    public partial class MasterTaskAdd : Form
    {
        private Master _parent = null;
        Dictionary<int, string> companiesWithOriginalTrackId = new Dictionary<int, string>();


        public MasterTaskAdd(Master parent)
        {
            InitializeComponent();
            _parent = parent;
            LoadCustomers();

            cmbPartner.DataSource = Common.LoadPartner(true);
            cmbPartner.SelectedIndex = 0;

            cmbTask.DataSource = Common.InitTasks();
            cmbTask.SelectedIndex = 0;

            cmbEmployee.DataSource = Common.LoadEmployee(true);
            cmbEmployee.SelectedIndex = 0;

            txtHrsBudgeted.KeyPress += new KeyPressEventHandler(Common.OnlyNumbers);
            cmbClients.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPartner.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTask.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEmployee.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void OnlyNumbers(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void LoadCustomers()
        {
            try
            {
                var customerSource = new AutoCompleteStringCollection();
                customerSource.AddRange(new string[] { "Please select customer" });
                int index = 1;
                foreach (DataRow itemRow in Common.DataReturnClientTrack("SELECT CompanyId, CompanyName FROM Companies WHERE Companyname IS NOT NULL AND Companyname <> '' ORDER BY CompanyName").Rows)
                {
                    customerSource.AddRange(new string[] { itemRow["CompanyName"].ToString().Trim() });
                    companiesWithOriginalTrackId.Add(index++, itemRow["CompanyId"].ToString());
                }

                cmbClients.DataSource = customerSource;
                cmbClients.AutoCompleteCustomSource = customerSource;
                cmbClients.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cmbClients.AutoCompleteSource = AutoCompleteSource.CustomSource;
                cmbClients.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelTask_Click(object sender, EventArgs e)
        {
            _parent.ReloadData();
            this.Close();
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            string errorMessage = string.Empty;
            //Validate Input
            if (cmbClients.SelectedIndex == 0)
            {
                errorMessage = "Please select customer" + Environment.NewLine;
            }
            if (cmbTask.SelectedIndex == 0)
            {
                errorMessage += "Please select task " + Environment.NewLine;
            }

            if (txtCustomTaskDescription.Visible && txtCustomTaskDescription.Text == "")
            {
                errorMessage += "Please enter custom task " + Environment.NewLine;
            }

            if (cmbPartner.SelectedIndex == 0)
            {
                errorMessage += "Please select partner " + Environment.NewLine;
            }
            if (cmbEmployee.SelectedIndex == 0)
            {
                errorMessage += "Please select employee " + Environment.NewLine;
            }
            if (txtHrsBudgeted.Text == string.Empty)
            {
                errorMessage += "Please enter budgeted hours" + Environment.NewLine;
            }

            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                //Check if item exists
                DataTable dt = Common.DataReturn("SELECT * FROM MasterTasks WHERE Client = '" + cmbClients.SelectedItem.ToString() +
                                                                    "' AND Task = '" + cmbTask.SelectedItem.ToString() +
                                                                    "' AND Format(DateDue, 'yyyy-mm-dd') = '" + dtpDateIn.Value.ToString().Split(' ')[0] + "'");

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Duplicate client, task and due date", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                OleDbCommand cmd = null;
                try
                {
                    string task = txtCustomTaskDescription.Visible ? "Custom:" + txtCustomTaskDescription.Text : cmbTask.SelectedItem.ToString();
                    cmd = new OleDbCommand("INSERT INTO MasterTasks (Client, Task, Partner, DateIn, DateDue, " +
                                               "Employee, ScheduleDate,HrsBudgeted, ClientTrackCompanyId) " +
                                               "VALUES(@Client, @Task, @Partner, @DateIn, @DateDue, @Employee, @ScheduleDate,@HrsBudgeted, @ClientTrackCompanyId)", Common.FileConnection);
                    cmd.Parameters.Add("@Client", OleDbType.VarChar).Value = cmbClients.SelectedItem.ToString();
                    cmd.Parameters.Add("@Task", OleDbType.VarChar).Value = task;
                    cmd.Parameters.Add("@Partner", OleDbType.VarChar).Value = Convert.ToInt32(((InNumbers.Common.ComboboxItem)cmbPartner.SelectedItem).Value);
                    cmd.Parameters.Add("@DateIn", OleDbType.Date).Value = dtpDateIn.Value;
                    cmd.Parameters.Add("@DateDue", OleDbType.Date).Value = dtpDateDue.Value;
                    cmd.Parameters.Add("@Employee", OleDbType.Char).Value = ((InNumbers.Common.ComboboxItem)cmbEmployee.SelectedItem).Value;
                    cmd.Parameters.Add("@ScheduleDate", OleDbType.Date).Value = dtpScheduleDate.Value;
                    cmd.Parameters.Add("@HrsBudgeted", OleDbType.Integer).Value = txtHrsBudgeted.Text;
                    cmd.Parameters.Add("@ClientTrackCompanyId", OleDbType.Integer).Value = companiesWithOriginalTrackId[cmbClients.SelectedIndex];

                    cmd.ExecuteNonQuery();
                    _parent.ReloadData();
                    this.Close();
                }
                catch (Exception exp) { string m = exp.Message; }
                finally
                {
                    Common.FileConnection.Close();
                }
            }
        }

        private void cmbTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If custom showadditional field for custome taks description
            if (cmbTask.SelectedItem.ToString() == "Custom")
            {
                lblCustomTaskLabel.Visible = true;
                txtCustomTaskDescription.Visible = true;
            }
            else
            {
                lblCustomTaskLabel.Visible = false;
                txtCustomTaskDescription.Visible = false;
            }
        }
    }
}

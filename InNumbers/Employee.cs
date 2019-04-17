using System;
using System.Data;
using System.Windows.Forms;

namespace InNumbers
{
    public partial class Employee : Form
    {
        private int currentSelectedRowIndex = -1;
        public Employee()
        {
            InitializeComponent();
            this.Text = Program.currentUserFullName + " Form";

            LoadData();
        }

        public void LoadData()
        {
            dgwEmployeeTasks.Rows.Clear();

            dgwEmployeeTasks.ColumnCount = 9;
            dgwEmployeeTasks.Columns[0].Name = "Manager";
            dgwEmployeeTasks.Columns[1].Name = "id";
            dgwEmployeeTasks.Columns[2].Name = "Client";
            dgwEmployeeTasks.Columns[3].Name = "Task";
            dgwEmployeeTasks.Columns[4].Name = "Schedule Date";
            dgwEmployeeTasks.Columns[5].Name = "Due Date";
            dgwEmployeeTasks.Columns[6].Name = "Day's To Due Date";
            dgwEmployeeTasks.Columns[7].Name = "Hours Budgeted";
            dgwEmployeeTasks.Columns[8].Name = "Variance";
            dgwEmployeeTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgwEmployeeTasks.Columns[1].Visible = false;
            #region SELECT FROM FILE
            try
            {
                foreach (DataRow itemRow in Common.DataReturn("SELECT * FROM MasterTasks WHERE isClosed = false AND employee = '" + Program.currentUserId + "' OR (ManagerId = " + Program.currentUserId + " AND ForReview IS NOT NULL)ORDER BY ScheduleDate").Rows)
                {
                    if (itemRow["ManagerId"].ToString() != Program.currentUserId)
                    {
                        if (itemRow["ForReview"].ToString() != "" && itemRow["RevisionDate"].ToString() == "") continue;

                    }



                    string[] dateDueArr = itemRow["DateDue"].ToString().Split(' ')[0].ToString().Split('-');
                    string[] scheduleDateArr = itemRow["ScheduleDate"].ToString().Split(' ')[0].ToString().Split('-');

                    int variance = Convert.ToInt32(itemRow["HrsBudgeted"]) - Convert.ToInt32(itemRow["WIPHours"].ToString() == "" ? "0" : itemRow["WIPHours"].ToString());
                    TimeSpan ts = Convert.ToDateTime(itemRow["DateDue"]) - DateTime.Today;

                    string[] row = new string[] { itemRow["ManagerId"].ToString() == Program.currentUserId ? "Y" : "",
                                                  itemRow["Id"].ToString(),
                                                  itemRow["Client"].ToString(),
                                                  itemRow["Task"].ToString(),
                                                  scheduleDateArr.Length == 3 ? scheduleDateArr[1] + "/" + scheduleDateArr[2] + "/" + scheduleDateArr[0] : itemRow["ScheduleDate"].ToString().Split(' ')[0],
                                                  dateDueArr.Length == 3 ? dateDueArr[1] + "/" + dateDueArr[2] + "/" + dateDueArr[0] : itemRow["DateDue"].ToString().Split(' ')[0],
                                                  ts.Days.ToString(),
                                                  itemRow["HrsBudgeted"].ToString(),
                                                  variance.ToString()
                    };
                    dgwEmployeeTasks.Rows.Add(row);
                }

                if (currentSelectedRowIndex != -1)
                {
                    dgwEmployeeTasks.Rows[currentSelectedRowIndex].Selected = true;
                }

                foreach (DataGridViewRow row in dgwEmployeeTasks.Rows)
                {
                    //Days to due date
                    TimeSpan ts = Convert.ToDateTime(row.Cells[4].Value) - DateTime.Today;
                    //if (ts.Days > 15)
                    //    row.DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                    if (ts.Days > 6 && ts.Days < 15)
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    else if (ts.Days >= 2 && ts.Days < 6)
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    else if (ts.Days < 2)
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                Common.FileConnection.Close();
            }
            #endregion
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            dgwEmployeeTasks.ClearSelection();
            string searchValue = txtSearchValue.Text;

            dgwEmployeeTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                bool showFirstSelected = false;
                foreach (DataGridViewRow row in dgwEmployeeTasks.Rows)
                {
                    if (row.Cells[1].Value.ToString().IndexOf(searchValue, 0, StringComparison.InvariantCultureIgnoreCase) != -1)
                    {
                        row.Selected = true;

                        if (!showFirstSelected)
                        {
                            dgwEmployeeTasks.FirstDisplayedScrollingRowIndex = dgwEmployeeTasks.SelectedRows[0].Index;
                            showFirstSelected = true;
                        }
                        //break;
                    }
                }

                if (dgwEmployeeTasks.SelectedRows.Count == 0)
                    MessageBox.Show("Search returned nothing", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Employee_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgwEmployeeTasks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select any task to edit", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (dgwEmployeeTasks.SelectedRows.Count > 1)
                {
                    MessageBox.Show("Please select only one row to edit", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (dgwEmployeeTasks.SelectedRows[0].Cells[0].Value.ToString() == "Y")
                    {
                        MessageBox.Show("Can't open task for edit", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        currentSelectedRowIndex = dgwEmployeeTasks.CurrentCell.RowIndex;
                        EmployeeTasks mt = new EmployeeTasks(this, Convert.ToInt32(dgwEmployeeTasks.SelectedRows[0].Cells[1].Value));
                        mt.ShowDialog();
                    }
                }
            }
        }

        public void ReloadData()
        {
            LoadData();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to exit ??",
                                  "Confirm Exit!!",
                                  MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
                Application.Exit();
        }

        private void BtnCalendar_Click(object sender, EventArgs e)
        {
            Calendar ct = new Calendar(Convert.ToInt32(Program.currentUserId));
            ct.ShowDialog();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}

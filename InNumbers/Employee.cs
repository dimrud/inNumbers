using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace InNumbers
{
    public partial class Employee : Form
    {
        private int currentSelectedRowIndex = -1;
        private bool isManager = false;
        public Employee()
        {
            InitializeComponent();
            this.Text = Program.currentUserFullName + " Form";
            dgwEmployeeTasks.SelectionChanged += DataGridView_SelectionChanged;
            CheckRole();
            //LoadData();
        }

        private void CheckRole()
        {
            foreach (DataRow itemRow in Common.DataReturn("SELECT Role FROM LoginInfo " +
                                                              "WHERE Id = Val('" + Program.currentUserId + "')").Rows)
            {
                if (itemRow["Role"].Equals("Manager"))
                {
                    btnCloseTask.Visible = true;
                    isManager = true;
                    LoadManagerData();
                }
                else
                {
                    btnCloseTask.Visible = false;
                    LoadEmployeeData();
                }
            }
        }
        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
           // if (dgwEmployeeTasks.SelectedRows.Count != 0)
          //  {
                //MessageBox.Show(dgwEmployeeTasks.SelectedRows[0].Cells[1].Value.ToString());
                // foreach (DataRow itemRow in Common.DataReturn("SELECT * FROM MasterTasks WHERE id = " + dgwEmployeeTasks.SelectedRows[0].Cells[1].Value.ToString()).Rows)
                // {
                //if (dgwEmployeeTasks.SelectedRows[0].Cells[10].Value.ToString() == Program.currentUserId && dgwEmployeeTasks.SelectedRows[0].Cells[11].Value.ToString() != Program.currentUserId)
                //if (dgwEmployeeTasks.SelectedRows[0].Cells[10].Value.ToString() == Program.currentUserId)
                //{
                //    btnCloseTask.Visible = true;
                //}
                //else
                //{
                //    btnCloseTask.Visible = false;
                //}
                //  }
           // }
        }

        private void LoadManagerData()
        {
            int totalRows = 0;
            dgwEmployeeTasks.Rows.Clear();

            dgwEmployeeTasks.ColumnCount = 12;
            dgwEmployeeTasks.Columns[0].Name = "id";
            dgwEmployeeTasks.Columns[1].Name = "Client";
            dgwEmployeeTasks.Columns[2].Name = "Task";
            dgwEmployeeTasks.Columns[3].Name = "Employee";
            dgwEmployeeTasks.Columns[4].Name = "Schedule Date";
            dgwEmployeeTasks.Columns[5].Name = "Due Date";
            dgwEmployeeTasks.Columns[6].Name = "Day's To Due Date";
            dgwEmployeeTasks.Columns[7].Name = "Variance";
            dgwEmployeeTasks.Columns[8].Name = "Hrs To Complete";
            dgwEmployeeTasks.Columns[9].Name = "Ready For Review";
            dgwEmployeeTasks.Columns[10].Name = "Ready For 2nd Review";//"Ask Partner";
            dgwEmployeeTasks.Columns[11].Name = "Partner";
            dgwEmployeeTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgwEmployeeTasks.Columns[0].Visible = false;
            dgwEmployeeTasks.Columns[11].Visible = false;
            dgwEmployeeTasks.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            #region SELECT FROM FILE
            try
            {
                string selectStr = "SELECT Id, Client, Employee, Task, HrsBudgeted, HoursToCompletion, DateDue, ScheduleDate, ForReview, AskPartner, WIPHours, For2Review, Partner FROM MasterTasks " +
                            "WHERE isClosed = False AND (employee = '" + Program.currentUserId + "' OR (ManagerId = " + Program.currentUserId + " AND ForReview IS NOT NULL)) ";
                foreach (DataRow itemRow in Common.DataReturn(selectStr).Rows)
                {
                    string employeeName = string.Empty;
                    foreach (DataRow employee in Common.DataReturn("SELECT * FROM LoginInfo WHERE Id = " + itemRow["Employee"].ToString()).Rows)
                    {
                        employeeName = employee["FirstName"].ToString()[0] + "." + employee["LastName"].ToString()[0] + ".";
                    }

                    string[] dateDueArr = itemRow["DateDue"].ToString().Split(' ')[0].ToString().Split('-');

                    string[] scheduleDateArr = itemRow["ScheduleDate"].ToString().Split(' ')[0].ToString().Split('-');
                    string[] forReviewArr = itemRow["ForReview"].ToString().Split(' ')[0].ToString().Split('-');

                    string[] for2ReviewArr = itemRow["For2Review"].ToString().Split(' ')[0].ToString().Split('-');

                    TimeSpan ts = Convert.ToDateTime(itemRow["DateDue"]) - DateTime.Today;

                    int variance = Convert.ToInt32(itemRow["HrsBudgeted"]) - Convert.ToInt32(itemRow["WIPHours"].ToString() == "" ? "0" : itemRow["WIPHours"].ToString());

                    string[] row = new string[] { itemRow["Id"].ToString(),
                                                  itemRow["Client"].ToString(),
                                                  itemRow["Task"].ToString(),
                                                  employeeName,
                                                  scheduleDateArr.Length == 3 ? scheduleDateArr[1] + "/" + scheduleDateArr[2] + "/" + scheduleDateArr[0] : itemRow["ScheduleDate"].ToString().Split(' ')[0],
                                                  dateDueArr.Length == 3 ? dateDueArr[1] + "/" + dateDueArr[2] + "/" + dateDueArr[0] : itemRow["DateDue"].ToString().Split(' ')[0],
                                                  ts.Days.ToString(),
                                                  variance.ToString(),
                                                  itemRow["HoursToCompletion"].ToString(),
                                                  forReviewArr.Length ==  3 ? forReviewArr[1] + "/" + forReviewArr[2] + "/" + forReviewArr[0] :itemRow["ForReview"].ToString().Split(' ')[0],
                                                  for2ReviewArr.Length == 3 ? for2ReviewArr[1] + "/" + for2ReviewArr[2] + "/" + for2ReviewArr[0] : itemRow["For2Review"].ToString().Split(' ')[0],

                };
                    dgwEmployeeTasks.Rows.Add(row);
                    totalRows++;
                }

                lblTotalRows.Text = totalRows.ToString();
                foreach (DataGridViewRow row in dgwEmployeeTasks.Rows)
                {
                    //Days to due date
                    TimeSpan ts = Convert.ToDateTime(row.Cells[5].Value) - DateTime.Today;
                    if (ts.Days < 0)
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                    else
                    {
                        if (Math.Abs(ts.Days) > 6 && Math.Abs(ts.Days) < 15)
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                        else if (Math.Abs(ts.Days) >= 2 && Math.Abs(ts.Days) <= 6)
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                        else if (Math.Abs(ts.Days) < 2)
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dgwEmployeeTasks.Refresh();
                Common.FileConnection.Close();
            }
            #endregion
        }

        public void LoadEmployeeData()
        {
            dgwEmployeeTasks.Rows.Clear();
            int totalRows = 0;
            dgwEmployeeTasks.ColumnCount = 12;
            dgwEmployeeTasks.Columns[0].Name = "Manager";
            dgwEmployeeTasks.Columns[1].Name = "id";
            dgwEmployeeTasks.Columns[2].Name = "Client";
            dgwEmployeeTasks.Columns[3].Name = "Task";
            dgwEmployeeTasks.Columns[4].Name = "Eployee Name";
            dgwEmployeeTasks.Columns[5].Name = "Schedule Date";
            dgwEmployeeTasks.Columns[6].Name = "Due Date";
            dgwEmployeeTasks.Columns[7].Name = "Day's To Due Date";
            dgwEmployeeTasks.Columns[8].Name = "Hours Budgeted";
            dgwEmployeeTasks.Columns[9].Name = "Variance";
            dgwEmployeeTasks.Columns[10].Name = "ManagerId";
            dgwEmployeeTasks.Columns[11].Name = "EmployeeId";
            dgwEmployeeTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgwEmployeeTasks.Columns[1].Visible = false;
            dgwEmployeeTasks.Columns[4].Visible = false;
            dgwEmployeeTasks.Columns[10].Visible = false;
            dgwEmployeeTasks.Columns[11].Visible = false;
            #region SELECT FROM FILE
            try
            {
                foreach (DataRow itemRow in Common.DataReturn("SELECT MasterTasks.*, (LoginInfo.FirstName + LoginInfo.LastName) AS EmployeeName  FROM MasterTasks INNER JOIN LoginInfo  on Val (MasterTasks.Employee) = LoginInfo.Id  " +
                                                              "WHERE isClosed = False AND (employee = '" + Program.currentUserId + "' OR (ManagerId = " + Program.currentUserId + " AND ForReview IS NOT NULL)) " +
                                                              "ORDER BY ScheduleDate").Rows)
                {
                    if (itemRow["ManagerId"].ToString() != Program.currentUserId)
                    {
                        if (itemRow["ForReview"].ToString() != "" && itemRow["RevisionDate"].ToString() == "") continue;
                    }
                    else
                    {
                        dgwEmployeeTasks.Columns[4].Visible = true;
                    }

                    string[] dateDueArr = itemRow["DateDue"].ToString().Split(' ')[0].ToString().Split('-');
                    string[] scheduleDateArr = itemRow["ScheduleDate"].ToString().Split(' ')[0].ToString().Split('-');

                    int variance = Convert.ToInt32(itemRow["HrsBudgeted"]) - Convert.ToInt32(itemRow["WIPHours"].ToString() == "" ? "0" : itemRow["WIPHours"].ToString());
                    TimeSpan ts = Convert.ToDateTime(itemRow["DateDue"]) - DateTime.Today;

                    string[] row = new string[] { itemRow["ManagerId"].ToString() == Program.currentUserId ? "Y" : "",
                                                  itemRow["Id"].ToString(),
                                                  itemRow["Client"].ToString(),
                                                  itemRow["Task"].ToString(),
                                                  itemRow["EmployeeName"].ToString(),
                                                  scheduleDateArr.Length == 3 ? scheduleDateArr[1] + "/" + scheduleDateArr[2] + "/" + scheduleDateArr[0] : itemRow["ScheduleDate"].ToString().Split(' ')[0],
                                                  dateDueArr.Length == 3 ? dateDueArr[1] + "/" + dateDueArr[2] + "/" + dateDueArr[0] : itemRow["DateDue"].ToString().Split(' ')[0],
                                                  ts.Days.ToString(),
                                                  itemRow["HrsBudgeted"].ToString(),
                                                  variance.ToString(),
                                                  itemRow["ManagerId"].ToString(),
                                                  itemRow["Employee"].ToString()
                    };
                    dgwEmployeeTasks.Rows.Add(row);
                    totalRows++;
                }

                if (currentSelectedRowIndex != -1)
                {
                    dgwEmployeeTasks.Rows[currentSelectedRowIndex].Selected = true;
                }

                foreach (DataGridViewRow row in dgwEmployeeTasks.Rows)
                {
                    //Days to due date
                    TimeSpan ts = Convert.ToDateTime(row.Cells[6].Value) - DateTime.Today;
                    if (ts.Days < 0)
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                    else
                    {
                        if (Math.Abs(ts.Days) > 6 && Math.Abs(ts.Days) < 15)
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                        else if (Math.Abs(ts.Days) >= 2 && Math.Abs(ts.Days) <= 6)
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                        else if (Math.Abs(ts.Days) < 2)
                            row.DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
                    }
                }
                lblTotalRows.Text = totalRows.ToString();
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
                    //if (dgwEmployeeTasks.SelectedRows[0].Cells[0].Value.ToString() == "Y" && dgwEmployeeTasks.SelectedRows[0].Cells[11].Value.ToString() != Program.currentUserId)
                    if (isManager)// dgwEmployeeTasks.SelectedRows[0].Cells[10].Value.ToString() == Program.currentUserId)
                    {
                        //MasterTasks mt = new MasterTasks(this, Convert.ToInt32(dgwMasterTasks.SelectedRows[0].Cells[0].Value));
                        //mt.ShowDialog();

                        currentSelectedRowIndex = dgwEmployeeTasks.CurrentCell.RowIndex;
                        MasterTasks mt = new MasterTasks(this, Convert.ToInt32(dgwEmployeeTasks.SelectedRows[0].Cells[0].Value));
                        mt.ShowDialog();
                        //MessageBox.Show("Can't open task for edit", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (isManager)
                LoadManagerData();
            else
                LoadEmployeeData();
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
            ReloadData();
        }

        private void BtnCloseTask_Click(object sender, EventArgs e)
        {
            if (dgwEmployeeTasks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select any task to close", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (dgwEmployeeTasks.SelectedRows.Count > 1)
                {
                    MessageBox.Show("Please select only one row to edit", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {

                    var confirmResult = MessageBox.Show("This task will be closed",
                                                     "Confirm Closing Task!!",
                                                      MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        OleDbCommand cmd = null;
                        if(isManager)
                        cmd = new OleDbCommand("UPDATE MasterTasks SET isClosed = True" +
                                               " WHERE id = " + dgwEmployeeTasks.SelectedRows[0].Cells[0].Value, Common.FileConnection);
                        else
                            cmd = new OleDbCommand("UPDATE MasterTasks SET isClosed = True" +
                                                                           " WHERE id = " + dgwEmployeeTasks.SelectedRows[0].Cells[1].Value, Common.FileConnection);
                        cmd.ExecuteNonQuery();
                        ReloadData();
                    }
                }
            }
        }
    }
}

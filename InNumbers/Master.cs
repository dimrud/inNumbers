using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using static InNumbers.Common;

namespace InNumbers
{
    public partial class Master : Form
    {
        int selectedPartnerIndex = 0;
       // int index = 0;
        public Master()
        {
            InitializeComponent();
            this.Text = Program.currentUserFullName + " Form";
            cmdFilterTasks.DataSource = LoadPartnerFilter();
            LoadData(selectedPartnerIndex);
        }

        private void LoadData(int selectedPartber)
        {
            int totalRows = 0;
            dgwMasterTasks.Rows.Clear();
            Thread.Sleep(500);

            dgwMasterTasks.ColumnCount = 12;
            dgwMasterTasks.Columns[0].Name = "id";
            dgwMasterTasks.Columns[1].Name = "Client";
            dgwMasterTasks.Columns[2].Name = "Task";
            dgwMasterTasks.Columns[3].Name = "Employee";
            dgwMasterTasks.Columns[4].Name = "Schedule Date";
            dgwMasterTasks.Columns[5].Name = "Due Date";
            dgwMasterTasks.Columns[6].Name = "Day's To Due Date";
            dgwMasterTasks.Columns[7].Name = "Variance";
            dgwMasterTasks.Columns[8].Name = "Hrs To Complete";
            dgwMasterTasks.Columns[9].Name = "Ready For Review";
            dgwMasterTasks.Columns[10].Name = "Ready For 2nd Review";//"Ask Partner";
            dgwMasterTasks.Columns[11].Name = "Partner";
            dgwMasterTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgwMasterTasks.Columns[0].Visible = false;
            dgwMasterTasks.Columns[11].Visible = false;
            dgwMasterTasks.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Bold);
            #region SELECT FROM FILE
            try
            {
                string aa = "SELECT Id, Client, Employee, Task, HrsBudgeted, HoursToCompletion, DateDue, ScheduleDate, ForReview, AskPartner, WIPHours, For2Review, Partner FROM MasterTasks WHERE isClosed = false " + (selectedPartnerIndex == 0 ? "" : " AND Partner = '" + selectedPartnerIndex + "'") + "  ORDER BY ScheduleDate";
                foreach (DataRow itemRow in Common.DataReturn(aa).Rows)
                {
                    string employeeName = string.Empty;
                    foreach (DataRow employee in Common.DataReturn("SELECT * FROM LoginInfo WHERE Id = " + itemRow["Employee"].ToString()).Rows)
                    {
                        //employeeName = employee["FirstName"] + " " + employee["LastName"];
                        employeeName = employee["FirstName"].ToString()[0] + "." + employee["LastName"].ToString()[0] + ".";
                    }



                    string[] dateDueArr = itemRow["DateDue"].ToString().Split(' ')[0].ToString().Split('-');

                    string[] scheduleDateArr = itemRow["ScheduleDate"].ToString().Split(' ')[0].ToString().Split('-');
                    string[] forReviewArr = itemRow["ForReview"].ToString().Split(' ')[0].ToString().Split('-');

                    string[] for2ReviewArr = itemRow["For2Review"].ToString().Split(' ')[0].ToString().Split('-');

                    TimeSpan ts = Convert.ToDateTime(itemRow["DateDue"]) - DateTime.Today;

                    int variance = Convert.ToInt32(itemRow["HrsBudgeted"]) - Convert.ToInt32(itemRow["WIPHours"].ToString() == "" ? "0" : itemRow["WIPHours"].ToString());
                    //if (ts.Days > 15)

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
                                                  //itemRow["AskPartner"].ToString()== "False" ? "No" : "Yes",
                                                  for2ReviewArr.Length == 3 ? for2ReviewArr[1] + "/" + for2ReviewArr[2] + "/" + for2ReviewArr[0] : itemRow["For2Review"].ToString().Split(' ')[0],

                };
                    dgwMasterTasks.Rows.Add(row);
                    totalRows++;
                }

                lblTotalRows.Text = totalRows.ToString();
                foreach (DataGridViewRow row in dgwMasterTasks.Rows)
                {
                    //Days to due date
                    TimeSpan ts = Convert.ToDateTime(row.Cells[5].Value) - DateTime.Today;
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
                MessageBox.Show(e.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dgwMasterTasks.Refresh();
                Common.FileConnection.Close();
            }
            #endregion

            
            //cmdFilterTasks.SelectedIndex = 0;
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgwMasterTasks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select any task to edit", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (dgwMasterTasks.SelectedRows.Count > 1)
                {
                    MessageBox.Show("Please select only one row to edit", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MasterTasks mt = new MasterTasks(this, Convert.ToInt32(dgwMasterTasks.SelectedRows[0].Cells[0].Value));
                    mt.ShowDialog();
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (dgwMasterTasks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select any task to close", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    var confirmResult = MessageBox.Show("Are you sure to close this task ??",
                                     "Confirm Closing!!",
                                     MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.Yes)
                    {
                        OleDbCommand cmd = new OleDbCommand("UPDATE MasterTasks SET isClosed = 1 WHERE id = " + dgwMasterTasks.SelectedRows[0].Cells[0].Value, Common.FileConnection);
                        cmd.ExecuteNonQuery();
                        ReloadData();
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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            dgwMasterTasks.ClearSelection();
            string searchValue = txtSearchValue.Text;

            dgwMasterTasks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                bool showFirstSelected = false;
                foreach (DataGridViewRow row in dgwMasterTasks.Rows)
                {
                    if (row.Cells[1].Value.ToString().IndexOf(searchValue, 0, StringComparison.InvariantCultureIgnoreCase) != -1)
                    {
                        row.Selected = true;

                        if (!showFirstSelected)
                        {
                            dgwMasterTasks.FirstDisplayedScrollingRowIndex = dgwMasterTasks.SelectedRows[0].Index;
                            showFirstSelected = true;
                        }
                        //break;
                    }
                }

                if (dgwMasterTasks.SelectedRows.Count == 0)
                    MessageBox.Show("Search returned nothing", "Info Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // MasterTasks mt = new MasterTasks(this, -1);
            // mt.ShowDialog();
            MasterTaskAdd mta = new MasterTaskAdd(this);
            mta.ShowDialog();
        }

        private void Master_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        public void ReloadData()
        {
            LoadData(selectedPartnerIndex);
        }

        private void BtnShowClosed_Click(object sender, EventArgs e)
        {
            ClosedTasks ct = new ClosedTasks();
            ct.ShowDialog();
        }

        private void BtnManageUsers_Click(object sender, EventArgs e)
        {
            ManageUserSelector us = new ManageUserSelector();
            us.ShowDialog();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show("Are you sure to exit ??",
                                    "Confirm Exit!!",
                                    MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
                Application.Exit();
        }

        private void BtnCalendar_Click(object sender, EventArgs e)
        {
            Calendar ct = new Calendar();
            ct.ShowDialog();
        }

        private void BtnManageCapacity_Click(object sender, EventArgs e)
        {
            ManageUserCapacity dlg = new ManageUserCapacity();
            dlg.ShowDialog();
        }

        private void CmdFilterTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedPartnerIndex = (int)((InNumbers.Common.ComboboxItem)cmdFilterTasks.SelectedItem).Value;           
            LoadData(selectedPartnerIndex);          
        }
    }
}

using System;
using System.Data;
using System.Windows.Forms;

namespace InNumbers
{
    public partial class ClosedTasks : Form
    {
        public ClosedTasks()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            dgwMasterTasks.Rows.Clear();

            dgwMasterTasks.ColumnCount = 9;
            dgwMasterTasks.Columns[0].Name = "id";
            dgwMasterTasks.Columns[1].Name = "Client";
            dgwMasterTasks.Columns[2].Name = "Task";
            dgwMasterTasks.Columns[3].Name = "Employee";
            dgwMasterTasks.Columns[4].Name = "Due Date";
            dgwMasterTasks.Columns[5].Name = "Schedule Date";
            dgwMasterTasks.Columns[6].Name = "Hrs To Complete";
            dgwMasterTasks.Columns[7].Name = "Ready For Review";
            dgwMasterTasks.Columns[8].Name = "Ask Partner";
            dgwMasterTasks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgwMasterTasks.Columns[0].Visible = false;

            #region SELECT FROM FILE
            try
            {
                foreach (DataRow itemRow in Common.DataReturn("SELECT * FROM MasterTasks WHERE isClosed = true ORDER BY ScheduleDate").Rows)
                {
                    string[] dateDueArr = itemRow["DateDue"].ToString().Split(' ')[0].ToString().Split('-');
                    string[] scheduleDateArr = itemRow["ScheduleDate"].ToString().Split(' ')[0].ToString().Split('-');
                    string[] forReviewArr = itemRow["ForReview"].ToString().Split(' ')[0].ToString().Split('-');


                    string employeeName = string.Empty;
                    foreach (DataRow employee in Common.DataReturn("SELECT * FROM LoginInfo WHERE Id = " + itemRow["Employee"].ToString()).Rows)
                    {
                        employeeName = employee["FirstName"] + " " + employee["LastName"];
                    }

                    string[] row = new string[] { itemRow["Id"].ToString(),
                                                  itemRow["Client"].ToString(),
                                                  itemRow["Task"].ToString(),
                                                  employeeName,
                                                  dateDueArr.Length == 3 ? dateDueArr[1] + "/" + dateDueArr[2] + "/" + dateDueArr[0] : itemRow["DateDue"].ToString().Split(' ')[0],
                                                  scheduleDateArr.Length == 3 ? scheduleDateArr[1] + "/" + scheduleDateArr[2] + "/" + scheduleDateArr[0] : itemRow["ScheduleDate"].ToString().Split(' ')[0],
                                                  itemRow["HrsBudgeted"].ToString(),
                                                  forReviewArr.Length == 3 ? forReviewArr[1] + "/" + forReviewArr[2] + "/" + forReviewArr[0] :itemRow["ForReview"].ToString().Split(' ')[0],
                                                  itemRow["AskPartner"].ToString()== "False" ? "No" : "Yes",
                    };
                    dgwMasterTasks.Rows.Add(row);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Common.FileConnection.Close();
            }
            #endregion
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            if (dgwMasterTasks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select any task to show", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (dgwMasterTasks.SelectedRows.Count > 1)
                {
                    MessageBox.Show("Please select only one row to show", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MasterClosedTasks mt = new MasterClosedTasks(this, Convert.ToInt32(dgwMasterTasks.SelectedRows[0].Cells[0].Value));
                    mt.ShowDialog();
                }
            }
        }

        public void ReloadData()
        {
            LoadData();
        }
    }
}

using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InNumbers
{
    public partial class MasterTasks : Form
    {
        private Master _parent = null;
        private int _taskId;
        private bool isClosing;
        //private bool _isAdd = true;

        public MasterTasks(Master parent, int taskId)
        {
            InitializeComponent();
            LoadTask(taskId);
            _taskId = taskId;
            this.Text = "Update Master Task";


            txtHoursBudgeted.KeyPress += new KeyPressEventHandler(Common.OnlyNumbers);
            txtAddTime.KeyPress += new KeyPressEventHandler(Common.OnlyNumbers);
            _parent = parent;
            dtpScheduleDate.Format = dtpDueDate.Format = DateTimePickerFormat.Custom;
            dtpScheduleDate.CustomFormat = dtpDueDate.CustomFormat = "dd-MM-yyyy";
        }

        private void LoadTask(int id)
        {
            cmbEmployee.DataSource = Common.LoadEmployee(false);
            cmbManager.DataSource = Common.LoadManager(false);
            #region SELECT FROM FILE
            foreach (DataRow itemRow in Common.DataReturn("SELECT * FROM MasterTasks WHERE id = " + id).Rows)
            {

                //Comes from Master init page
                lblClientValue.Text = itemRow["Client"].ToString();
                lblTaskValue.Text = itemRow["Task"].ToString();
                string[] lblDateInValueArr = itemRow["DateIn"].ToString() == "" ? null : itemRow["DateIn"].ToString().Split(' ')[0].ToString().Split('-');
                lblDateInValue.Text = lblDateInValueArr.Length == 3 ? lblDateInValueArr == null ? "" : lblDateInValueArr[1] + "/" + lblDateInValueArr[2] + "/" + lblDateInValueArr[0] : itemRow["DateIn"].ToString().Split(' ')[0];


                foreach (DataRow partner in Common.DataReturn("SELECT FirstName, LastName FROM LoginInfo WHERE Id = " + itemRow["Partner"].ToString()).Rows)
                {
                    lblPartnerValue.Text = partner["FirstName"] + " " + partner["LastName"];
                }

                string[] lblDueDateValueArr = itemRow["DateDue"].ToString() == "" ? null : itemRow["DateDue"].ToString().Split(' ')[0].ToString().Split('-');
                lblDueDateValue.Text = lblDueDateValueArr.Length == 3 ? lblDueDateValueArr == null ? "" : lblDueDateValueArr[1] + "/" + lblDueDateValueArr[2] + "/" + lblDueDateValueArr[0] : itemRow["DateDue"].ToString().Split(' ')[0];


                int employeeCurrentIndex = 0;
                int employeeSelectedtIndex = 0;
                foreach (Common.ComboboxItem item in cmbEmployee.Items)
                {
                    if (item.Value.ToString() == itemRow["Employee"].ToString())
                    {
                        employeeSelectedtIndex = employeeCurrentIndex;
                    }
                    employeeCurrentIndex++;
                }
                cmbEmployee.SelectedIndex = employeeSelectedtIndex;


                int managerCurrentIndex = 0;
                int managerSelectedtIndex = 0;
                foreach (Common.ComboboxItem item in cmbManager.Items)
                {
                    if (item.Value.ToString() == itemRow["ManagerId"].ToString())
                    {
                        managerSelectedtIndex = managerCurrentIndex;
                    }
                    managerCurrentIndex++;
                }
                cmbManager.SelectedIndex = managerSelectedtIndex;


                txtHoursBudgeted.Text = itemRow["HrsBudgeted"].ToString();
                dtpDueDate.Value = Convert.ToDateTime(itemRow["DateDue"]);
                dtpScheduleDate.Value = Convert.ToDateTime(itemRow["ScheduleDate"]);
                //dtpRevisionDate.Value = Convert.ToDateTime(itemRow["RevisionDate"]);

                txtAddTime.Text = itemRow["AdditionalTime"].ToString();
                int variance = Convert.ToInt32(txtHoursBudgeted.Text) - Convert.ToInt32(lblWIPHoursValue.Text == "" ? "0" : lblWIPHoursValue.Text);
                lblVariance.Text = variance.ToString();
                if (variance < 0)
                    lblVariance.BackColor = Color.Red;
                //else
                //    lblVariance.BackColor = Color.Black;

                lblVariance.Text = variance.ToString();


                string[] lblOSRequestedSentArr = itemRow["OsInfoRequestSent"].ToString().Split(' ')[0].ToString().Split('-');
                lblOSRequestedSent.Text = lblOSRequestedSentArr.Length == 3 ? lblOSRequestedSentArr == null ? "" : lblOSRequestedSentArr[1] + "/" + lblOSRequestedSentArr[2] + "/" + lblOSRequestedSentArr[0] : itemRow["OsInfoRequestSent"].ToString().Split(' ')[0];


                string[] txtFUDateArr = itemRow["FollowUpDate"].ToString().Split(' ')[0].ToString().Split('-');
                txtFUValue.Text = txtFUDateArr.Length == 3 ? txtFUDateArr[1] + "/" + txtFUDateArr[2] + "/" + txtFUDateArr[0] : itemRow["FollowUpDate"].ToString().Split(' ')[0];

                string[] txtOSInfoReceivedArr = itemRow["OSInfoDateReceived"].ToString().Split(' ')[0].ToString().Split('-');
                txtOSInfoReceived.Text = txtOSInfoReceivedArr.Length == 3 ? txtOSInfoReceivedArr[1] + "/" + txtOSInfoReceivedArr[2] + "/" + txtOSInfoReceivedArr[0] : itemRow["OSInfoDateReceived"].ToString().Split(' ')[0]; //(itemRow["OSInfoDateReceived"] != System.DBNull.Value ? Convert.ToDateTime(itemRow["OSInfoDateReceived"]) : DateTime.Today);

                string[] txtRevisionDateArr = itemRow["RevisionDate"].ToString().Split(' ')[0].ToString().Split('-');
                txtRevisionDate.Text = txtRevisionDateArr.Length == 3 ? txtRevisionDateArr[1] + "/" + txtRevisionDateArr[2] + "/" + txtRevisionDateArr[0] : itemRow["RevisionDate"].ToString().Split(' ')[0];

                string[] lblReady2ndReviewArr = itemRow["For2Review"].ToString().Split(' ')[0].ToString().Split('-');
                lblReadyFor2ndReview.Text = lblReady2ndReviewArr.Length == 3 ? lblReady2ndReviewArr[1] + "/" + lblReady2ndReviewArr[2] + "/" + lblReady2ndReviewArr[0] : itemRow["For2Review"].ToString().Split(' ')[0];



                txtHrsToComplete.Text = itemRow["HoursToCompletion"].ToString();

                //Days to due date
                TimeSpan ts = Convert.ToDateTime(itemRow["DateDue"]) - DateTime.Today;
                if (Math.Abs(ts.Days) >= 15)
                    lblDaysToDueDate.BackColor = Color.Green;
                else if (Math.Abs(ts.Days) > 6 && Math.Abs(ts.Days) < 15)
                    lblDaysToDueDate.BackColor = Color.Yellow;
                else if (ts.Days >= 2 && ts.Days <= 6)
                    lblDaysToDueDate.BackColor = Color.Red;
                else
                    Blink();

                lblDaysToDueDate.Text = ts.Days.ToString();
                //Ask Partner
                lblAskPartnerValue.Text = itemRow["AskPartner"].ToString() == "False" ? "No" : "Yes";
                //Note to partner
                rtbNotesOfPrepayer.Text = itemRow["NotesOfPrepayer"].ToString();
                //Note to partner
                rtbNotes.Text = itemRow["Notes"].ToString();  
                //ReadyFor Review
                string[] lblReadyForReviewValueArr = itemRow["ForReview"].ToString().Split(' ')[0].ToString().Split('-');
                lblReadyForReviewValue.Text = lblReadyForReviewValueArr.Length == 3 ? lblReadyForReviewValueArr[1] + "/" + lblReadyForReviewValueArr[2] + "/" + lblReadyForReviewValueArr[0] : itemRow["ForReview"].ToString().Split(' ')[0];

                double totalTime = 0;
                //Calculate Work In Progress
                //If Ready for review is not empty leave WIP empty
                if (string.IsNullOrWhiteSpace(lblReadyForReviewValue.Text))
                    //Calculate based on scheduled date 
                    //If scheduled time in future, calculate work in progress from Dockets table in Client Track 

                    foreach (DataRow docketTime in Common.DataReturnClientTrack("SELECT TotalTimeRounded, DateCreated, Date() as Today " +
                                                                             "FROM Dockets " +
                                                                             "WHERE CompanyIDNum = " + itemRow["ClientTrackCompanyId"]).Rows)
                    {
                        totalTime += Convert.ToDouble(docketTime["TotalTimeRounded"]);
                    }

                lblWIPHoursValue.Text = itemRow["WIPHours"].ToString();
            }
            #endregion
        }

        private async void Blink()
        {
            while (true)
            {
                await Task.Delay(500);
                lblDaysToDueDate.BackColor = lblDaysToDueDate.BackColor == Color.Red ? Color.Orange : Color.Red;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            _parent.ReloadData();
            this.Close();
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            OleDbCommand cmd = null;
            try
            {
                string errorMessage = string.Empty;
                if (string.IsNullOrEmpty(txtHoursBudgeted.Text))
                {
                    errorMessage += "Hours Budgeted field can't be empty" + Environment.NewLine;
                }

                if (errorMessage.Length > 0)
                {
                    MessageBox.Show(errorMessage, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    cmd = new OleDbCommand("UPDATE MasterTasks SET HrsBudgeted = " + txtHoursBudgeted.Text +
                                            ",DateDue = '" + Convert.ToDateTime(dtpDueDate.Value) + "'" +
                                            ",ScheduleDate = '" + Convert.ToDateTime(dtpScheduleDate.Value) + "'" +
                                            (txtRevisionDate.Text != "" ? ",RevisionDate = '" + Convert.ToDateTime(txtRevisionDate.Text) + "'" : "") +
                                            (txtAddTime.Text != "" ? ",AdditionalTime = " + txtAddTime.Text : "") +
                                            ",Notes= '" + rtbNotes.Text + "'" +
                                            ",Employee = " + ((InNumbers.Common.ComboboxItem)cmbEmployee.SelectedItem).Value +
                                            ",ManagerId = " + ((InNumbers.Common.ComboboxItem)cmbManager.SelectedItem).Value +
                                            " WHERE id = " + _taskId, Common.FileConnection);
                    cmd.ExecuteNonQuery();
                    _parent.ReloadData();
                    this.Close();
                }

            }
            catch (Exception exp) { string m = exp.Message; }
            finally
            {
                Common.FileConnection.Close();
            }
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }


        //private void txtWIPBox_TextChanged(object sender, EventArgs e)
        //{
        //    int variance = Convert.ToInt32(txtHoursBudgeted.Text) - Convert.ToInt32(txtWIPBox.Text == "" ? "0" : txtWIPBox.Text);
        //    if (variance < 0)
        //        lblVariance.ForeColor = Color.Red;
        //    else
        //        lblVariance.ForeColor = Color.Black;

        //    lblVariance.Text = variance.ToString();
        //}

        private void TxtHoursBudgeted_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtHoursBudgeted.Text))
            {
                return;
            }

            int variance = Convert.ToInt32(txtHoursBudgeted.Text) - Convert.ToInt32(lblWIPHoursValue.Text == "" ? "0" : lblWIPHoursValue.Text);
            if (variance < 0)
                lblVariance.BackColor = Color.Red;
            else
                lblVariance.BackColor = this.BackColor;

            lblVariance.Text = variance.ToString();
        }


        private void BtnCancel_MouseLeave(object sender, EventArgs e)
        {
            isClosing = false;
        }

        private void BtnCancel_MouseHover(object sender, EventArgs e)
        {
            isClosing = true;
        }

        private void TxtRevisionDate_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (txtRevisionDate.Text == "" || isClosing) return;

            CultureInfo ci = new CultureInfo("en-IE");

            if (!DateTime.TryParseExact(txtRevisionDate.Text, "MM/dd/yyyy", ci, DateTimeStyles.None, out DateTime rs))
            {
                MessageBox.Show("Revision date should be in format MM/DD/YYYY", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }
    }
}

using System;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InNumbers
{
    public partial class EmployeeTasks : Form
    {
        private Employee _parent = null;
        private int _taskId;
        private bool isClosing;

        public EmployeeTasks(Employee parent, int taskId)
        {
            InitializeComponent();

            _taskId = taskId;

            txtWIPHoursValue.KeyPress += new KeyPressEventHandler(Common.OnlyNumbers);
            txtHoursToComplete.KeyPress += new KeyPressEventHandler(Common.OnlyNumbers);

            //txtOSRequestSent.KeyPress += new KeyPressEventHandler(Common.OnlyNumbersAndSpecChar);  

            _parent = parent;
            cmbAskPartner.DataSource = Common.AskPartner();
            LoadTask(taskId);
        }

        private void LoadTask(int id)
        {
            #region SELECT FROM FILE
            foreach (DataRow itemRow in Common.DataReturn("SELECT * FROM MasterTasks WHERE id = " + id).Rows)
            {
                //Comes from Master init page
                lblClientValue.Text = itemRow["Client"].ToString();
                lblTaskValue.Text = itemRow["Task"].ToString();

                foreach (DataRow partner in Common.DataReturn("SELECT FirstName, LastName FROM LoginInfo WHERE ClientTrackId = " + itemRow["Partner"].ToString()).Rows)
                {
                    lblPartnerValue.Text = partner["FirstName"] + " " + partner["LastName"];
                }

                string[] lblDateInValueArr = itemRow["DateIn"].ToString() == "" ? null : itemRow["DateIn"].ToString().Split(' ')[0].ToString().Split('-');
                lblDateInValue.Text = lblDateInValueArr.Length == 3 ? lblDateInValueArr == null ? "" : lblDateInValueArr[1] + "/" + lblDateInValueArr[2] + "/" + lblDateInValueArr[0] : itemRow["DateIn"].ToString().Split(' ')[0];


                string[] lblDueDateValueArr = itemRow["DateDue"].ToString().Split(' ')[0].ToString().Split('-');
                lblDueDateValue.Text = lblDueDateValueArr.Length == 3 ? lblDueDateValueArr[1] + "/" + lblDueDateValueArr[2] + "/" + lblDueDateValueArr[0] : itemRow["DateDue"].ToString().Split(' ')[0]; //itemRow["DateDue"].ToString().Split(' ')[0].Replace(;

                string[] lblRevisionDateArr = itemRow["RevisionDate"].ToString().Split(' ')[0].ToString().Split('-');
                lblRevisionDate.Text = lblRevisionDateArr.Length == 3 ? lblRevisionDateArr[1] + "/" + lblRevisionDateArr[2] + "/" + lblRevisionDateArr[0] : itemRow["RevisionDate"].ToString().Split(' ')[0];



                lblHrsBudgetedValue.Text = itemRow["HrsBudgeted"].ToString();
                lblAdditionalTime.Text = itemRow["AdditionalTime"].ToString();

                string[] lblScheduleDateValueArr = itemRow["ScheduleDate"].ToString().Split(' ')[0].ToString().Split('-');
                lblScheduleDateValue.Text = lblScheduleDateValueArr.Length == 3 ? lblScheduleDateValueArr[1] + "/" + lblScheduleDateValueArr[2] + "/" + lblScheduleDateValueArr[0] : itemRow["ScheduleDate"].ToString().Split(' ')[0]; //itemRow["ScheduleDate"].ToString().Split(' ')[0];

                txtWIPHoursValue.Text = itemRow["WIPHours"].ToString();

                string[] txtOSRequestSentArr = itemRow["OsInfoRequestSent"].ToString().Split(' ')[0].ToString().Split('-');
                txtOSRequestSent.Text = txtOSRequestSentArr.Length == 3 ? txtOSRequestSentArr[1] + "/" + txtOSRequestSentArr[2] + "/" + txtOSRequestSentArr[0] : itemRow["OsInfoRequestSent"].ToString().Split(' ')[0];// (itemRow["OsInfoRequestSent"] != System.DBNull.Value ? Convert.ToDateTime(itemRow["OsInfoRequestSent"]) : DateTime.Today);  

                string[] txtFUDateArr = itemRow["FollowUpDate"].ToString().Split(' ')[0].ToString().Split('-');
                txtFUDate.Text = txtFUDateArr.Length == 3 ? txtFUDateArr[1] + "/" + txtFUDateArr[2] + "/" + txtFUDateArr[0] : itemRow["FollowUpDate"].ToString().Split(' ')[0];//(itemRow["FollowUpDate"] != System.DBNull.Value ? Convert.ToDateTime(itemRow["FollowUpDate"]): DateTime.Today);

                string[] txtOSInfoReceivedArr = itemRow["OSInfoDateReceived"].ToString().Split(' ')[0].ToString().Split('-');
                txtOSInfoReceived.Text = txtOSInfoReceivedArr.Length == 3 ? txtOSInfoReceivedArr[1] + "/" + txtOSInfoReceivedArr[2] + "/" + txtOSInfoReceivedArr[0] : itemRow["OSInfoDateReceived"].ToString().Split(' ')[0]; //(itemRow["OSInfoDateReceived"] != System.DBNull.Value ? Convert.ToDateTime(itemRow["OSInfoDateReceived"]) : DateTime.Today);

                txtHoursToComplete.Text = itemRow["HoursToCompletion"].ToString().Split(' ')[0].Replace('-', '/');

                //Days to due date
                TimeSpan ts = Convert.ToDateTime(itemRow["DateDue"]) - DateTime.Today;
                if (ts.Days > 15)
                    lblDaysToDueDate.BackColor = Color.Green;
                else if (ts.Days > 6 && ts.Days < 15)
                    lblDaysToDueDate.BackColor = Color.Yellow;
                else if (ts.Days > 2 && ts.Days < 5)
                    lblDaysToDueDate.BackColor = Color.Red;
                else
                    Blink();

                lblDaysToDueDate.Text = ts.Days.ToString();
                //Ask Partner
                if (itemRow["AskPartner"].ToString() == "False")
                    cmbAskPartner.SelectedIndex = 0;
                else
                    cmbAskPartner.SelectedIndex = 1;
                //Note to partner
                rtbNotes.Text = itemRow["Notes"].ToString();
                //Note to partner
                rtbNotesOfPrepayer.Text = itemRow["NotesOfPrepayer"].ToString();
                //ReadyFor Review
                string[] txtReadyForReviewArr = itemRow["ForReview"].ToString().Split(' ')[0].ToString().Split('-');
                txtReadyForReview.Text = txtReadyForReviewArr.Length == 3 ? txtReadyForReviewArr[1] + "/" + txtReadyForReviewArr[2] + "/" + txtReadyForReviewArr[0] : itemRow["ForReview"].ToString().Split(' ')[0];//itemRow["ForReview"].ToString();//(itemRow["ForReview"] != System.DBNull.Value ? Convert.ToDateTime(itemRow["ForReview"]) : DateTime.Today);  
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

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            OleDbCommand cmd = null;
            try
            {

                string errorMessage = string.Empty;
                if (string.IsNullOrEmpty(txtWIPHoursValue.Text))
                {
                    errorMessage += "WIP Hours field can't be empty" + Environment.NewLine;
                }

                if (string.IsNullOrEmpty(txtHoursToComplete.Text))
                {
                    errorMessage += "Hours To Complete field can't be empty" + Environment.NewLine;
                }


                if (errorMessage.Length > 0)
                {
                    MessageBox.Show(errorMessage, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //string sss = "UPDATE MasterTasks SET WIPHours = " + txtWIPHoursValue.Text +
                    //                        ",  OsInfoRequestSent = '" + Convert.ToDateTime(dtpOSRequestSent.Value) +
                    //                        "', FollowUpDate = '" + Convert.ToDateTime(dtpFUDate.Value) +
                    //                        "', OSInfoDateReceived = '" + Convert.ToDateTime(dtpOSInfoReceived.Value) +
                    //                        "', HoursToCompletion = " + txtHoursToComplete.Text +
                    //                        " , AskPartner = " + (cmbAskPartner.SelectedItem.ToString() == "No" ? false : true) +
                    //                        " , Notes = " + (rtbNotes.Text == String.Empty ? "''" : rtbNotes.Text) +
                    //                        " , NotesOfPrepayer = " + (rtbNotesOfPrepayer.Text == String.Empty ? "''" : rtbNotesOfPrepayer.Text) +
                    //                        " , ForReview = '" + Convert.ToDateTime(dtpReadyForReview.Value) +
                    //                        "' WHERE id = " + _taskId;
                    //DateTime date1 = new DateTime(2010, 8, 18);

                    cmd = new OleDbCommand("UPDATE MasterTasks SET WIPHours = " + txtWIPHoursValue.Text +
                                            (txtOSRequestSent.Text != "" ? ",  OsInfoRequestSent = '" + new DateTime(Convert.ToInt32(txtOSRequestSent.Text.Split('/')[2]), Convert.ToInt32(txtOSRequestSent.Text.Split('/')[0]), Convert.ToInt32(txtOSRequestSent.Text.Split('/')[1])) + "'" : "") +
                                            (txtFUDate.Text != "" ? ",  FollowUpDate = '" + new DateTime(Convert.ToInt32(txtFUDate.Text.Split('/')[2]), Convert.ToInt32(txtFUDate.Text.Split('/')[0]), Convert.ToInt32(txtFUDate.Text.Split('/')[1])) + "'" : "") +
                                            (txtOSInfoReceived.Text != "" ? ",  OSInfoDateReceived = '" + new DateTime(Convert.ToInt32(txtOSInfoReceived.Text.Split('/')[2]), Convert.ToInt32(txtOSInfoReceived.Text.Split('/')[0]), Convert.ToInt32(txtOSInfoReceived.Text.Split('/')[1])) + "'" : "") +
                                            (txtReady2ndReview.Text != "" ? ",For2Review = '" + Convert.ToDateTime(txtReady2ndReview.Text) + "'" : "") +
                                            ", HoursToCompletion = " + txtHoursToComplete.Text +
                                            " , AskPartner = " + (cmbAskPartner.SelectedItem.ToString() == "No" ? false : true) +
                                            " , Notes = '" + (rtbNotes.Text == String.Empty ? "" : rtbNotes.Text) +
                                            "' , NotesOfPrepayer = '" + (rtbNotesOfPrepayer.Text == String.Empty ? "" : rtbNotesOfPrepayer.Text) + "'" +
                                            (txtReadyForReview.Text != "" ? ",  ForReview = '" + new DateTime(Convert.ToInt32(txtReadyForReview.Text.Split('/')[2]), Convert.ToInt32(txtReadyForReview.Text.Split('/')[0]), Convert.ToInt32(txtReadyForReview.Text.Split('/')[1])) + "'" : "") +
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

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            _parent.ReloadData();
            this.Close();
        }

        private void TxtWIPHoursValue_TextChanged(object sender, EventArgs e)
        {
            int variance = Convert.ToInt32(lblHrsBudgetedValue.Text) - Convert.ToInt32(txtWIPHoursValue.Text == "" ? "0" : txtWIPHoursValue.Text);
            if (variance < 0)
                lblVariance.BackColor = Color.Red;
            else
                lblVariance.BackColor = this.BackColor;

            lblVariance.Text = variance.ToString();
        }

        void TxtOSRequestSent_Validating(object sender, CancelEventArgs e)
        {
            if (txtOSRequestSent.Text == "" || isClosing) return;

            CultureInfo ci = new CultureInfo("en-IE");

            if (!DateTime.TryParseExact(txtOSRequestSent.Text, "MM/dd/yyyy", ci, DateTimeStyles.None, out DateTime rs))

            {
                MessageBox.Show("OS Request Sent date should be in format MM/DD/YYYY", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        void TxtFUDate_Validating(object sender, CancelEventArgs e)
        {
            if (txtFUDate.Text == "" || isClosing) return;

            CultureInfo ci = new CultureInfo("en-IE");

            if (!DateTime.TryParseExact(txtOSRequestSent.Text, "MM/dd/yyyy", ci, DateTimeStyles.None, out DateTime rs))

            {
                MessageBox.Show("Follow UP date should be in format MM/DD/YYYY", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        void TxtOSInfoReceived_Validating(object sender, CancelEventArgs e)
        {
            if (txtFUDate.Text == "" || isClosing) return;

            CultureInfo ci = new CultureInfo("en-IE");

            if (!DateTime.TryParseExact(txtOSInfoReceived.Text, "MM/dd/yyyy", ci, DateTimeStyles.None, out DateTime rs))
            {
                MessageBox.Show("OS Info Received date should be in format MM/DD/YYYY", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        private void BtnCancel_MouseHover(object sender, EventArgs e)
        {
            isClosing = true;
        }

        private void BtnCancel_MouseLeave(object sender, EventArgs e)
        {
            isClosing = false;
        }

        private void TxtReady2ndReview_Validating(object sender, CancelEventArgs e)
        {
            if (txtReady2ndReview.Text == "" || isClosing) return;

            CultureInfo ci = new CultureInfo("en-IE");

            if (!DateTime.TryParseExact(txtReady2ndReview.Text, "MM/dd/yyyy", ci, DateTimeStyles.None, out DateTime rs))

            {
                MessageBox.Show("Ready for 2nd review date should be in format MM/DD/YYYY", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }
    }
}

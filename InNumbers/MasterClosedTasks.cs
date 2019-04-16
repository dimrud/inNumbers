using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InNumbers
{
    public partial class MasterClosedTasks : Form
    {
        private ClosedTasks _parent = null;
        private int _taskId;
        //private bool _isAdd = true;

        public MasterClosedTasks(ClosedTasks parent, int taskId)
        {
            InitializeComponent();
            LoadTask(taskId);
            _taskId = taskId;
            this.Text = "Update Master Task";

            //txtHoursBudgeted.KeyPress += new KeyPressEventHandler(Common.OnlyNumbers);
            _parent = parent;
        }

        private void LoadTask(int id)
        {
            cmbEmployee.DataSource = Common.LoadEmployee(false);
            #region SELECT FROM FILE
            foreach (DataRow itemRow in Common.DataReturn("SELECT * FROM MasterTasks WHERE id = " + id).Rows)
            {
                //Comes from Master init page
                lblClientValue.Text = itemRow["Client"].ToString();
                lblTaskValue.Text = itemRow["Task"].ToString();

                string[] lblDateInValueArr = itemRow["DateIn"].ToString().Split(' ')[0].ToString().Split('-');
                lblDateInValue.Text = lblDateInValueArr.Length == 3 ? lblDateInValueArr[1] + "/" + lblDateInValueArr[2] + "/" + lblDateInValueArr[0] : itemRow["DateIn"].ToString().Split(' ')[0];


               // lblDateInValue.Text = itemRow["DateIn"].ToString().Split(' ')[0];

                foreach (DataRow partner in Common.DataReturn("SELECT FirstName, LastName FROM LoginInfo WHERE Id = " + itemRow["Partner"].ToString()).Rows)
                {
                    lblPartnerValue.Text = partner["FirstName"] + " " + partner["LastName"];
                }

                string[] lblDueDateValueArr = itemRow["DateDue"].ToString().Split(' ')[0].ToString().Split('-');
                lblDueDateValue.Text = lblDueDateValueArr.Length == 3 ? lblDueDateValueArr[1] + "/" + lblDueDateValueArr[2] + "/" + lblDueDateValueArr[0] : itemRow["DateDue"].ToString().Split(' ')[0];

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

                txtHoursBudgeted.Text = itemRow["HrsBudgeted"].ToString();
                string[] lblScheduleDateArr = itemRow["ScheduleDate"].ToString().Split(' ')[0].ToString().Split('-');
                lblScheduleDate.Text = lblScheduleDateArr.Length == 3 ? lblScheduleDateArr[1] + "/" + lblScheduleDateArr[2] + "/" + lblScheduleDateArr[0] : itemRow["ScheduleDate"].ToString().Split(' ')[0];

                //dtpScheduleDate.Value = Convert.ToDateTime(itemRow["ScheduleDate"]);

                lblWIPHoursValue.Text = itemRow["WIPHours"].ToString();


                string[] lblOSRequestedSentArr = itemRow["OsInfoRequestSent"].ToString().Split(' ')[0].ToString().Split('-');
                lblOSRequestedSent.Text = lblOSRequestedSentArr.Length == 3 ? lblOSRequestedSentArr[1] + "/" + lblOSRequestedSentArr[2] + "/" + lblOSRequestedSentArr[0] : itemRow["OsInfoRequestSent"].ToString().Split(' ')[0];

                string[] txtFUDateArr = itemRow["FollowUpDate"].ToString().Split(' ')[0].ToString().Split('-');
                txtFUValue.Text = txtFUDateArr.Length == 3 ? txtFUDateArr[1] + "/" + txtFUDateArr[2] + "/" + txtFUDateArr[0] : itemRow["FollowUpDate"].ToString().Split(' ')[0];

                string[] txtOSInfoReceivedArr = itemRow["OSInfoDateReceived"].ToString().Split(' ')[0].ToString().Split('-');
                txtOSInfoReceived.Text = txtOSInfoReceivedArr.Length == 3 ? txtOSInfoReceivedArr[1] + "/" + txtOSInfoReceivedArr[2] + "/" + txtOSInfoReceivedArr[0] : itemRow["OSInfoDateReceived"].ToString().Split(' ')[0]; //(itemRow["OSInfoDateReceived"] != System.DBNull.Value ? Convert.ToDateTime(itemRow["OSInfoDateReceived"]) : DateTime.Today);

                string[] lblRevisionDateArr = itemRow["RevisionDate"].ToString().Split(' ')[0].ToString().Split('-');
                lblRevisionDate.Text = lblRevisionDateArr.Length == 3 ? lblRevisionDateArr[1] + "/" + lblRevisionDateArr[2] + "/" + lblRevisionDateArr[0] : itemRow["RevisionDate"].ToString().Split(' ')[0];

                lblAdditionalTime.Text = itemRow["AdditionalTime"].ToString();
                txtHrsToComplete.Text = itemRow["HoursToCompletion"].ToString();


                string[] lblReady2ndReviewArr = itemRow["For2Review"].ToString().Split(' ')[0].ToString().Split('-');
                lblReadyFor2ndReview.Text = lblReady2ndReviewArr.Length == 3 ? lblReady2ndReviewArr[1] + "/" + lblReady2ndReviewArr[2] + "/" + lblReady2ndReviewArr[0] : itemRow["For2Review"].ToString().Split(' ')[0];

                //Days to due date
                TimeSpan ts = Convert.ToDateTime(itemRow["DateDue"]) - DateTime.Today;
                //if (ts.Days > 15)
                //    lblDaysToDueDate.ForeColor = Color.Green;
                //else if (ts.Days > 6 && ts.Days < 15)
                //    lblDaysToDueDate.ForeColor = Color.Yellow;
                //else if (ts.Days > 2 && ts.Days < 5)
                //    lblDaysToDueDate.ForeColor = Color.Red;
                //else
                //    Blink();

                lblDaysToDueDate.Text = ts.Days.ToString();
                //Ask Partner
                lblAskPartnerValue.Text = itemRow["AskPartner"].ToString() == "False" ? "No" : "Yes";
                //Note to partner
                rtbNotes.Text = itemRow["Notes"].ToString();
                //Note to partner
                rtbNotesOfPrepayer.Text = itemRow["NotesOfPrepayer"].ToString();
                //ReadyFor Review
                string[] lblReadyForReviewValueArr = itemRow["ForReview"].ToString().Split(' ')[0].ToString().Split('-');
                lblReadyForReviewValue.Text = lblReadyForReviewValueArr.Length == 3 ? lblReadyForReviewValueArr[1] + "/" + lblReadyForReviewValueArr[2] + "/" + lblReadyForReviewValueArr[0] : itemRow["ForReview"].ToString().Split(' ')[0];


                //lblReadyForReviewValue.Text = itemRow["ForReview"].ToString().Split(' ')[0];
            }
            #endregion
        }

        //private async void Blink()
        //{
        //    while (true)
        //    {
        //        await Task.Delay(500);
        //        lblDaysToDueDate.BackColor = lblDaysToDueDate.BackColor == Color.Red ? Color.Orange : Color.Red;
        //    }
        //}

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    

        //public class ComboboxItem
        //{
        //    public string Text { get; set; }
        //    public object Value { get; set; }

        //    public override string ToString()
        //    {
        //        return Text;
        //    }
        //}

        //private void txtWIPBox_TextChanged(object sender, EventArgs e)
        //{
        //    int variance = Convert.ToInt32(txtHoursBudgeted.Text) - Convert.ToInt32(txtWIPBox.Text == "" ? "0" : txtWIPBox.Text);
        //    if (variance < 0)
        //        lblVariance.ForeColor = Color.Red;
        //    else
        //        lblVariance.ForeColor = Color.Black;

        //    lblVariance.Text = variance.ToString();
        //}

        //private void txtHoursBudgeted_TextChanged(object sender, EventArgs e)
        //{
        //    int variance = Convert.ToInt32(txtHoursBudgeted.Text) - Convert.ToInt32(lblWIPHoursValue.Text == "" ? "0" : lblWIPHoursValue.Text);
        //    if (variance < 0)
        //        lblVariance.ForeColor = Color.Red;
        //    else
        //        lblVariance.ForeColor = Color.Black;

        //    lblVariance.Text = variance.ToString();
        //}
    }
}

using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace InNumbers
{
    class Common
    {
        private static OleDbConnection _fileConnection = null;
        private static OleDbConnection _fileConnectionClientTrack = null;

        public static void OnlyNumbers(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            //// only allow one decimal point
            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}
        }

        public static void OnlyNumbersAndSpecChar(object sender, KeyPressEventArgs e)
        {
            string myStr = @"^[0-9/]+$";

            Match match = Regex.Match(e.KeyChar.ToString(), myStr, RegexOptions.IgnoreCase);
            if (match.Success)
                e.Handled = false;

            else
                e.Handled = true;
        }


        public static OleDbConnection FileConnection
        {
            get
            {
                if (_fileConnection != null && _fileConnection.State != ConnectionState.Closed)
                    return _fileConnection;
                else
                {
                    OpenFileConnection();
                    return _fileConnection;
                }
            }
        }

        public static OleDbConnection FileConnectionClientTrack
        {
            get
            {
                if (_fileConnectionClientTrack != null && _fileConnectionClientTrack.State != ConnectionState.Closed)
                    return _fileConnectionClientTrack;
                else
                {
                    OpenFileConnectionClientTrack();
                    return _fileConnectionClientTrack;
                }
            }
        }

        public static void OpenFileConnectionClientTrack()
        {
            _fileConnectionClientTrack = new OleDbConnection("Provider=Microsoft.jet.oledb.4.0;data source=" + Program.filePathClientTrack + "\\" + Program.fileNameClientTrack);
            _fileConnectionClientTrack.Open();
        }

        public static void OpenFileConnection()
        {
            _fileConnection = new OleDbConnection("Provider=Microsoft.jet.oledb.4.0;data source=" + Program.filePath + "\\" + Program.fileName);
            _fileConnection.Open();
        }

        public static void CloseFileConnectionClientTrack()
        {
            if (_fileConnectionClientTrack.State != ConnectionState.Closed)
            {
                _fileConnectionClientTrack.Close();
            }
        }

        public static void CloseFileConnection()
        {
            if (_fileConnection.State != ConnectionState.Closed)
            {
                _fileConnection.Close();
            }
        }

        #region InitTasks
        public static ComboBox.ObjectCollection InitTasks()
        {
            ComboBox cb = new ComboBox();
            cb.Items.Add("Please Select");
            cb.Items.Add("Bookkeeping");
            cb.Items.Add("HST Period Return");
            cb.Items.Add("BK Review");
            cb.Items.Add("BK & Year End File");
            cb.Items.Add("NTR");
            cb.Items.Add("Review Engagement");
            cb.Items.Add("Audit Engagement");
            cb.Items.Add("File Review");
            cb.Items.Add("T4 / T5 Preperation");
            cb.Items.Add("T4 / T5 Review");
            cb.Items.Add("T1 Preperation");
            cb.Items.Add("T1 Review");
            cb.Items.Add("CLient Meeting");
            cb.Items.Add("Custom");

            return cb.Items;
        }
        #endregion

        #region Ask Partner
        public static ComboBox.ObjectCollection AskPartner()
        {
            ComboBox cb = new ComboBox();
            cb.Items.Add("No");
            cb.Items.Add("Yes");
            return cb.Items;
        }
        #endregion

        #region Connect To App file
        public static DataTable DataReturn(string select)
        {
            DataTable dt = new DataTable();
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.jet.oledb.4.0;data source=" + Program.filePath + "\\" + Program.fileName);
            conn.Open();
            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = select;//;
            OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // close conn after complete
            dt.Load(reader);
            conn.Close();
            reader.Close();
            return dt;
        }
        #endregion 
        #region Connect To ClientTrack file
        public static DataTable DataReturnClientTrack(string select)
        {
            DataTable dt = new DataTable();
            OleDbConnection conn = new OleDbConnection("Provider=Microsoft.jet.oledb.4.0;data source=" + Program.filePathClientTrack + "\\" + Program.fileNameClientTrack);
            conn.Open();
            OleDbCommand cmd = conn.CreateCommand();
            cmd.CommandText = select;//;
            OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // close conn after complete
            dt.Load(reader);
            conn.Close();
            reader.Close();
            return dt;
        }
        #endregion 

        #region Load Employee
        public static ComboBox.ObjectCollection LoadEmployee(bool isIncludeFirstLine)
        {
            ComboBox cb = new ComboBox();
            if (isIncludeFirstLine)
                cb.Items.Add("Please select Employee");

            foreach (DataRow itemRow in DataReturn("SELECT * FROM LoginInfo WHERE Role = 'Employee' AND isWorking = true").Rows)
            {
                ComboboxItem item = new ComboboxItem
                {
                    Text = itemRow["FirstName"] + " " + itemRow["LastName"],
                    Value = itemRow["ClientTrackId"]
                };

                cb.Items.Add(item);
            }

            return cb.Items;
        }
        #endregion
        #region Load Partner
        public static ComboBox.ObjectCollection LoadPartner(bool isIncludeFirstLine)
        {
            ComboBox cb = new ComboBox();
            if (isIncludeFirstLine)
                cb.Items.Add("Please select partner");

            foreach (DataRow itemRow in DataReturn("SELECT * FROM LoginInfo WHERE Role = 'Partner' AND isWorking = true").Rows)
            {
                ComboboxItem item = new ComboboxItem
                {
                    Text = itemRow["FirstName"] + " " + itemRow["LastName"],
                    Value = itemRow["ClientTrackId"]
                };

                cb.Items.Add(item);
            }

            return cb.Items;
        }

        public static ComboBox.ObjectCollection LoadPartnerFilter()
        {
            ComboBox cb = new ComboBox();
            ComboboxItem itemAll = new ComboboxItem
            {
                Text = "Select All",
                Value = 0
            };
            cb.Items.Add(itemAll);

            foreach (DataRow itemRow in DataReturn("SELECT * FROM LoginInfo WHERE Role = 'Partner' AND isWorking = true").Rows)
            {
                ComboboxItem item = new ComboboxItem
                {
                    Text = itemRow["FirstName"] + " " + itemRow["LastName"],
                    Value = itemRow["ClientTrackId"]
                };

                cb.Items.Add(item);
            }

            return cb.Items;
        }

        #endregion

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}

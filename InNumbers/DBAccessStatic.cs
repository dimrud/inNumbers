using System;
using System.Data;
using System.Data.SqlClient;


namespace InNumbers
{
    public static class DBAccessStatic
    {
        public static string pcf = @"Data Source=10.0.0.2;Initial Catalog=Data;User Id=1;Password=123321;Connection Timeout=0;Application Name=UploadMergeCounts";
        //public static string lockDB = @"Data Source=10.0.0.2;Initial Catalog=msllockdb;User Id=rudoyd;Password=$2209%;Connection Timeout=0;";

        public static DataSet DBSelect(string Command, string ConnectionStrring)
        {
            return DBSelect(Command, ConnectionStrring, null);
        }

        public static DataSet DBSelect(string Command, string ConnectionStrring, SqlTransaction Transaction)
        {
            SqlConnection Connection = new SqlConnection(ConnectionStrring);
            //Connection.ConnectionTimeout = 60;
            DataSet myDataSet = new DataSet();
            try
            {
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(Command, Connection);
                if (Transaction != null) myDataAdapter.SelectCommand.Transaction = Transaction;
                myDataAdapter.Fill(myDataSet, "Data");
            }
            catch (Exception e)
            {
                // ErrorHandler.ProcessError(e);
            }
            return myDataSet;
        }

        public static DataSet DBSet(string Command, string ConnectionStrring)
        {
            return DBSet(Command, ConnectionStrring, null);
        }

        public static DataSet DBSet(string Command, string ConnectionStrring, params SqlParameter[] SQLParams)
        {
            SqlConnection Connection = new SqlConnection(ConnectionStrring);
            SqlCommand command1 = new SqlCommand(Command)
            {
                CommandTimeout = 0,
                Connection = Connection
            };
            DataSet myDataSet = new DataSet();
            try
            {
                Connection.Open();
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(command1);//, Connection);
                if (SQLParams != null && SQLParams.Length > 0) myDataAdapter.SelectCommand.Parameters.AddRange(SQLParams);
                myDataAdapter.Fill(myDataSet, "Data");
            }
            catch (Exception e)
            {
                //ErrorHandler.ProcessError(e);
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
            return myDataSet;
        }

        public static DataTable DBTable(string Command, string ConnectionStrring)
        {
            return DBSet(Command, ConnectionStrring).Tables[0];
        }

        public static DataTable DBTable(string Command, string ConnectionStrring, params SqlParameter[] SQLParams)
        {
            return DBSet(Command, ConnectionStrring, SQLParams).Tables[0];
        }

        public static DataRow DBRow(string Command, string ConnectionStrring)
        {
            return DBSet(Command, ConnectionStrring).Tables[0].Rows[0];
        }

        public static DataRow DBRow(string Command, string ConnectionStrring, params SqlParameter[] SQLParams)
        {
            return DBSet(Command, ConnectionStrring, SQLParams).Tables[0].Rows[0];
        }

        public static int DBDML(string Command, string ConnectionStrring)
        {
            return DBDML(Command, ConnectionStrring, null);
        }

        public static int DBDML(string Command, string ConnectionStrring, SqlTransaction Transaction)
        {
            SqlConnection Connection = new SqlConnection(ConnectionStrring);
            int Affected = 0;
            try
            {
                Connection.Open();
                SqlCommand Exec = new SqlCommand(Command, Connection);
                if (Transaction != null) Exec.Transaction = Transaction;
                Affected = Exec.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                // ErrorHandler.ProcessError(e);
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
            return Affected;
        }

        public static DataSet DBSelect(string Command, SqlConnection connection)
        {
            return DBSelect(Command, connection, null);
        }

        public static DataSet DBSelect(string Command, SqlConnection connection, SqlTransaction Transaction)
        {
            DataSet myDataSet = new DataSet();
            try
            {
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(Command, connection);
                if (Transaction != null) myDataAdapter.SelectCommand.Transaction = Transaction;
                myDataAdapter.Fill(myDataSet, "Data");
            }
            catch (Exception e)
            {
                // ErrorHandler.ProcessError(e);
            }
            return myDataSet;
        }

        public static DataSet DBSet(string Command, SqlConnection connection)
        {
            return DBSet(Command, connection, null);
        }

        public static DataSet DBSet(string Command, SqlConnection connection, params SqlParameter[] SQLParams)
        {
            DataSet myDataSet = new DataSet();
            try
            {
                SqlDataAdapter myDataAdapter = new SqlDataAdapter(Command, connection);
                if (SQLParams != null && SQLParams.Length > 0) myDataAdapter.SelectCommand.Parameters.AddRange(SQLParams);
                myDataAdapter.Fill(myDataSet, "Data");
            }
            catch (Exception e)
            {
                // ErrorHandler.ProcessError(e);
            }
            return myDataSet;
        }

        public static DataTable DBTable(string Command, SqlConnection connection)
        {
            return DBSet(Command, connection).Tables[0];
        }

        public static DataTable DBTable(string Command, SqlConnection connection, params SqlParameter[] SQLParams)
        {
            return DBSet(Command, connection, SQLParams).Tables[0];
        }

        public static DataRow DBRow(string Command, SqlConnection connection)
        {
            return DBSet(Command, connection).Tables[0].Rows[0];
        }

        public static DataRow DBRow(string Command, SqlConnection connection, params SqlParameter[] SQLParams)
        {
            return DBSet(Command, connection, SQLParams).Tables[0].Rows[0];
        }

        public static int DBDML(string Command, SqlConnection connection)
        {
            return DBDML(Command, connection, null);
        }

        public static int DBDML(string Command, SqlConnection connection, SqlTransaction Transaction)
        {
            int Affected = 0;
            try
            {
                SqlCommand Exec = new SqlCommand(Command, connection);
                if (Transaction != null) Exec.Transaction = Transaction;
                Affected = Exec.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //ErrorHandler.ProcessError(e);
            }
            return Affected;
        }

        public static string IsStringType(Type ColumnType)
        {
            switch (ColumnType.Name)
            {
                case "String": return "'";
                case "DateTime": return "'";
            }
            return "";
        }

        public static string DBResult(string Command, SqlConnection Connection)
        {
            string Result = "";
            try
            {
                Connection.Open();
                SqlCommand Exec = new SqlCommand(Command, Connection);
                Exec.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Result = e.Message;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
            return Result;
        }

        public static string DBResult(string Command, string ConnectionString)
        {
            SqlConnection Connection = new SqlConnection(ConnectionString);
            return DBResult(Command, Connection);
        }

        public static DataTable GetComboTable(string Values, string Displays)
        {
            DataTable ComboTable = new DataTable();
            ComboTable.Columns.Add(new DataColumn("Value"));
            ComboTable.Columns.Add(new DataColumn("Display"));
            ComboTable.Rows.Add("", "Select");
            for (int j = 0; j < Displays.Split(';').Length; j++) ComboTable.Rows.Add(Values.Split(';')[j], Displays.Split(';')[j]);
            return ComboTable;
        }

        #region Runs with Store procedures  (taken from new web site)
        private static SqlConnection con; //connection to data source
        //public static string ConnectionString
        //{
        //    get
        //    {
        //        return System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationData"].ToString();
        //    }
        //}

        public static void RunProcedure(string procName)
        {
            SqlCommand cmd;
            try
            {
                cmd = CreateCommand(procName, null);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                // _errorMessage = e.Message;
            }
            finally
            {
                Close();
            }
        }

        public static void RunProcedure(string procName, SqlParameter[] prams, ref DataSet myDataSet)
        {
            SqlCommand cmd;
            myDataSet = new DataSet();
            try
            {
                cmd = CreateCommand(procName, prams);
                SqlDataAdapter myAdapter = new SqlDataAdapter(cmd);
                myAdapter.Fill(myDataSet);
            }
            catch (Exception e)
            {
                string errorStr = e.Message;
            }
            finally
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
        }

        public static void RunProcedure(string procName, SqlParameter[] prams)
        {
            SqlCommand cmd;
            try
            {
                cmd = CreateCommand(procName, prams);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                string errormessage = e.Message;
            }
            finally
            {
                Close();
            }
        }


        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        //'This method takes a stored proc name and a SqlDataReader (BY REF) and returns the results    '
        //'in the same DataReader that you pass in as ref. This invokes ExecuteReader on SqlCommand type'
        //'instance                                                                                     '
        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        public static void RunProcedure(string procName, ref SqlDataReader dataReader)
        {
            SqlCommand cmd;
            try
            {
                using (cmd = CreateCommand(procName, null))
                {
                    dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                }
            }
            catch
            {
                dataReader = null;
                // _errorMessage = e.Message;
            }
            finally
            {
                Close();
            }

        }

        public static void RunProcedure(string procName, ref DataSet myDataSet)
        {
            SqlCommand cmd;
            myDataSet = new DataSet();
            try
            {
                cmd = CreateCommand(procName, null);
                SqlDataAdapter myAdapter = new SqlDataAdapter(cmd);
                myAdapter.Fill(myDataSet);
            }
            catch
            {
                // _errorMessage = e.Message;
            }
            finally
            {
                Close();
            }
        }


        public static void RunProcedure(string procName, SqlParameter[] prams, ref SqlDataReader dataReader)
        {
            SqlCommand cmd;
            try
            {
                cmd = CreateCommand(procName, prams);
                dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch
            {
                dataReader = null;
                // _errorMessage = e.Message;

            }
            finally
            {
                Close();
            }
        }


        private static SqlCommand CreateCommand(string procName, SqlParameter[] prams)
        {
            SqlCommand cmd;

            Open();

            cmd = new SqlCommand(procName, con);
            cmd.CommandTimeout = 1500;
            cmd.CommandType = CommandType.StoredProcedure;

            if (prams != null)
                foreach (SqlParameter parameter in prams)
                    cmd.Parameters.Add(parameter);

            return cmd;
        }

        #region Create SQL parameter
        public static SqlParameter MakeParameter(string paramName, object Value, SqlDbType dbType)
        {
            //SqlParameter param; param = new SqlParameter(ParamName, Value); param.Direction = ParameterDirection.Input;
            return new SqlParameter(paramName, Value) { SqlDbType = dbType, Direction = ParameterDirection.Input };
        }
        public static SqlParameter MakeParameter(string paramName, ParameterDirection direction, object Value)
        {
            return new SqlParameter(paramName, Value) { Direction = direction };
        }
        public static SqlParameter MakeParameter(string paramName, ParameterDirection direction, SqlDbType dbType, int size, string sourceColumn)
        {
            return new SqlParameter(paramName, dbType, size, sourceColumn) { Direction = direction };
        }
        public static SqlParameter MakeParameter(string paramName, ParameterDirection direction, SqlDbType dbType, byte precision, byte scale)
        {
            return new SqlParameter(paramName, dbType) { Direction = direction, Precision = precision, Scale = scale };
        }
        #endregion

        private static void Open()
        {
            if (con == null)
            {
                con = new SqlConnection(pcf);
                con.Open();
            }
            else
            {
                if ((con.State == ConnectionState.Closed) || (con.State == ConnectionState.Broken))
                    con.Open();
            }
        }

        public static void Close()
        {
            if (con != null)
                con.Close();
        }
        #endregion
    }
}

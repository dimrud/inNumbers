﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InNumbers
{
    public partial class Login : Form
    { 
        public Login()
        {
            InitializeComponent();
        }

        private void BtnSignIn_Click(object sender, EventArgs e)
        {
            DoLogin();
        }

        private void DoLogin()
        {
            bool isLoged = false;
            //Select user name and pass from file
            //OleDbConnection fileConnection = new OleDbConnection("Provider=Microsoft.jet.oledb.4.0;data source=" + Program.filePath + "\\" + Program.fileName);

            //fileConnection.Open();
            //DataTable schemaInformation = fileConnection.GetSchema("Tables");

            //foreach (DataRow tableInfo in schemaInformation.Rows)
            //{
               // if (tableInfo.ItemArray[2].ToString().Equals("LoginInfo", StringComparison.InvariantCultureIgnoreCase))
              //  {
                    //DataTable itemsDt = new DataTable();
                    //var queryUserInfo = "SELECT id, username, password, role, firstname, lastname FROM LoginInfo";
                    //var adapterUserInfo = new OleDbDataAdapter(queryUserInfo, fileConnection);

                    //OleDbCommandBuilder oleDbCommandBuilderUserInfo = new OleDbCommandBuilder(adapterUserInfo);

                    //adapterUserInfo.Fill(itemsDt);

                    foreach (DataRow itemRow in Common.DataReturn("SELECT Id, username, password, role, firstname, lastname FROM LoginInfo WHERE  isWorking = true").Rows)
                    {
                        if (itemRow["username"].ToString().Equals(txtUserName.Text) && itemRow["password"].ToString().Equals(txtPassword.Text))
                        {
                            isLoged = true;
                            Program.currentUserId = itemRow["Id"].ToString();
                            Program.currentUserFullName = itemRow["firstname"].ToString() + " " + itemRow["lastname"].ToString();

                            this.Hide();
                            if (itemRow["role"].ToString().Equals("Partner", StringComparison.InvariantCultureIgnoreCase))
                            {
                                //Show Partner Form
                                Master m = new Master();
                                m.Show();
                            }
                            else
                            {
                                //Show Employee Form
                                Employee e = new Employee();
                                e.Show();
                            }
                        }
                    }

                    if(!isLoged)
                    {

                        lblError.Text = "User name and/or password incorrect. Try again!!!";
                    }
               // }
           // }
        }

        private void LoginObj_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DoLogin();
            }
        }
    }
}

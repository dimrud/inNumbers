﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace InNumbers
{
    public partial class Calendar : Form
    {
        Dictionary<string, string> panelToDate = new Dictionary<string, string>();
        int currentEmployeeId = 0;
        public Calendar(int employeeId)
        {
            InitializeComponent();
            InitTitles();
            InitPanelWithDate();

            if (employeeId > 0)
            {
                cmbEmployee.Visible = false;
                txtEmployeeName.Visible = true;
                foreach (DataRow employee in Common.DataReturn("SELECT * FROM LoginInfo WHERE Id = " + employeeId).Rows)
                {
                    //employeeName = employee["FirstName"] + " " + employee["LastName"];
                    txtEmployeeName.Text = employee["FirstName"].ToString() + " " + employee["LastName"].ToString();
                }

                currentEmployeeId = employeeId;
                InitData();
            }
            else
            {
                cmbEmployee.Visible = true;
                txtEmployeeName.Visible = false;
                LoadEmployees();
            }


            //InitData();
        }

        private void LoadEmployees()
        {
            cmbEmployee.DataSource = Common.LoadEmployee(true);
            cmbEmployee.SelectedIndex = 0;
            cmbEmployee.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void InitTitles()
        {
            var today = DateTime.Now;
            panel1_title.Text = today.DayOfWeek.ToString().Substring(0, 3) + ", " + today.ToString("MMMM") + " " + today.Day + " " + today.Year;
            panel2_title.Text = today.AddDays(1).DayOfWeek.ToString().Substring(0, 3) + ", " + today.AddDays(1).ToString("MMMM") + " " + today.AddDays(1).Day + " " + today.AddDays(1).Year;
            panel3_title.Text = today.AddDays(2).DayOfWeek.ToString().Substring(0, 3) + ", " + today.AddDays(2).ToString("MMMM") + " " + today.AddDays(2).Day + " " + today.AddDays(2).Year;
            panel4_title.Text = today.AddDays(3).DayOfWeek.ToString().Substring(0, 3) + ", " + today.AddDays(3).ToString("MMMM") + " " + today.AddDays(3).Day + " " + today.AddDays(3).Year;
            panel5_title.Text = today.AddDays(4).DayOfWeek.ToString().Substring(0, 3) + ", " + today.AddDays(4).ToString("MMMM") + " " + today.AddDays(4).Day + " " + today.AddDays(4).Year;
            panel6_title.Text = today.AddDays(5).DayOfWeek.ToString().Substring(0, 3) + ", " + today.AddDays(5).ToString("MMMM") + " " + today.AddDays(5).Day + " " + today.AddDays(5).Year;
            panel7_title.Text = today.AddDays(6).DayOfWeek.ToString().Substring(0, 3) + ", " + today.AddDays(6).ToString("MMMM") + " " + today.AddDays(6).Day + " " + today.AddDays(6).Year;
            panel8_title.Text = today.AddDays(7).DayOfWeek.ToString().Substring(0, 3) + ", " + today.AddDays(7).ToString("MMMM") + " " + today.AddDays(7).Day + " " + today.AddDays(7).Year;
            panel9_title.Text = today.AddDays(8).DayOfWeek.ToString().Substring(0, 3) + ", " + today.AddDays(8).ToString("MMMM") + " " + today.AddDays(8).Day + " " + today.AddDays(8).Year;
            panel10_title.Text = today.AddDays(9).DayOfWeek.ToString().Substring(0, 3) + ", " + today.AddDays(9).ToString("MMMM") + " " + today.AddDays(9).Day + " " + today.AddDays(9).Year;
            panel11_title.Text = today.AddDays(10).DayOfWeek.ToString().Substring(0, 3) + ", " + today.AddDays(10).ToString("MMMM") + " " + today.AddDays(10).Day + " " + today.AddDays(10).Year;
            panel12_title.Text = today.AddDays(11).DayOfWeek.ToString().Substring(0, 3) + ", " + today.AddDays(11).ToString("MMMM") + " " + today.AddDays(11).Day + " " + today.AddDays(11).Year;
            panel13_title.Text = today.AddDays(12).DayOfWeek.ToString().Substring(0, 3) + ", " + today.AddDays(12).ToString("MMMM") + " " + today.AddDays(12).Day + " " + today.AddDays(12).Year;
            panel14_title.Text = today.AddDays(13).DayOfWeek.ToString().Substring(0, 3) + ", " + today.AddDays(13).ToString("MMMM") + " " + today.AddDays(13).Day + " " + today.AddDays(13).Year;
        }

        private void InitPanelWithDate()
        {
            panelToDate.Add("panel1", DateTime.Today.ToString());
            panelToDate.Add("panel2", DateTime.Today.AddDays(1).ToString());
            panelToDate.Add("panel3", DateTime.Today.AddDays(2).ToString());
            panelToDate.Add("panel4", DateTime.Today.AddDays(3).ToString());
            panelToDate.Add("panel5", DateTime.Today.AddDays(4).ToString());
            panelToDate.Add("panel6", DateTime.Today.AddDays(5).ToString());
            panelToDate.Add("panel7", DateTime.Today.AddDays(6).ToString());
            panelToDate.Add("panel8", DateTime.Today.AddDays(7).ToString());
            panelToDate.Add("panel9", DateTime.Today.AddDays(8).ToString());
            panelToDate.Add("panel10", DateTime.Today.AddDays(9).ToString());
            panelToDate.Add("panel11", DateTime.Today.AddDays(10).ToString());
            panelToDate.Add("panel12", DateTime.Today.AddDays(11).ToString());
            panelToDate.Add("panel13", DateTime.Today.AddDays(12).ToString());
            panelToDate.Add("panel14", DateTime.Today.AddDays(13).ToString());
        }

        private void InitData()
        {
            try
            {
                int lastPanel = 1;
                int lastHour = 1;

                //Clear previous data
                for (int i = 1; i < 15; i++)
                {
                    for (int k = 1; k < 8; k++)
                    {
                        Control[] myControl = this.Controls.Find("panel" + i + "_hour" + k, true);
                        myControl[0].Text = "";
                    }
                }

                if (currentEmployeeId == 0 && cmbEmployee.SelectedIndex < 1) return;
                //if (cmbEmployee.SelectedIndex < 1) return;

                string select = "SELECT * FROM Capacity WHERE Employee = " + (currentEmployeeId == 0 ? ((InNumbers.Common.ComboboxItem)cmbEmployee.SelectedItem).Value : currentEmployeeId) +
                                                                     " AND DayOff >= Date() ORDER BY DayOff";
                DataTable daysOffDT = Common.DataReturn("SELECT * FROM Capacity WHERE Employee = " + (currentEmployeeId == 0 ? ((InNumbers.Common.ComboboxItem)cmbEmployee.SelectedItem).Value : currentEmployeeId) +
                                                                     " AND DayOff >= Date() ORDER BY DayOff");

                //Check panels for Days off
                Dictionary<string, string> panelToDateUpdated = new Dictionary<string, string>();
                foreach (DataRow row in daysOffDT.Rows)
                {
                    foreach (KeyValuePair<string, string> entry in panelToDate)
                    {
                        if (DateTime.Compare(Convert.ToDateTime(entry.Value), Convert.ToDateTime(row["DayOff"])) == 0)
                        {
                            panelToDateUpdated[entry.Key] = entry.Value + ":Off";
                        }
                        else
                        {
                            if (!panelToDateUpdated.ContainsKey(entry.Key))
                            {
                                panelToDateUpdated[entry.Key] = entry.Value;
                            }
                        }
                    }
                }

                //Check if any tasks
                DataRowCollection itemRows = Common.DataReturn("SELECT Client, HrsBudgeted, HoursToCompletion, WIPHours, ScheduleDate FROM MasterTasks WHERE isClosed = False AND employee = '" + (currentEmployeeId == 0 ? ((Common.ComboboxItem)cmbEmployee.SelectedItem).Value : currentEmployeeId) + "'  ORDER BY ScheduleDate").Rows;
                //Any tasks
                if (itemRows.Count > 0)
                {
                    foreach (DataRow itemRow in itemRows)//Common.DataReturn("SELECT Client, HrsBudgeted, HoursToCompletion, WIPHours, ScheduleDate FROM MasterTasks WHERE isClosed = false AND employee = '" + (currentEmployeeId == 0 ? ((InNumbers.Common.ComboboxItem)cmbEmployee.SelectedItem).Value : currentEmployeeId) + "'  ORDER BY ScheduleDate").Rows)
                    {
                        if (lastPanel < 15)
                        {

                            int hoursToShow = Convert.ToInt32(itemRow["HrsBudgeted"]) - Convert.ToInt32(itemRow["WIPHours"] == DBNull.Value ? "0" : itemRow["WIPHours"]);

                            if (hoursToShow <= 0 && Convert.ToInt32(itemRow["HoursToCompletion"] == DBNull.Value ? "0" : itemRow["HoursToCompletion"]) == 0)
                                hoursToShow = 0;

                            bool skippedPanel = false;
                            for (int i = 0; i < hoursToShow; i++)
                            {
                                //if (skippedPanel)
                                //{
                                //    i--;
                                //}

                                if (panelToDateUpdated.Count > 0 && panelToDateUpdated["panel" + lastPanel].Split(' ')[2].Split(':').Length > 1 && panelToDateUpdated["panel" + lastPanel].Split(' ')[2].Split(':')[1] == "Off")
                                {
                                    for (int k = 1; k < 8; k++)
                                    {
                                        Control[] myControlDayOff = this.Controls.Find("panel" + lastPanel + "_hour" + k, true);
                                        myControlDayOff[0].Text = "Day Off";
                                    }
                                    lastPanel++;
                                    skippedPanel = true;
                                    i--;
                                }
                                else
                                {
                                    skippedPanel = false;
                                }


                                if (!skippedPanel)
                                {
                                    if (lastHour == 8)
                                    {
                                        lastHour = 1;
                                        lastPanel++;

                                        if (panelToDateUpdated.Count > 0 && panelToDateUpdated["panel" + lastPanel].Split(' ')[2].Split(':').Length > 1 && panelToDateUpdated["panel" + lastPanel].Split(' ')[2].Split(':')[1] == "Off")
                                        {
                                            for (int l = 1; l < 8; l++)
                                            {
                                                Control[] myControlDayOff = this.Controls.Find("panel" + lastPanel + "_hour" + l, true);
                                                myControlDayOff[0].Text = "Day Off";
                                            }

                                            lastPanel++;
                                            skippedPanel = true;
                                            i--;

                                        }
                                    }

                                    //Check if ScheduleDate > Panel Date 
                                    DateTime scheduleDate = DateTime.Parse(itemRow["ScheduleDate"].ToString().Split(' ')[0]);
                                    DateTime currentPanelDate = DateTime.Parse(panelToDate["panel" + lastPanel].Split(' ')[0]);
                                    int result = DateTime.Compare(currentPanelDate, scheduleDate);

                                    if (result < 0)
                                    {
                                        lastPanel++;
                                        skippedPanel = true;
                                        i--;
                                    }

                                    if (!skippedPanel)
                                    {
                                        Control[] myControl = this.Controls.Find("panel" + lastPanel + "_hour" + lastHour, true);

                                        if (myControl.Length > 0)
                                        {
                                            myControl[0].Text = itemRow["Client"].ToString();
                                            lastHour++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //No tasks, just put days off   
                else
                {
                    while (lastPanel < 15)
                    {
                        if (panelToDateUpdated.Count > 0 && panelToDateUpdated["panel" + lastPanel].Split(' ')[2].Split(':').Length > 1 && panelToDateUpdated["panel" + lastPanel].Split(' ')[2].Split(':')[1] == "Off")
                        {
                            for (int k = 1; k < 8; k++)
                            {
                                Control[] myControlDayOff = this.Controls.Find("panel" + lastPanel + "_hour" + k, true);
                                myControlDayOff[0].Text = "Day Off";
                            }
                            //lastPanel++;
                        }
                        lastPanel++;
                    }
                }
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
            }
        }

        private void CmbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitData();
        }

        private void BtnCancelTask_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

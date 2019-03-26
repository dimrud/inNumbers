namespace InNumbers
{
    partial class ManageUserCapacity
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cmbEmployee = new System.Windows.Forms.ComboBox();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.dgwDaysOff = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDeleteCapacity = new System.Windows.Forms.Button();
            this.btnAddCapacity = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgwDaysOff)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Employee:";
            // 
            // cmbEmployee
            // 
            this.cmbEmployee.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEmployee.FormattingEnabled = true;
            this.cmbEmployee.Location = new System.Drawing.Point(147, 39);
            this.cmbEmployee.Name = "cmbEmployee";
            this.cmbEmployee.Size = new System.Drawing.Size(448, 33);
            this.cmbEmployee.TabIndex = 1;
            this.cmbEmployee.SelectedIndexChanged += new System.EventHandler(this.CmbEmployee_SelectedIndexChanged);
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(764, 97);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 2;
            // 
            // dgwDaysOff
            // 
            this.dgwDaysOff.AllowUserToAddRows = false;
            this.dgwDaysOff.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwDaysOff.Location = new System.Drawing.Point(30, 139);
            this.dgwDaysOff.Name = "dgwDaysOff";
            this.dgwDaysOff.ReadOnly = true;
            this.dgwDaysOff.RowTemplate.Height = 28;
            this.dgwDaysOff.Size = new System.Drawing.Size(565, 226);
            this.dgwDaysOff.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(220, 29);
            this.label2.TabIndex = 4;
            this.label2.Text = "Non working dates:";
            // 
            // btnDeleteCapacity
            // 
            this.btnDeleteCapacity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteCapacity.Location = new System.Drawing.Point(30, 383);
            this.btnDeleteCapacity.Name = "btnDeleteCapacity";
            this.btnDeleteCapacity.Size = new System.Drawing.Size(130, 40);
            this.btnDeleteCapacity.TabIndex = 5;
            this.btnDeleteCapacity.Text = "Delete Date";
            this.btnDeleteCapacity.UseVisualStyleBackColor = true;
            this.btnDeleteCapacity.Click += new System.EventHandler(this.BtnDeleteCapacity_Click);
            // 
            // btnAddCapacity
            // 
            this.btnAddCapacity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddCapacity.Location = new System.Drawing.Point(764, 382);
            this.btnAddCapacity.Name = "btnAddCapacity";
            this.btnAddCapacity.Size = new System.Drawing.Size(130, 40);
            this.btnAddCapacity.TabIndex = 6;
            this.btnAddCapacity.Text = "Select Date";
            this.btnAddCapacity.UseVisualStyleBackColor = true;
            this.btnAddCapacity.Click += new System.EventHandler(this.BtnAddCapacity_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(760, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(289, 29);
            this.label3.TabIndex = 7;
            this.label3.Text = "Select non working dates:";
            // 
            // ManageUserCapacity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 459);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnAddCapacity);
            this.Controls.Add(this.btnDeleteCapacity);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgwDaysOff);
            this.Controls.Add(this.monthCalendar1);
            this.Controls.Add(this.cmbEmployee);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "ManageUserCapacity";
            this.Text = "Manage User Capacity";
            ((System.ComponentModel.ISupportInitialize)(this.dgwDaysOff)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbEmployee;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.DataGridView dgwDaysOff;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDeleteCapacity;
        private System.Windows.Forms.Button btnAddCapacity;
        private System.Windows.Forms.Label label3;
    }
}
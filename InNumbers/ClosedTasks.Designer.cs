namespace InNumbers
{
    partial class ClosedTasks
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
            this.dgwMasterTasks = new System.Windows.Forms.DataGridView();
            this.btnShow = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgwMasterTasks)).BeginInit();
            this.SuspendLayout();
            // 
            // dgwMasterTasks
            // 
            this.dgwMasterTasks.AllowUserToAddRows = false;
            this.dgwMasterTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgwMasterTasks.Location = new System.Drawing.Point(12, 17);
            this.dgwMasterTasks.Name = "dgwMasterTasks";
            this.dgwMasterTasks.ReadOnly = true;
            this.dgwMasterTasks.RowTemplate.Height = 28;
            this.dgwMasterTasks.Size = new System.Drawing.Size(1604, 616);
            this.dgwMasterTasks.TabIndex = 1;
            // 
            // btnShow
            // 
            this.btnShow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShow.Location = new System.Drawing.Point(12, 645);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(130, 40);
            this.btnShow.TabIndex = 2;
            this.btnShow.Text = "Show Task";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // ClosedTasks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1632, 694);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.dgwMasterTasks);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClosedTasks";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.Text = "Closed Tasks";
            ((System.ComponentModel.ISupportInitialize)(this.dgwMasterTasks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgwMasterTasks;
        private System.Windows.Forms.Button btnShow;
    }
}
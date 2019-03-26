namespace InNumbers
{
    partial class ManageUserSelector
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
            this.btnUsers = new System.Windows.Forms.Button();
            this.btnCapacity = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnUsers
            // 
            this.btnUsers.Location = new System.Drawing.Point(115, 53);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Size = new System.Drawing.Size(160, 35);
            this.btnUsers.TabIndex = 0;
            this.btnUsers.Text = "Manage Users";
            this.btnUsers.UseVisualStyleBackColor = true;
            // 
            // btnCapacity
            // 
            this.btnCapacity.Location = new System.Drawing.Point(327, 53);
            this.btnCapacity.Name = "btnCapacity";
            this.btnCapacity.Size = new System.Drawing.Size(160, 35);
            this.btnCapacity.TabIndex = 1;
            this.btnCapacity.Text = "Manage Capacity";
            this.btnCapacity.UseVisualStyleBackColor = true;
            this.btnCapacity.Click += new System.EventHandler(this.btnCapacity_Click);
            // 
            // ManageUserSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 144);
            this.Controls.Add(this.btnCapacity);
            this.Controls.Add(this.btnUsers);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ManageUserSelector";
            this.Text = "Manage User Selector";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.Button btnCapacity;
    }
}
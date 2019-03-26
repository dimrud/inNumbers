using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InNumbers
{
    public partial class ManageUserSelector : Form
    {
        public ManageUserSelector()
        {
            InitializeComponent();
        }

        private void btnCapacity_Click(object sender, EventArgs e)
        {


            //if (Application.OpenForms[form.Name] == null)
            //{
            //   form.Show();
            //}
            //else
            //{
            //   Application.OpenForms[form.Name].Activate();
            //}
            //Application.OpenForms[frm].Focus();
            this.Close();
            ManageUserCapacity frm = new ManageUserCapacity();
           


            frm.Show();
            //this.Close();
            

        }
    }
}

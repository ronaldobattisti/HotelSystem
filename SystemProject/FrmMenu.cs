using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemProject
{
    public partial class FrmMenu : Form
    {
        public FrmMenu()
        {
            InitializeComponent();
        }

        private void ClientMenu_Click(object sender, EventArgs e)
        {
            FrmRegister frm = new FrmRegister();
            frm.ShowDialog();
        }

        private void FrmMenu_Load(object sender, EventArgs e)
        {

        }
    }
}

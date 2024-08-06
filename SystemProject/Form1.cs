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
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            //I setted the text boxt to enable false in the inicialization
            //when click in new, al the boxes become acessible
            enableLabel();
            //and their content become void
            clearFields();
            //and the cursos goes to Name
            txtName.Focus();
            //and enable all buttons
            enableButton();
            btnNew.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clearFields();
            disableLabel();
            disableButton();
            btnNew.Enabled = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            clearFields();
            disableLabel();
            disableButton();
            btnNew.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clearFields();
            disableButton();
            disableLabel();
            btnNew.Enabled = true;
        }

        //Method to desable buttons don't need signature
        private void disableButton()
        {
            btnNew.Enabled = false;
            btnSave.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void enableButton()
        {
            btnNew.Enabled = true;
            btnSave.Enabled = true;
            btnDelete.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void clearFields()
        {
            txtName.Text = "";
            txtAdress.Text = "";
            txtCpf.Text = "";
            txtPhone.Text = "";
        }

        private void enableLabel()
        {
            txtPhone.Enabled = true;
            txtName.Enabled = true;
            txtCpf.Enabled = true;
            txtAdress.Enabled = true;
        }

        private void disableLabel()
        {
            txtPhone.Enabled = false;
            txtName.Enabled = false;
            txtCpf.Enabled = false;
            txtAdress.Enabled = false;
        }
    }
}

using MySql.Data.MySqlClient;
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
    public partial class FrmLogin : Form
    {
        #region Connect and disconnect from DB
        //Instanciating Connection class
        Connection con = new Connection();
        String sqlText;
        MySqlCommand cmd;
        String id;
        String photo;
        String err;

        string changedPic = "no";

        #endregion

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            object exists = null;
            try
            {
                con.OpenConnection();
                //sqlText = "SELECT count(*) FROM login WHERE name=@name and password=@password";
                sqlText = "SELECT EXISTS(SELECT * FROM login WHERE name=@name and password=@password)";
                cmd = new MySqlCommand(sqlText, con.con);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                exists = cmd.ExecuteScalar();
                if (Convert.ToInt32(exists) > 0)
                {
                    MessageBox.Show("Correct log");
                    FrmMenu frm = new FrmMenu();
                    //this.Hide();
                    frm.ShowDialog();
                    //this.Hide();
                }
                else
                {
                    MessageBox.Show("incorrect user or password");
                }
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }
            finally
            {
                MessageBox.Show(err);
            }        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

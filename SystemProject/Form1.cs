using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemProject
{
    public partial class MainFrm : Form
    {
        #region Connect and disconnect from DB
        //Instanciating Connection class
        Connection con = new Connection();
        String sqlText;
        MySqlCommand cmd;

        String id;

        public MainFrm()
        {
            InitializeComponent();
        }
        #endregion

        private void FormatGrid()
        {
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Name";
            dataGridView1.Columns[2].HeaderText = "Adress";
            dataGridView1.Columns[3].HeaderText = "Cpf";
            dataGridView1.Columns[4].HeaderText = "Phone";

            dataGridView1.Columns[0].Visible = false;
        }

        private void ListGrid()
        {
            //Open connection
            con.OpenConnection();
            //SQL Command to select all data ordering acendently by name
            String sqlText = "SELECT * FROM client ORDER BY NAME ASC";
            //cmd = new MySqlCommand(sqlText, con.con);
            //Command to adapt DS content to the grid
            //MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            //da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            //da.Fill(dt);
            dataGridView1.DataSource = dt;

            MySqlDataAdapter da = new MySqlDataAdapter(sqlText, con.con);
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            con.CloseConnection();

            FormatGrid();
        }

        #region Initialization
        private void MainFrm_Load_1(object sender, EventArgs e)
        {
            ListGrid();
        }
        #endregion

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
            if(txtName.Text.Trim() == "")
            {
                MessageBox.Show("Input the name.");
                txtName.Text = "";
                txtName.Focus();
                return;
            }

            if(txtCpf.Text == "   ,   ,   -" || txtCpf.Text.Length != 14)
            {
                MessageBox.Show("CPF incorrect.");
                txtCpf.Text = "";
                return;
            }

            //Open connection with DataBase
            con.OpenConnection();
            //CRUD
            //Create - Read - Update - Delete
            //sqlText is the string that will be used into MySqlDatabase query, in this case to insert data
            sqlText = "INSERT INTO client (name, adress, cpf, phone) VALUES (@name, @adress, @cpf, @phone)";
            //Instanciate cmd as a new MySqlCommand object, that is responsable for executing commands such SQL query against MySqlDatabase
            //The con.con parameter is the connection instancied into Connection class
            cmd = new MySqlCommand(sqlText, con.con);
            cmd.Parameters.AddWithValue("@name", txtName.Text);
            cmd.Parameters.AddWithValue("@adress", txtAdress.Text);
            cmd.Parameters.AddWithValue("@cpf", txtCpf.Text.Replace(',','.'));
            cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
            cmd.ExecuteNonQuery();
            con.CloseConnection();

            clearFields();
            disableLabel();
            disableButton();
            btnNew.Enabled = true;
            //
            //Update Grid view
            ListGrid();
            //
            MessageBox.Show("Register saved sucessifully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            clearFields();
            disableLabel();
            disableButton();
            btnNew.Enabled = true;
            //Update the Gris visualization
            ListGrid();
            MessageBox.Show("Register deleted sucessifully");
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
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void enableButton()
        {
            btnNew.Enabled = true;
            btnSave.Enabled = true;
            btnUpdate.Enabled = true;
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Input the name.");
                txtName.Text = "";
                txtName.Focus();
                return;
            }

            if (txtCpf.Text == "   ,   ,   -" || txtCpf.Text.Length != 14)
            {
                MessageBox.Show("CPF incorrect.");
                txtCpf.Text = "";
                return;
            }

            con.OpenConnection();
            sqlText = "UPDATE client SET name=@name, adress=@adress, cpf=@cpf, phone=@phone WHERE id=@id";
            cmd = new MySqlCommand(sqlText, con.con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", txtName.Text);
            cmd.Parameters.AddWithValue("@adress", txtAdress.Text);
            cmd.Parameters.AddWithValue("@cpf", txtCpf.Text);
            cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
            //Update grid view
            cmd.ExecuteNonQuery();
            con.CloseConnection();
            clearFields();
            disableLabel();
            disableButton();
            btnNew.Enabled = true;
            //Update Grid view
            ListGrid();

            MessageBox.Show("Register updated sucessifully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            enableButton();
            btnNew.Enabled = false;
            btnSave.Enabled = false;
            enableLabel();

            //Casting to convert DataGridVie to String
            id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            txtName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            txtAdress.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            txtCpf.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            txtPhone.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();

        }
    }
}

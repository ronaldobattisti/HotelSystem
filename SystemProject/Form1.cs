using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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

        String photo;

        string changedPic = "no";

        String oldCpf = "";

        public MainFrm()
        {
            InitializeComponent();
        }
        #endregion

        #region Initialization
        private void MainFrm_Load_1(object sender, EventArgs e)
        {
            clearImage();
            ListGrid();
        }
        #endregion

        #region Buttons action
        private void btnNew_Click(object sender, EventArgs e)
        {
            //I setted the text boxt to enable false in the inicialization
            //when click in new, all the boxes become acessible
            enableLabel();
            //and their content become void
            clearFields();
            //and the cursos goes to Name
            txtName.Focus();
            //and enable all buttons
            enableButton();
            btnNew.Enabled = false;
            btnDelete.Enabled = false;

            clearImage();
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
            sqlText = "INSERT INTO client (name, adress, cpf, phone, photo) VALUES (@name, @adress, @cpf, @phone, @image)";
            //Instanciate cmd as a new MySqlCommand object, that is responsable for executing commands such SQL query against MySqlDatabase
            //The con.con parameter is the connection instancied into Connection class
            cmd = new MySqlCommand(sqlText, con.con);
            cmd.Parameters.AddWithValue("@name", txtName.Text);
            cmd.Parameters.AddWithValue("@adress", txtAdress.Text);
            cmd.Parameters.AddWithValue("@cpf", txtCpf.Text.Replace(',','.'));
            cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
            //Need to create a method "img" to send a image to DB
            cmd.Parameters.AddWithValue("@image", img());

            //check if CPF exists
            //Create a TataTable with all instances fouded identic like the input and then check if is bigger than 0
            MySqlCommand cmdVerify = new MySqlCommand("SELECT * FROM client WHERE cpf=@cpf", con.con);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmdVerify;
            cmdVerify.Parameters.AddWithValue("@cpf", txtCpf.Text.Replace(',', '.'));
            DataTable dt = new DataTable();
            da.Fill(dt);
            if(dt.Rows.Count > 0)
            {
                MessageBox.Show("CPF already registered", "Register", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtCpf.Text = "";
                txtCpf.Focus();
                return;
            }

            cmd.ExecuteNonQuery();
            con.CloseConnection();

            clearFields();
            disableLabel();
            disableButton();
            btnNew.Enabled = true;
            //
            clearImage();
            //Update Grid view
            ListGrid();
            //
            MessageBox.Show("Register saved sucessifully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (txtCpf.Text == "   .   .   -" || txtCpf.Text.Length != 14)
            {
                MessageBox.Show("CPF incorrect.");
                txtCpf.Text = "";
                return;
            }

            con.OpenConnection();
            
            if(changedPic == "yes")
            {
                sqlText = "UPDATE client SET name=@name, adress=@adress, cpf=@cpf, phone=@phone, photo=@photo WHERE id=@id";
                cmd = new MySqlCommand(sqlText, con.con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@adress", txtAdress.Text);
                cmd.Parameters.AddWithValue("@cpf", txtCpf.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@photo", img());
            }
            else if (changedPic == "no")
            {
                sqlText = "UPDATE client SET name=@name, adress=@adress, cpf=@cpf, phone=@phone WHERE id=@id";
                cmd = new MySqlCommand(sqlText, con.con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@adress", txtAdress.Text);
                cmd.Parameters.AddWithValue("@cpf", txtCpf.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
            }

            //check if CPF exists
            //Create a TataTable with all instances fouded identic like the input and then check if is bigger than 0
            if(txtCpf.Text != oldCpf)
            {
                MySqlCommand cmdVerify = new MySqlCommand("SELECT * FROM client WHERE cpf=@cpf", con.con);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmdVerify;
                cmdVerify.Parameters.AddWithValue("@cpf", txtCpf.Text.Replace(',', '.'));
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("CPF already registered", "Register", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtCpf.Text = "";
                    txtCpf.Focus();
                    return;
                }
            }

            //Update grid view
            cmd.ExecuteNonQuery();
            con.CloseConnection();
            clearFields();
            disableLabel();
            disableButton();
            btnNew.Enabled = true;

            //Update Grid view
            ListGrid();

            clearImage();

            MessageBox.Show("Register updated sucessifully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Var is used to get int the format of the input
            var ans = MessageBox.Show("You really want to delete this register?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ans == DialogResult.Yes)
            {
                con.OpenConnection();
                sqlText = "DELETE FROM client WHERE id=@id";
                cmd = new MySqlCommand(sqlText, con.con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                con.CloseConnection();

                clearFields();
                disableLabel();
                disableButton();
                btnNew.Enabled = true;

                ListGrid();
                MessageBox.Show("Register deleted Sucessifully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clearFields();
            disableButton();
            disableLabel();
            btnNew.Enabled = true;
            changedPic = "no";
        }
        #endregion

        #region Enable/disable/clear region
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
            txtSearch.Text = "";
        }

        private void enableLabel()
        {
            txtPhone.Enabled = true;
            txtName.Enabled = true;
            txtCpf.Enabled = true;
            txtAdress.Enabled = true;
            txtSearch.Enabled = true;
        }

        private void disableLabel()
        {
            txtPhone.Enabled = false;
            txtName.Enabled = false;
            txtCpf.Enabled = false;
            txtAdress.Enabled = false;
            txtSearch.Enabled = false;
        }
        #endregion

        #region Grid region
        private void FormatGrid()
        {
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Name";
            dataGridView1.Columns[2].HeaderText = "Adress";
            dataGridView1.Columns[3].HeaderText = "Cpf";
            dataGridView1.Columns[4].HeaderText = "Phone";
            dataGridView1.Columns[5].HeaderText = "Photo";

            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[5].Visible = false;
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

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                clearImage();

                changedPic = "no";

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

                oldCpf = dataGridView1.CurrentRow.Cells[3].Value.ToString();

                //get photo - if the sixth value (image) is DBNull
                if (dataGridView1.CurrentRow.Cells[5].Value != DBNull.Value)
                {
                    byte[] img = (byte[])dataGridView1.Rows[e.RowIndex].Cells[5].Value;
                    MemoryStream ms = new MemoryStream(img);

                    image.Image = System.Drawing.Image.FromStream(ms);

                }
                else
                {
                    image.Image = Properties.Resources.profile;
                }
            }
            else 
            {
                return;
            }

            

        }
        #endregion

        private void NameSearch()
        {
            con.OpenConnection();
            sqlText = "SELECT * FROM client WHERE name LIKE @name ORDER BY name asc";
            cmd = new MySqlCommand(sqlText, con.con);
            //This '%' is a Like operator, that searches by aproximation
            cmd.Parameters.AddWithValue("@name", "%" + txtSearch.Text + "%");

            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            //While MySqlAdapter is a object used to retrieve and adapt the DB content,
            //the command fill send these content to a dataTable adapted to the .net environment
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.CloseConnection();

            FormatGrid();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            NameSearch();
        }

        private void btnImg_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Image(*.jpg; *.png) | *.jpg; *.png";
            if (of.ShowDialog() == DialogResult.OK)
            {
                //Get the file path
                photo = of.FileName.ToString();
                image.ImageLocation = photo;

                changedPic = "yes";
            }
            else
            {
                changedPic = "no";
            }
        }

        private byte[] img()
        {
            byte[] image_byte = null;

            if (photo == "") 
            {
                return null;
            }

            FileStream fs = new FileStream(photo, FileMode.Open, FileAccess.Read);

            BinaryReader br = new BinaryReader(fs);

            image_byte = br.ReadBytes((int)fs.Length);

            return image_byte;
        }

        private void clearImage()
        {
            image.Image = Properties.Resources.profile;
            photo = "ft/profile.png";
        }
    }
}

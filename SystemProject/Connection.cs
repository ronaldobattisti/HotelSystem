using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemProject
{
    internal class Connection
    {
        //Connection parameters to acess local server
        public string connec = "SERVER=localhost; DATABASE=class; UID=root; PwD=; PORT=;";

        public MySqlConnection con = null;

        //Open connection
        public void OpenConnection()
        {
            //Test
            try
            {
                con = new MySqlConnection(connec);
                con.Open();
            }
            catch (Exception er)
            {
                //error
                MessageBox.Show("Server Error: " + er.Message);
            }
        }

        public void CloseConnection() 
        {
            try
            {
                con = new MySqlConnection (connec);
                con.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show("Server error: " + er.Message);
            }
        }
        //Close connection
    }
}

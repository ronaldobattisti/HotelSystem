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
        //Estava verificando que o usuário que consta no erro está configurando dentro do banco de dados, é necessário depurar seu banco e buscar o local onde este usuário está configurado e alterar para os dados que adicionou no cPanel e no seu código. Por nosso suporte ser limitado apenas ao ambiente de hospedagem não conseguiramos lhe auxiliar nesta ação, por isso indicamos a depurar ou caso não possua os conhecimentos, o apoio de um profissional em desenvolvimento
        public string connec = "SERVER=ns905.hostgator.com.br; DATABASE=ronal657_hotelSystem; UID=ronal657_rbattisti; PWD=Ronaldo@02; PORT=3306;";

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

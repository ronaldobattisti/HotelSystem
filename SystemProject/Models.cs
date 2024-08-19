using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml;

namespace SystemProject
{
    internal class Person
    {
        //By convention, private components starts with _
        private string _name;
        private string _cpf;
        private string _sqlText;
        MySqlCommand _cmd;
        public int Id {  get; }
        //check if name is valid, ie if it is not null
        public string Name 
        { 
            //Expression-Bodied Member, sintaxe used instead of get { return _cpf; } or set { _cpf = cpf; }
            get => _name;
            set
            {
                //value is setted 
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name shouldn't be empty!");
                }
                _name = value;
            } 
        }
        public string Adress { get; set; }
        //Check if CPF is valid, ie if it have numbers and if it is separated by . not ,
        public string Cpf
        {
            get => _cpf;
            set
            {
                if (value.Length != 14)
                {
                    throw new ArgumentException("Invalid CPF number");
                }
                if (value.Trim(',').Length > 0)
                {
                    value = value.Replace(',', '.');
                }
                if (AlreadyRegistered(value) == true)
                {
                    throw new ArgumentException("This CPF is already in use!");
                }
                _cpf = value;
            }
        }

        public string Phone {  get; set; }
        public byte[] Photo { get; set; }
        public Person(string name, string adress, string cpf, string phone, byte[] photo) 
        {
            Name = name;
            Adress = adress;
            Cpf = cpf;
            Phone = phone;
            Photo = photo;
        }

        private bool AlreadyRegistered(string value)
        {
            Connection con = new Connection();
            con.OpenConnection();
            MySqlCommand cmdVerify = new MySqlCommand("SELECT * FROM client WHERE cpf=@cpf", con.con);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmdVerify;
            cmdVerify.Parameters.AddWithValue("@cpf", value);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.CloseConnection();
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}

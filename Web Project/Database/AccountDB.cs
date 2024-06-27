using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Web_Project.Models;

namespace Web_Project.Database
{
    public class AccountDB
    {

        string connectionString = "Data Source=HASANPC;Initial Catalog=MITREATTACK;Integrated Security=True ";
        internal Account FindByUser(Account account)
        {
            

            string queryString = "SELECT * FROM USERS WHERE USER_MAIL = @Mail AND USERPASS = @Password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                command.Parameters.Add("@Mail", System.Data.SqlDbType.VarChar, 50).Value = account.mail;
                command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 50).Value = account.password;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        account.name = reader["USERNAME"].ToString();
                        account.surname = reader["USERSURNAME"].ToString();
                        account.ID = reader["USER_ID"].ToString();
                        reader.Close();
                    }

                    connection.Close();
                }

                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }

            return account;
        }
        internal bool AddUser(Account account)
        {
            string queryString = "INSERT INTO USERS VALUES (@ID,@Mail,@Username,@Surname,@Password)";
            string getIDCount = "SELECT COUNT(USER_ID) FROM USERS";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand getId = new SqlCommand(getIDCount, connection);
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    int ID = (int)getId.ExecuteScalar();

                    command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = ID;
                    command.Parameters.Add("@Mail", System.Data.SqlDbType.VarChar, 50).Value = account.mail;
                    command.Parameters.Add("@Username", System.Data.SqlDbType.VarChar, 50).Value = account.name;
                    command.Parameters.Add("@Surname", System.Data.SqlDbType.VarChar, 50).Value = account.surname;
                    command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 50).Value = account.password;

                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }

                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    return false;
                }
            }

        }
        

        internal void update(Account account)
        {
            string update = "UPDATE USERS SET USERNAME = @username, USERSURNAME = @surname, USER_MAIL = @mail, USERPASS = @password WHERE USER_ID = @ID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(update, connection);
                command.Parameters.Add("@mail", System.Data.SqlDbType.VarChar, 50).Value = account.mail;
                command.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 50).Value = account.name;
                command.Parameters.Add("@surname", System.Data.SqlDbType.VarChar, 50).Value = account.surname;
                command.Parameters.Add("@password", System.Data.SqlDbType.VarChar, 50).Value = account.password;
                command.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = Int16.Parse(account.ID);
                command.ExecuteNonQuery();
                connection.Close();
            }

        }
    }
}

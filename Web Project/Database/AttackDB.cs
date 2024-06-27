using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Web_Project.Models;

namespace Web_Project.Database 
{
    public class AttackDB
    {
        static string userName = Environment.UserName;
        string connectionString = "Data Source=HASANPC;Initial Catalog=MITREATTACK;Integrated Security=True ";
        public Attack getAttack(string attackName)
        {
            Attack attack = new Attack();
            string getAttackData = "SELECT * FROM TESTS WHERE TEST_ID = @testID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(getAttackData, connection);
                command.Parameters.Add("@testID", System.Data.SqlDbType.VarChar, 50).Value = attackName;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    attack.ID = attackName;
                    attack.test_exp = reader["TEST_EXPLAINATION"].ToString();
                    attack.test_loc = reader["TEST_LOC"].ToString();
                }
                reader.Close();
                connection.Close();
            }
            return attack;
        }

        public void insertTest(Attack attack, string result,string UserID)
        {
            

            string insertString = "INSERT INTO USED_TESTS VALUES (@testID,@userID,@test_desc,@test_device,@test_res)";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    SqlCommand command = new SqlCommand(insertString, connection);
                    command.Parameters.Add("@testID", System.Data.SqlDbType.VarChar, 50).Value = attack.ID;
                    command.Parameters.Add("@userID", System.Data.SqlDbType.VarChar, 50).Value = UserID;
                    command.Parameters.Add("@test_desc", System.Data.SqlDbType.VarChar, 100).Value = attack.test_exp;
                    command.Parameters.Add("@test_device", System.Data.SqlDbType.VarChar, 50).Value = userName;
                    command.Parameters.Add("@test_res", System.Data.SqlDbType.VarChar, 8000).Value = result;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Database "+e.Message);
            }
        }

        public Attack getAttackResult(string ID,string UserID)
        {
            Attack attack = new Attack();

            string insertString = "SELECT * FROM USED_TESTS WHERE USER_ID = @userID AND TEST_ID = @testID";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    SqlCommand command = new SqlCommand(insertString, connection);
                    command.Parameters.Add("@testID", System.Data.SqlDbType.VarChar, 50).Value = ID;
                    command.Parameters.Add("@userID", System.Data.SqlDbType.VarChar, 50).Value = UserID;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        attack.ID = ID;
                        attack.test_exp = reader["TEST_DESC"].ToString();
                        attack.test_result = reader["TEST_RES"].ToString();
                    }
                    connection.Close();
                }
                return attack;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Hata" + e.Message);
                return attack;
            }
        }
    }
}

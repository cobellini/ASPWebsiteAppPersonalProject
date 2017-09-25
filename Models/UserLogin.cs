using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Data;


namespace ComicWebsite.Models
{
    public class UserLogin
    {
        
        private const string userCheckQuery = "SELECT * FROM users WHERE Email = @email AND Password = @password";
        private const string getSaltCode = "SELECT salt FROM users WHERE Email = @email";

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


        public string saltCode;
        public string hashedPassword;




        public string GetSalt()
        {
            var myDbConnection = new ServerAccess();
            using (myDbConnection.DBConnect)
            {
                using (MySqlCommand getSalt = new MySqlCommand(getSaltCode, myDbConnection.DBConnect))
                {
                    getSalt.Parameters.AddWithValue("@email", Email);
                    myDbConnection.DBConnect.Open();

                    MySqlDataReader reader = getSalt.ExecuteReader();
                    saltCode = null;
                    if (reader.Read())
                    {
                        saltCode = reader.GetString(0);
                    }
                }
                myDbConnection.DBConnect.Close();
                return saltCode;
            }
        }





        public bool LoginUserCheck()
        {
            var myDbConnection = new ServerAccess();
            PasswordHash checkPassword = new PasswordHash();
         
            hashedPassword = checkPassword.HashPassword(Password, GetSalt());

            using (myDbConnection.DBConnect)
            {

                bool exists;

                using (MySqlCommand checkLogin = new MySqlCommand(userCheckQuery, myDbConnection.DBConnect))
                {
                    checkLogin.Parameters.AddWithValue("@email", Email);
                    checkLogin.Parameters.AddWithValue("@password", hashedPassword);
                    myDbConnection.DBConnect.Open();
                    int TotalRows = 0;
                    TotalRows = Convert.ToInt32(checkLogin.ExecuteScalar());

                    if (TotalRows > 0)
                    {
                        exists = true;
                    }
                    else
                    {
                        exists = false;
                    }

                }

                myDbConnection.DBConnect.Close();
                return exists;
            }
        }
    }
}
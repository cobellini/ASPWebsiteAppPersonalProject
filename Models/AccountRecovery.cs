using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace ComicWebsite.Models
{
    public class AccountRecovery
    {
        [Required(ErrorMessage = "Please enter a valid email")]
        public string givenEmail { get; set; }

        public string newPassword { get; set; }
        public string confirmednewPassword { get; set; }
        public int givenCode { get; set; }
        public int verificationCode { get; set; }
        Random rand;
        private const string EmailCheckString = "SELECT count(*) FROM users WHERE EMAIL = @email";
        private const string passwordCheckString = "SELECT Password From users WHERE Email = @email";
        private const string getSaltCode = "SELECT salt FROM users WHERE Email = @email";
        public string oldPassword;
        public string saltCode;
        


        public string ConvertNewPassword(string input)
        {
            PasswordHash hashpass = new PasswordHash();
            input = hashpass.HashPassword(input, GetSaltCode());
            return input;
        }

        public void generateCode()
        {
            rand = new Random();
            verificationCode = rand.Next(1, 2000000);
        }

        public bool checkEmail()
        {
            var myDbConnection = new ServerAccess();

            using (myDbConnection.DBConnect)
            {

                bool exists;

                using (MySqlCommand checkUserCommand = new MySqlCommand(EmailCheckString, myDbConnection.DBConnect))
                {
                    checkUserCommand.Parameters.AddWithValue("@email", givenEmail);
                    myDbConnection.DBConnect.Open();
                    int TotalRows = 0;
                    TotalRows = Convert.ToInt32(checkUserCommand.ExecuteScalar());

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

        public void ChangePassword()
        {
            var myDbConnection = new ServerAccess();
           

            using (myDbConnection.DBConnect)
            {

                using (MySqlCommand updatePassword = new MySqlCommand("ChangePassword", myDbConnection.DBConnect))
                {
                    myDbConnection.DBConnect.Open();
                    updatePassword.CommandType = CommandType.StoredProcedure;
                    updatePassword.Parameters.AddWithValue("email", givenEmail);
                    updatePassword.Parameters.AddWithValue("password", ConvertNewPassword(newPassword));
                    updatePassword.ExecuteNonQuery();
                }

                myDbConnection.DBConnect.Close();
            }
        }

        public bool ExistingPassword()
        {
            var myDbConnection = new ServerAccess();
           

            using (myDbConnection.DBConnect)
            {

                using (MySqlCommand checkUserPassword = new MySqlCommand(passwordCheckString, myDbConnection.DBConnect))
                {
                    checkUserPassword.Parameters.AddWithValue("@email", givenEmail);
                    myDbConnection.DBConnect.Open();
                    MySqlDataReader reader = checkUserPassword.ExecuteReader();
                    while (reader.Read())
                    {
                        oldPassword = (string)reader["Password"];
                    }
                }


                myDbConnection.DBConnect.Close();

                if (oldPassword == ConvertNewPassword(newPassword))
                {
                    return false;
                }
                else
                {
                    return true;
                }

                
            }
        }

        public string GetSaltCode()
        {
            var myDbConnection = new ServerAccess();
            using (myDbConnection.DBConnect)
            {
                using (MySqlCommand getSalt = new MySqlCommand(getSaltCode, myDbConnection.DBConnect))
                {
                    getSalt.Parameters.AddWithValue("@email", givenEmail);
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
    }
}
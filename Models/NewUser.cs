using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Data;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;

namespace ComicWebsite.Models
{
    public class NewUser
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }


        private const string checkAvailableUsers = "SELECT * FROM users WHERE Email = @email AND Username = @usrname";
        public int generatedCode;
        public int givenCode { get; set; }

        public void generateSignUpCode()
        {
            Random rand = new Random();

            generatedCode = rand.Next(0, 200000);
            
        }


        public bool checkUser()
        {
            var myDbConnection = new ServerAccess();


            using (myDbConnection.DBConnect)
            {
                
                bool exists;
                
                using (MySqlCommand checkUserCommand = new MySqlCommand(checkAvailableUsers, myDbConnection.DBConnect))
                {
                    checkUserCommand.Parameters.AddWithValue("@email", Email);
                    checkUserCommand.Parameters.AddWithValue("@usrname", Username);
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


        public void InsertNewUser()
        {
            var myDbConnection = new ServerAccess();
            PasswordHash passwordHashing = new PasswordHash();
            string salty = passwordHashing.CreateSalt(48);
            string generateHashedPassword = passwordHashing.HashPassword(Password, salty);

            using (myDbConnection.DBConnect)
            {
                myDbConnection.DBConnect.Open();

                using (MySqlCommand newUserCommand = new MySqlCommand("NewUser", myDbConnection.DBConnect))
                {
                    newUserCommand.CommandType = CommandType.StoredProcedure;

                    newUserCommand.Parameters.AddWithValue("email", Email);
                    newUserCommand.Parameters.AddWithValue("password", generateHashedPassword);
                    newUserCommand.Parameters.AddWithValue("username", Username);
                    newUserCommand.Parameters.AddWithValue("Salt", salty);

                    newUserCommand.ExecuteNonQuery();
                }
            }

            myDbConnection.DBConnect.Close();


        }



    }

}

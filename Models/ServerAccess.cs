using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace ComicWebsite.Models
{
    public class ServerAccess
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["SQLConnection"].ToString();
        MySqlConnection connection;

        public MySqlConnection DBConnect
        {
            get { return this.connection; }
        }
        
        public ServerAccess()
        {
            this.connection = new MySqlConnection(connectionString);
        }
    }
}
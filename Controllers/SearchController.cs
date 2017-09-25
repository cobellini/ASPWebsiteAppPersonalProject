using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComicWebsite.Models;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Data;

namespace ComicWebsite.Controllers
{
    public class SearchController : Controller
    {
           
        public ActionResult Results(string inputText)
        {
            var queryString = new Search();
            var model = new List<Search>();
            var myDbConnection = new ServerAccess();
            using (myDbConnection.DBConnect)
            {
                using (MySqlCommand getResults = new MySqlCommand(queryString.searchQuery, myDbConnection.DBConnect))
                {
                    getResults.Parameters.AddWithValue("@input", "%" + inputText + "%");
                    myDbConnection.DBConnect.Open();

                    MySqlDataReader reader = getResults.ExecuteReader();

                    while (reader.Read())
                    {
                        var newTitle = new Search();
                        newTitle.MovieTitle = reader["movieDescription"].ToString();
                        newTitle.youtubeLink = reader["youtubelink"].ToString();
                        newTitle.thumbnail = reader["thumbnail"].ToString();
                        model.Add(newTitle);
                    }
                }
            }
            // var result = search.getResults();
            return View(model);
        }
    }
}
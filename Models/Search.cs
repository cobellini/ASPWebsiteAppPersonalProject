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
    public class Search
    {

        public string searchInput;
        public string searchQuery = "SELECT movieDescription, youtubelink, thumbnail FROM trailerlist WHERE movieName LIKE @input ";
        
        public string MovieTitle { get; set; }
        public string youtubeLink { get; set; }
        public string thumbnail { get; set; }


    }
}
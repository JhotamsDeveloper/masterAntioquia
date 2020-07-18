using System;
using System.Collections.Generic;
using System.Text;
using System.Web.WebPages.Html;

namespace Model.DTOs
{
    public class SearchDto
    {
        public string Name { get; set; }
        public string NameUrl { get; set; }
        public string SquareCover { get; set; }
        public DateTime SearchDate { get; set; }
    }

}

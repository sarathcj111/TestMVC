using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestMVC.Models
{
    public class SearchModel
    {
        public List<BookModel> Books { get; set; }
        public List<CompanyModel> Companies { get; set; }
        public string SearchString { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestMVC.Models;
using TestMVC.Repository.Interface;

namespace TestMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBookRepository _IBookRepository;
        private readonly ICompanyRepository _ICompanyRepository;
        public HomeController(ILogger<HomeController> logger, IBookRepository iBookRepository, ICompanyRepository iCompanyRepository)
        {
            _logger = logger;
            _IBookRepository = iBookRepository;
            _ICompanyRepository = iCompanyRepository;
        }
        public IActionResult Index()
        {
            var books = _IBookRepository.GetAllBooks();
            return View(books);
        }

        public IActionResult EditBook(int bookId)
        {
            var bookList = _IBookRepository.EditBook(bookId);
            return View("Index", bookList);
        }

        public IActionResult DeleteBook(int bookId)
        {
            var isbook = _IBookRepository.DeleteBook(bookId, out List<BookModel> books);
            return View("Index", books);
        }

        public IActionResult OpenAddBookPage()
        {
            var companies = _ICompanyRepository.GetAllCompanys();            
            ViewBag.ListItem = companies;
            return View("AddBook");
        }

        public IActionResult AddBook(BookModel book)
        {
            var books = _IBookRepository.AddNewBook(book);            
            return View("Index", books);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

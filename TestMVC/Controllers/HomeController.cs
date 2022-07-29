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
        public async Task<IActionResult> Index()
        {
            //var books = _IBookRepository.GetAllBooks();
            try
            {
                var books = await _IBookRepository.GetAllBooksFromApi();
                return View(books);
            }
            catch(Exception ex)
            {
                return View("Error",new ErrorViewModel { RequestId = ex.Message ?? HttpContext.TraceIdentifier });
            }
            //return View(books);
        }

        public async Task<IActionResult> OpenEditBookPage(int bookId)
        {
            var bookList = await _IBookRepository.GetAllBooksFromApi();
            var book = bookList.Find(x => x.Id == bookId);
            var companies = _ICompanyRepository.GetAllCompanys();
            ViewBag.ListItem = companies;

            return View("EditBook", book);
        }

        public async Task<IActionResult> EditBook(BookModel book)
        {
            //var bookList = _IBookRepository.EditBook(book);
            //return View("Index", bookList);
            try
            {
                var books = await _IBookRepository.EditBookAsync(book);
                return View("Index", books);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message ?? HttpContext.TraceIdentifier });
            }
        }

        public async Task<IActionResult> DeleteBook(int bookId)
        {
            //var isbook = _IBookRepository.DeleteBook(bookId, out List<BookModel> books);
            //return View("Index", books);
            try
            {
                var books = await _IBookRepository.DeleteBookAsync(bookId);
                return View("Index", books);
            }
            catch (Exception ex)
            {

                return View("Error", new ErrorViewModel { RequestId = ex.Message ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult OpenAddBookPage()
        {
            var companies = _ICompanyRepository.GetAllCompanys();            
            ViewBag.ListItem = companies;
            return View("AddBook");
        }

        public async Task<IActionResult> AddBook(BookModel book)
        {
            //var books = _IBookRepository.AddNewBook(book);            
            //return View("Index", books);
            try
            {
                var isAdded = await _IBookRepository.AddNewBookAsync(book);
                if (isAdded)
                {
                    var books = await _IBookRepository.GetAllBooksFromApi();
                    return View("Index" , books);
                }
                else
                {
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel { RequestId = ex.Message ?? HttpContext.TraceIdentifier });
            }
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

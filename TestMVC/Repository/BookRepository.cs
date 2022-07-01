using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestMVC.Helper;
using TestMVC.Models;
using TestMVC.Repository.Interface;

namespace TestMVC.Repository
{
    public class BookRepository: IBookRepository
    {
        private readonly IConfiguration _config;
        private HelperClass _helper;
        public BookRepository(IConfiguration config)
        {
            _config = config;
            _helper = new HelperClass();
        }
        private List<BookModel> bookList = new List<BookModel>();

        private List<BookModel> GenerateBookList()
        {
            for(var i = 0; i <= 100; i++)
            {
                if (_config.GetValue<int>($"Book{i + 1}:Id") > 0)
                {
                    BookModel book = new BookModel();
                    book.Id = _config.GetValue<int>($"Book{i + 1}:Id");
                    book.Title = _config.GetValue<string>($"Book{i + 1}:Title");
                    book.Genre = _config.GetValue<string>($"Book{i + 1}:Genre");
                    book.Price = _config.GetValue<decimal>($"Book{i + 1}:Price");
                    book.Company = _config.GetValue<string>($"Book{i + 1}:Company");
                    bookList.Add(book);
                }
                else
                    break;
            }
            return bookList;
        }
        public List<BookModel> AddNewBook(BookModel book)
        {
            var books = GenerateBookList();
            book.Id = books.Count + 1;

            var companies = new CompanyRepository(_config);
            var companyName = companies.GetAllCompanys().Find(x => x.Id == Convert.ToInt32(book.Company)).Name;
            book.Company = companyName;

            books.Add(book);
            _helper.AddtoJson(book);
            return books;
        }
        public List<BookModel> GetAllBooks()
        {
            GenerateBookList();
            return bookList;
        }
        public List<BookModel> EditBook(int bookId)
        {
            var books = GenerateBookList();
            var book = books.Find(x => x.Id == bookId);
            book.Title = "edited " + book.Title;
            return books;
        }
        public bool DeleteBook(int bookId,out List<BookModel> books)
        {
            books = GenerateBookList();
            books.Remove(books.Find(x => x.Id == bookId));
            var book = books.Find(x => x.Id == bookId);
            if (book == null)
            {
                _helper.RemoveFromJson(bookId,"Book");
                return true;
            }
            else
                return false;
        }


        // Do not pay attention to this part..
        
    }
}

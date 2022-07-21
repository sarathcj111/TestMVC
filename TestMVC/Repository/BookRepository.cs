using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Net.Http;
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

        public async Task<bool> AddNewBookAsync(BookModel book)
        {
            var books = await GetAllBooksFromApi();
            book.Id = books.Count + 1;

            var companies = new CompanyRepository(_config);
            var companyName = companies.GetAllCompanys().Find(x => x.Id == Convert.ToInt32(book.Company)).Name;
            book.Company = companyName;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44329");

                string json = JsonConvert.SerializeObject(book);

                StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var result = await client.PostAsync("Test/addBook", httpContent);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    throw new Exception("Server error try after some time.");
                }
            }
        }

        public List<BookModel> GetAllBooks()
        {
            GenerateBookList();
            return bookList;
        }
        public async Task<List<BookModel>> GetAllBooksFromApi()
        {
            List<BookModel> books = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44329");

                var result = await client.GetAsync("Test/getAllBooks");

                if (result.IsSuccessStatusCode)
                {
                    books = JsonConvert.DeserializeObject<List<BookModel>>(await result.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new Exception("Server error. try after some time.");
                }
            }
            return books;
        }

        public List<BookModel> EditBook(BookModel book)
        {
            var books = GenerateBookList();
            var objBook = books.Find(x => x.Id == book.Id);
            var companies = new CompanyRepository(_config);
            var companyName = companies.GetAllCompanys().Find(x => x.Id == Convert.ToInt32(book.Id)).Name;
            book.Company = companyName;
            //objBook.Title = book.Title;
            //objBook.Genre = book.Genre;
            //objBook.Price = book.Price;
            //objBook.Company = book.Company;
            books.Remove(books.Find(x => x.Id == book.Id));
            _helper.RemoveFromJson(book.Id, "Book");

            books.Add(book);
            _helper.AddtoJson(book);

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

        public BookModel SearchBook(int bookId)
        {
            var Books = GenerateBookList();
            var Book = Books.Find(x => x.Id == bookId);
            return Book;
        }

        public IEnumerable<BookModel> SearchLikeBook(string book)
        {
            var Books = GenerateBookList();
            var Book = Books.Where(s => s.Title.Contains(book));
            return Book;
        }

        public IEnumerable<BookModel> SearchLikePatternBook(string pattern)
        {
            var Books = GenerateBookList();
            var Book = Books.Where(s => FileSystemName.MatchesSimpleExpression(pattern, s.Title));
            return Book;
        }

        public IEnumerable<BookModel> SearchBookByCompany(string bookCompanyId)
        {
            var companies = new CompanyRepository(_config);
            var companyName = companies.GetAllCompanys().Find(x => x.Id == Convert.ToInt32(bookCompanyId)).Name;
            var books = GenerateBookList();
            var bookList = books.Where(s => s.Company == companyName);
            return bookList;
        }


    }
}

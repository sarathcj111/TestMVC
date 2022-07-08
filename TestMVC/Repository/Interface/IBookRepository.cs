using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestMVC.Models;

namespace TestMVC.Repository.Interface
{
    public interface IBookRepository
    {
        List<BookModel> AddNewBook(BookModel bookModel);
        List<BookModel> GetAllBooks();
        List<BookModel> EditBook(BookModel bookModel);
        bool DeleteBook(int bookId, out List<BookModel> books);
    }
}

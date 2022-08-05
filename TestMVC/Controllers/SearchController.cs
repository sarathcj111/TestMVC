using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using TestMVC.ActionFilter;
using TestMVC.Models;
using TestMVC.Repository.Interface;

namespace TestMVC.Controllers
{
    [CustomFiter()]
    public class SearchController : Controller
    {
        private readonly ICompanyRepository _ICompanyRepository;
        private readonly IBookRepository _IBookRepository;

        public SearchController(ICompanyRepository iCompanyRepository, IBookRepository iBookRepository)
        {
            _ICompanyRepository = iCompanyRepository;
            _IBookRepository = iBookRepository;
        }

        public IActionResult Index()
        {
            var objSearchModel = new SearchModel();
            objSearchModel.Books = _IBookRepository.GetAllBooks();
            objSearchModel.Companies = _ICompanyRepository.GetAllCompanys();
            ViewBag.SearchListItem = objSearchModel.Companies;
            return View(objSearchModel);
        }

        public IActionResult SearchByCompanyId(string searchString)
        {
            if(searchString == null)
            {
                TempData["msg"] = "<script>alert('No company selected');</script>";
                return RedirectToAction("Index");
            }
            var objSearchModel = new SearchModel();
            objSearchModel.SearchString = null;
            var books = _IBookRepository.SearchBookByCompany(searchString);
            var company = _ICompanyRepository.SearchCompany(Convert.ToInt32(searchString));
            //objSearchModel.Books = new List<BookModel>();
            objSearchModel.Books = books.ToList();
            objSearchModel.Companies = new List<CompanyModel>();
            objSearchModel.Companies.Add(company);
            ViewBag.SearchListItem = _ICompanyRepository.GetAllCompanys();
            return View("Index",objSearchModel);
        }
        public IActionResult SearchByBookId(string searchString)
        {
            if (searchString == null)
            {
                TempData["msg"] = "<script>alert('No search Input');</script>";
                return RedirectToAction("Index");
            }
            var objSearchModel = new SearchModel();
            objSearchModel.SearchString = null;
            var books = _IBookRepository.GetAllBooks().Where(x => x.Id == Convert.ToInt32(searchString));
            var companies = _ICompanyRepository.GetAllCompanys().Where(z => z.Name == (books.ToList().Count > 0 ? books.ToList()[0].Company : null));
            //objSearchModel.Books = new List<BookModel>();
            objSearchModel.Books = books.ToList();
            objSearchModel.Companies = new List<CompanyModel>();
            objSearchModel.Companies = companies.ToList();
            if(objSearchModel.Books.Count == 0 && objSearchModel.Companies.Count == 0)
            {
                TempData["msg"] = "<script>alert('No search Data Found');</script>";
                return RedirectToAction("Index");
            }
            ViewBag.SearchListItem = _ICompanyRepository.GetAllCompanys();
            return View("Index",objSearchModel);
        }

        public IActionResult SearchByCompanyPattern(string searchString)
        {
            if (searchString == null)
            {
                TempData["msg"] = "<script>alert('No search Input');</script>";
                return RedirectToAction("Index");
            }
            var objSearchModel = new SearchModel();
            objSearchModel.SearchString = null;
            var books = _IBookRepository.GetAllBooks().Where(s => FileSystemName.MatchesSimpleExpression($"*{searchString}*", s.Company));
            var companies = _ICompanyRepository.GetAllCompanys().Where(s => FileSystemName.MatchesSimpleExpression($"*{searchString}*", s.Name));
            //objSearchModel.Books = new List<BookModel>();
            objSearchModel.Books = books.ToList();
            objSearchModel.Companies = new List<CompanyModel>();
            objSearchModel.Companies = companies.ToList();
            if (objSearchModel.Books.Count == 0 && objSearchModel.Companies.Count == 0)
            {
                TempData["msg"] = "<script>alert('No search Data Found');</script>";
                return RedirectToAction("Index");
            }
            ViewBag.SearchListItem = _ICompanyRepository.GetAllCompanys();
            return View("Index",objSearchModel);
        }

    }
}

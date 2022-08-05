using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TestMVC.ActionFilter;
using TestMVC.Models;
using TestMVC.Repository.Interface;

namespace TestMVC.Controllers
{
    [CustomFiter()]
    public class CompanyController : Controller
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyRepository _ICompanyRepository;
        public CompanyController(ILogger<CompanyController> logger, ICompanyRepository iCompanyRepository)
        {
            _logger = logger;
            _ICompanyRepository = iCompanyRepository;
        }
        public IActionResult Index()
        {
            var companys = _ICompanyRepository.GetAllCompanys();
            return View(companys);
        }

        public IActionResult EditCompany(CompanyModel company)
        {
            var companyList = _ICompanyRepository.EditCompany(company);
            return View("Index", companyList);
        }

        public IActionResult DeleteCompany(int companyId)
        {
            var isCompany = _ICompanyRepository.DeleteCompany(companyId, out List<CompanyModel> companys);
            return View("Index", companys);
        }

        public IActionResult OpenAddCompanyPage()
        {
            List<SelectListItem> Cities = new List<SelectListItem>()
            {
                new SelectListItem() { Value="Kolkata", Text="Kolkata" },
                new SelectListItem() { Value="Chennai", Text="Chennai" },
                new SelectListItem() { Value="Banglore", Text="Banglore" },
                new SelectListItem() { Value="Delhi", Text="Delhi" }
            };

            ViewBag.CityList = Cities;
            return View("AddCompany");
        }

        public IActionResult AddCompany(CompanyModel objCompany)
        {
            List<CompanyModel> companys = null;
            if (objCompany.Id == 0)
                companys = _ICompanyRepository.AddNewCompany(objCompany);
            else
                companys = _ICompanyRepository.EditCompany(objCompany);
            return View("Index", companys);
        }

        public IActionResult OpenEditCompanyPage(int companyId)
        {
            var company = _ICompanyRepository.SearchCompany(companyId);
            var companyLike = _ICompanyRepository.SearchLikeCompany("com");
            var companyPattern = _ICompanyRepository.SearchLikeCompany("com*");

            List<SelectListItem> Cities = new List<SelectListItem>()
            {
                new SelectListItem() { Value="Kolkata", Text="Kolkata" },
                new SelectListItem() { Value="Chennai", Text="Chennai" },
                new SelectListItem() { Value="Banglore", Text="Banglore" },
                new SelectListItem() { Value="Delhi", Text="Delhi" }
            };

            ViewBag.Cities = Cities;

            return View("EditCompany", company);
        }
    }
}

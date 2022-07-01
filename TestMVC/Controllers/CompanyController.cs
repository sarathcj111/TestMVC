using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestMVC.Models;
using TestMVC.Repository.Interface;

namespace TestMVC.Controllers
{
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

        public IActionResult EditCompany(int companyId)
        {
            var companyList = _ICompanyRepository.EditCompany(companyId);
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

        public IActionResult AddCompany(CompanyModel company)
        {
            var companys = _ICompanyRepository.AddNewCompany(company);
            return View("Index", companys);
        }
    }
}

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Threading.Tasks;
using TestMVC.Helper;
using TestMVC.Models;
using TestMVC.Repository.Interface;

namespace TestMVC.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IConfiguration _config;
        private HelperClass _helper;
        public CompanyRepository(IConfiguration config)
        {
            _config = config;
            _helper = new HelperClass();
        }
        private List<CompanyModel> CompanyList = new List<CompanyModel>();

        private List<CompanyModel> GenerateCompanyList()
        {
            for (var i = 0; i <= 100; i++)
            {
                if(_config.GetValue<int>($"Company{i + 1}:Id") > 0)
                {
                    CompanyModel Company = new CompanyModel();
                    Company.Id = _config.GetValue<int>($"Company{i + 1}:Id");
                    Company.Name = _config.GetValue<string>($"Company{i + 1}:Name");
                    Company.City = _config.GetValue<string>($"Company{i + 1}:City");
                    CompanyList.Add(Company);
                }
                else
                    break;
            }
            return CompanyList;
        }
        public List<CompanyModel> AddNewCompany(CompanyModel Company)
        {
            var Companys = GenerateCompanyList();
            Company.Id = Companys.Count + 1;
            Companys.Add(Company);
            _helper.AddtoJson(Company);
            return Companys;
        }
        public List<CompanyModel> GetAllCompanys()
        {
            GenerateCompanyList();
            return CompanyList;
        }
        //public List<CompanyModel> EditCompany(int CompanyId)
        //{
        //    var Companys = GenerateCompanyList();
        //    var Company = Companys.Find(x => x.Id == CompanyId);
        //    Company.Name = "edited " + Company.Name;
        //    return Companys;
        //}
        public List<CompanyModel> EditCompany(CompanyModel company)
        {
            var Companys = GenerateCompanyList();
            var Company = Companys.Find(x => x.Id == company.Id);
            Companys.Remove(company);
            _helper.RemoveFromJson(Company.Id, "Company");
            Companys.Add(company);
            _helper.AddtoJson(Company);
            return Companys;
        }
        public bool DeleteCompany(int CompanyId, out List<CompanyModel> Companys)
        {
            Companys = GenerateCompanyList();
            Companys.Remove(Companys.Find(x => x.Id == CompanyId));
            var Company = Companys.Find(x => x.Id == CompanyId);
            if (Company == null)
            {
                _helper.RemoveFromJson(CompanyId, "Company");
                return true;
            }
            else
                return false;
        }

        public CompanyModel SearchCompany(int CompanyId)
        {
            var Companys = GenerateCompanyList();
            var Company = Companys.Find(x => x.Id == CompanyId);
            return Company;
        }

        public IEnumerable<CompanyModel> SearchLikeCompany(string company)
        {
            var Companys = GenerateCompanyList();
            var Company = Companys.Where(s => s.Name.Contains(company));
            return Company;
        }

        public IEnumerable<CompanyModel> SearchLikePatternCompany(string pattern)
        {
            var Companys = GenerateCompanyList();
            var Company = Companys.Where(s => FileSystemName.MatchesSimpleExpression(pattern, s.Name));
            return Company;
        }
    }
}

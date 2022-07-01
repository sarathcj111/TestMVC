using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestMVC.Models;

namespace TestMVC.Repository.Interface
{
    public interface ICompanyRepository
    {
        List<CompanyModel> AddNewCompany(CompanyModel CompanyModel);
        List<CompanyModel> GetAllCompanys();
        List<CompanyModel> EditCompany(int CompanyId);
        bool DeleteCompany(int CompanyId, out List<CompanyModel> Companys);
    }
}

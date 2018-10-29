using System;
using System.Collections.Generic;
using System.Text;
using VatCalculator.Models;

namespace VatCalculator.Interfaces
{
    public interface IDataService
    {
        Customer GetCustomerById(int id);
        Provider GetProviderById(int id);
        Country GetCountryById(int id);
    }
}

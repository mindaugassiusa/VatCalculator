﻿using System.Collections.Generic;
using System.Linq;
using VatCalculator.Interfaces;
using VatCalculator.Models;

namespace VatCalculator.Services
{
    public class DataService : IDataService
    {
        public static readonly List<Country> _countries = new List<Country>()
        {
            new Country {Id = 1, Name = "Lithuania", CountryInformation = {IsEuMember = true, Vat = 21}},
            new Country {Id = 2, Name = "USA", CountryInformation = {IsEuMember = false, Vat = 20}},
            new Country {Id = 3, Name = "Germany", CountryInformation = {IsEuMember = false, Vat = 19}},
            new Country {Id = 4, Name = "Latvia", CountryInformation = {IsEuMember = false, Vat = 21}},
        };
        public static readonly List<Customer> _customers = new List<Customer>()
        {
            new Customer{Id = 1, Name = "Maxima", IsCompany = true, IsVatPayer = true, Country = _countries.FirstOrDefault(x => x.Id == 1)},
            new Customer{Id = 2, Name = "Petras Petraitis", IsCompany = false, IsVatPayer = true, Country = _countries.FirstOrDefault(x => x.Id == 4)},
            new Customer{Id = 3, Name = "John Johnson", IsCompany = false, IsVatPayer = false, Country = _countries.FirstOrDefault(x => x.Id == 2)},
            new Customer{Id = 4, Name = "Pro Alpha", IsCompany = true, IsVatPayer = false, Country = _countries.FirstOrDefault(x => x.Id == 3)},
            new Customer{Id = 5, Name = "Tomas Tomaitis", IsCompany = false, IsVatPayer = false, Country = _countries.FirstOrDefault(x => x.Id == 1)},
        };
        public static readonly List<Provider> _providers = new List<Provider>()
        {
            new Provider{Id = 1, Name = "Senukai", IsVatPayer = true, Country = _countries.FirstOrDefault(x => x.Id == 1)},
            new Provider{Id = 2, Name = "Lidl", IsVatPayer = false, Country = _countries.FirstOrDefault(x => x.Id == 1)},
        };

        public Customer GetCustomerById(int id)
        {
            return _customers.FirstOrDefault(x => x.Id == id);
        }

        public Provider GetProviderById(int id)
        {
            return _providers.FirstOrDefault(x => x.Id == id);
        }

        public Country GetCountryById(int id)
        {
            return _countries.FirstOrDefault(x => x.Id == id);
        }
    }
}

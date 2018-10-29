using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using VatCalculator.Interfaces;
using VatCalculator.Models;
using VatCalculator.Services;
using Xunit;

namespace VatCalculatorTests.CalculatorServiceTests
{
    public class CalculateVatTests
    {
        protected readonly CalculatorService _calculatorService;
        protected readonly IDataService _dataService;

        public static readonly List<Country> _mockedCountries = new List<Country>()
        {
            new Country {Id = 1, Name = "Lithuania", Vat = 21, IsEuMember = true},
            new Country {Id = 2, Name = "Great Britain", Vat = 20, IsEuMember = false},
            new Country {Id = 3, Name = "Germany", Vat = 19, IsEuMember = true},
            new Country {Id = 4, Name = "Latvia", Vat = 21, IsEuMember = true},
        };

        public static readonly List<Customer> _mockedCustomers = new List<Customer>()
        {
            new Customer{Id = 1, Name = "Maxima", IsCompany = true, IsVatPayer = true, Country = _mockedCountries.FirstOrDefault(x => x.Id == 1)},
            new Customer{Id = 2, Name = "Petras Petraitis", IsCompany = false, IsVatPayer = true, Country = _mockedCountries.FirstOrDefault(x => x.Id == 4)},
            new Customer{Id = 3, Name = "John Johnson", IsCompany = false, IsVatPayer = false, Country = _mockedCountries.FirstOrDefault(x => x.Id == 2)},
            new Customer{Id = 4, Name = "Pro Alpha", IsCompany = true, IsVatPayer = false, Country = _mockedCountries.FirstOrDefault(x => x.Id == 3)},
            new Customer{Id = 5, Name = "Tomas Tomaitis", IsCompany = false, IsVatPayer = false, Country = _mockedCountries.FirstOrDefault(x => x.Id == 1)},
        };

        public static readonly List<Provider> _mockedProviders = new List<Provider>()
        {
            new Provider{Id = 1, Name = "Senukai", IsVatPayer = true, Country = _mockedCountries.FirstOrDefault(x => x.Id == 1)},
            new Provider{Id = 2, Name = "Lidl", IsVatPayer = false, Country = _mockedCountries.FirstOrDefault(x => x.Id == 1)},
        };

        public CalculateVatTests()
        {
            _dataService = Substitute.For<IDataService>();
            _calculatorService = new CalculatorService(_dataService);
        }

        [Fact]
        public void ShouldReturnZeroWhenProviderIsNotVatPayer()
        {
            var customerId = 1;
            var providerId = 2;
            var amount = 1000;

            _dataService.GetCustomerById(customerId).Returns(_mockedCustomers.FirstOrDefault(x => x.Id == customerId));
            _dataService.GetProviderById(providerId).Returns(_mockedProviders.FirstOrDefault(x => x.Id == providerId));

            var result = _calculatorService.CalculateVat(customerId, providerId, amount);

            result.ShouldBe(0);
        }

        [Fact]
        public void ShouldReturnZeroWhenProviderIsVatPayerAndCustomerCountryIsNotEuMemberAndCustomerAndProviderCountriesAreDifferent()
        {
            var customerId = 3;
            var providerId = 1;
            var amount = 1000;

            _dataService.GetCustomerById(customerId).Returns(_mockedCustomers.FirstOrDefault(x => x.Id == customerId));
            _dataService.GetProviderById(providerId).Returns(_mockedProviders.FirstOrDefault(x => x.Id == providerId));

            var result = _calculatorService.CalculateVat(customerId, providerId, amount);

            result.ShouldBe(0);
        }

        [Fact]
        public void ShouldReturnZeroWhenCustomerCountryIsEuMemberAndCustomerIsVatPayerAndCustomerAndProviderCountriesAreDifferent()
        {
            var customerId = 2;
            var providerId = 1;
            var amount = 1000;

            _dataService.GetCustomerById(customerId).Returns(_mockedCustomers.FirstOrDefault(x => x.Id == customerId));
            _dataService.GetProviderById(providerId).Returns(_mockedProviders.FirstOrDefault(x => x.Id == providerId));

            var result = _calculatorService.CalculateVat(customerId, providerId, amount);

            result.ShouldBe(0);
        }

        [Fact]
        public void ShouldReturnCalculatedVatWhenCustomerCountryIsEuMemberAndCustomerIsNotVatPayerAndCustomerAndProviderCountriesAreDifferent()
        {
            var customerId = 4;
            var providerId = 1;
            var amount = 1000;

            _dataService.GetCustomerById(customerId).Returns(_mockedCustomers.FirstOrDefault(x => x.Id == customerId));
            _dataService.GetProviderById(providerId).Returns(_mockedProviders.FirstOrDefault(x => x.Id == providerId));

            var result = _calculatorService.CalculateVat(customerId, providerId, amount);

            result.ShouldBe(190);
        }

        [Fact]
        public void ShouldCalculateVatWhenCustomerAndProviderCountriesAreTheSame()
        {
            var customerId = 1;
            var providerId = 1;
            var amount = 2000;

            _dataService.GetCustomerById(customerId).Returns(_mockedCustomers.FirstOrDefault(x => x.Id == customerId));
            _dataService.GetProviderById(providerId).Returns(_mockedProviders.FirstOrDefault(x => x.Id == providerId));

            var result = _calculatorService.CalculateVat(customerId, providerId, amount);

            result.ShouldBe(420);
        }

        [Fact]
        public void ShouldThrowExceptionWhenCustomerIsZero()
        {
            var customerId = 0;
            var providerId = 1;
            var amount = 1000;

            _dataService.GetCustomerById(customerId).Returns(_mockedCustomers.FirstOrDefault(x => x.Id == customerId));
            _dataService.GetProviderById(customerId).Returns(_mockedProviders.FirstOrDefault(x => x.Id == customerId));

            Should.Throw<Exception>(() => _calculatorService.CalculateVat(customerId, providerId, amount));
        }

        [Fact]
        public void ShouldThrowExceptionWhenProviderIsZero()
        {
            var customerId = 1;
            var providerId = 0;
            var amount = 1000;

            _dataService.GetCustomerById(customerId).Returns(_mockedCustomers.FirstOrDefault(x => x.Id == customerId));
            _dataService.GetProviderById(customerId).Returns(_mockedProviders.FirstOrDefault(x => x.Id == customerId));

            Should.Throw<Exception>(() => _calculatorService.CalculateVat(customerId, providerId, amount));
        }

        [Fact]
        public void ShouldThrowExceptionWhenAmountIsZero()
        {
            var customerId = 1;
            var providerId = 1;
            var amount = 0;

            _dataService.GetCustomerById(customerId).Returns(_mockedCustomers.FirstOrDefault(x => x.Id == customerId));
            _dataService.GetProviderById(customerId).Returns(_mockedProviders.FirstOrDefault(x => x.Id == customerId));

            Should.Throw<Exception>(() => _calculatorService.CalculateVat(customerId, providerId, amount));
        }
    }  
}

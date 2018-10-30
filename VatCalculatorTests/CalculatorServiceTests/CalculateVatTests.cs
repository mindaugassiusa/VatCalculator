using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
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

        public CalculateVatTests()
        {
            _dataService = Substitute.For<IDataService>();
            _calculatorService = new CalculatorService(_dataService);
        }

        [Fact]
        public void ShouldReturnZeroWhenProviderIsNotVatPayer()
        {
            var customer = new Customer()
            {
                Id = 1,
                Name = "Maxima",
                IsCompany = true,
                IsVatPayer = true,
                Country = new Country
                {
                    Id = 1,
                    Name = "Lithuania",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = true,
                        Vat = 21,
                    }
                }
            };

            var provider = new Provider
            {
                Id = 2,
                Name = "Lidl",
                IsVatPayer = false,
                Country = new Country
                {
                    Id = 1,
                    Name = "Lithuania",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = true,
                        Vat = 21,
                    }
                }
            };
            var amount = 1000;

            _dataService.GetCustomerById(customer.Id).Returns(customer);
            _dataService.GetProviderById(provider.Id).Returns(provider);

            var result = _calculatorService.CalculateVat(customer.Id, provider.Id, amount);

            result.ShouldBe(0);
        }

        [Fact]
        public void ShouldReturnZeroWhenProviderIsVatPayerAndCustomerCountryIsNotEuMemberAndCustomerAndProviderCountriesAreDifferent()
        {
            var customer = new Customer()
            {
                Id = 3,
                Name = "John Johnson",
                IsCompany = false,
                IsVatPayer = false,
                Country = new Country
                {
                    Id = 2,
                    Name = "Great Britain",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = false,
                        Vat = 20,
                    }
                }
            };

            var provider = new Provider
            {
                Id = 1,
                Name = "Senukai",
                IsVatPayer = true,
                Country = new Country
                {
                    Id = 1,
                    Name = "Lithuania",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = true,
                        Vat = 21,
                    }
                }
            };
            var amount = 1000;

            _dataService.GetCustomerById(customer.Id).Returns(customer);
            _dataService.GetProviderById(provider.Id).Returns(provider);

            var result = _calculatorService.CalculateVat(customer.Id, provider.Id, amount);

            result.ShouldBe(0);
        }

        [Fact]
        public void ShouldReturnZeroWhenCustomerCountryIsEuMemberAndCustomerIsVatPayerAndCustomerAndProviderCountriesAreDifferent()
        {
            var customer = new Customer()
            {
                Id = 2,
                Name = "Petras Petraitis",
                IsCompany = false,
                IsVatPayer = true,
                Country = new Country
                {
                    Id = 4,
                    Name = "Latvia",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = true,
                        Vat = 21,
                    }
                }
            };
            var provider = new Provider
            {
                Id = 1,
                Name = "Senukai",
                IsVatPayer = true,
                Country = new Country
                {
                    Id = 1,
                    Name = "Lithuania",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = true,
                        Vat = 21,
                    }
                }
            };
            var amount = 1000;

            _dataService.GetCustomerById(customer.Id).Returns(customer);
            _dataService.GetProviderById(provider.Id).Returns(provider);

            var result = _calculatorService.CalculateVat(customer.Id, provider.Id, amount);

            result.ShouldBe(0);
        }

        [Fact]
        public void ShouldReturnCalculatedVatWhenCustomerCountryIsEuMemberAndCustomerIsNotVatPayerAndCustomerAndProviderCountriesAreDifferent()
        {
            var customer = new Customer()
            {
                Id = 4,
                Name = "Pro Alpha",
                IsCompany = true,
                IsVatPayer = false,
                Country = new Country
                {
                    Id = 3,
                    Name = "Germany",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = true,
                        Vat = 19,
                    }
                }
            };
            var provider = new Provider
            {
                Id = 1,
                Name = "Senukai",
                IsVatPayer = true,
                Country = new Country
                {
                    Id = 1,
                    Name = "Lithuania",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = true,
                        Vat = 21,
                    }
                }
            };
            var amount = 1000;

            _dataService.GetCustomerById(customer.Id).Returns(customer);
            _dataService.GetProviderById(provider.Id).Returns(provider);

            var result = _calculatorService.CalculateVat(customer.Id, provider.Id, amount);

            result.ShouldBe(190);
        }

        [Fact]
        public void ShouldCalculateVatWhenCustomerAndProviderCountriesAreTheSame()
        {
            var customer = new Customer()
            {
                Id = 1,
                Name = "Maxima",
                IsCompany = true,
                IsVatPayer = true,
                Country = new Country
                {
                    Id = 1,
                    Name = "Lithuania",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = true,
                        Vat = 21,
                    }
                }
            };
            var provider = new Provider
            {
                Id = 1,
                Name = "Senukai",
                IsVatPayer = true,
                Country = new Country
                {
                    Id = 1,
                    Name = "Lithuania",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = true,
                        Vat = 21,
                    }
                }
            };
            var amount = 2000;

            _dataService.GetCustomerById(customer.Id).Returns(customer);
            _dataService.GetProviderById(provider.Id).Returns(provider);

            var result = _calculatorService.CalculateVat(customer.Id, provider.Id, amount);

            result.ShouldBe(420);
        }

        [Fact]
        public void ShouldThrowExceptionWhenCustomerIsZero()
        {
            var customer = new Customer()
            {
                Id = 0,
                Name = "",
                IsCompany = true,
                IsVatPayer = true,
                Country = new Country
                {
                    Id = 0,
                    Name = "",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = true,
                        Vat = 0,
                    }
                }
            };
            var provider = new Provider
            {
                Id = 1,
                Name = "Senukai",
                IsVatPayer = true,
                Country = new Country
                {
                    Id = 1,
                    Name = "Lithuania",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = true,
                        Vat = 21,
                    }
                }
            };
            var amount = 1000;

            _dataService.GetCustomerById(customer.Id).Returns(customer);
            _dataService.GetProviderById(provider.Id).Returns(provider);

            Should.Throw<Exception>(() => _calculatorService.CalculateVat(customer.Id, provider.Id, amount));
        }

        [Fact]
        public void ShouldThrowExceptionWhenProviderIsZero()
        {
            var customer = new Customer()
            {
                Id = 1,
                Name = "Maxima",
                IsCompany = true,
                IsVatPayer = true,
                Country = new Country
                {
                    Id = 1,
                    Name = "Lithuania",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = true,
                        Vat = 21,
                    }
                }
            };
            var provider = new Provider
            {
                Id = 0,
                Name = "",
                IsVatPayer = true,
                Country = new Country
                {
                    Id = 0,
                    Name = "",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = true,
                        Vat = 0,
                    }
                }
            };
            var amount = 1000;

            _dataService.GetCustomerById(customer.Id).Returns(customer);
            _dataService.GetProviderById(provider.Id).Returns(provider);

            Should.Throw<Exception>(() => _calculatorService.CalculateVat(customer.Id, provider.Id, amount));
        }

        [Fact]
        public void ShouldThrowExceptionWhenAmountIsZero()
        {
            var customer = new Customer()
            {
                Id = 1,
                Name = "Maxima",
                IsCompany = true,
                IsVatPayer = true,
                Country = new Country
                {
                    Id = 1,
                    Name = "Lithuania",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = true,
                        Vat = 21,
                    }
                }
            };
            var provider = new Provider
            {
                Id = 1,
                Name = "Senukai",
                IsVatPayer = true,
                Country = new Country
                {
                    Id = 1,
                    Name = "Lithuania",
                    CountryInformation = new CountryInformation
                    {
                        IsEuMember = true,
                        Vat = 21,
                    }
                }
            };
            var amount = 0;

            _dataService.GetCustomerById(customer.Id).Returns(customer);
            _dataService.GetProviderById(provider.Id).Returns(provider);

            Should.Throw<Exception>(() => _calculatorService.CalculateVat(customer.Id, provider.Id, amount));
        }
    }  
}

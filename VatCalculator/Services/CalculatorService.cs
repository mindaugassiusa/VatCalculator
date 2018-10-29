using System;
using System.Collections.Generic;
using System.Text;
using VatCalculator.Interfaces;

namespace VatCalculator.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly IDataService _dataService;

        public CalculatorService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public decimal CalculateVat(int customerId, int providerId, decimal amount)
        {
            if (customerId == 0 || providerId == 0 || amount == 0)
            {
                throw new Exception("Wrong parameters provided.");
            }

            var customer = _dataService.GetCustomerById(customerId);
            var provider = _dataService.GetProviderById(providerId);

            if (!provider.IsVatPayer
                || (provider.IsVatPayer && !customer.Country.IsEuMember && provider.Country != customer.Country)
                || (customer.Country.IsEuMember && customer.IsVatPayer && provider.Country != customer.Country))
            {
                return 0;
            }

            if ((customer.Country.IsEuMember && !customer.IsVatPayer && provider.Country != customer.Country)
                || (customer.Country == provider.Country))
            {
                var result = Decimal.Multiply(amount, customer.Country.Vat) / 100;

                return result;
            }

            throw new Exception("System failed to calculate your request");
        }
    }
}

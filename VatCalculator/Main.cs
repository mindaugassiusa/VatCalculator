using System;
using VatCalculator.Interfaces;

namespace VatCalculator
{
    public class Main
    {
        private readonly ICalculatorService _calculatorService;

        public Main(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        public decimal Start(int customerId, int providerId, decimal amount)
        {
            return _calculatorService.CalculateVat(customerId, providerId, amount);
        }
    }
}

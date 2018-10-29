using System;
using System.Collections.Generic;
using System.Text;

namespace VatCalculator.Interfaces
{
    public interface ICalculatorService
    {
        decimal CalculateVat(int customerId, int providerId, decimal amount);
    }
}

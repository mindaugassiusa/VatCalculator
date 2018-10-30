
namespace VatCalculator.Interfaces
{
    public interface ICalculatorService
    {
        decimal CalculateVat(int customerId, int providerId, decimal amount);
    }
}

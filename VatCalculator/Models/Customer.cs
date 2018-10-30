
namespace VatCalculator.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsCompany { get; set; }
        public bool IsVatPayer { get; set; }
        public Country Country { get; set; }
    }
}

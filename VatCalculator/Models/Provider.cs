
namespace VatCalculator.Models
{
    public class Provider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsVatPayer { get; set; }
        public Country Country { get; set; }
    }
}


namespace VatCalculator.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int Vat { get; set; }
        //public bool IsEuMember { get; set; }
        public CountryInformation CountryInformation { get; set; }
    }
}


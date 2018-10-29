using System;
using System.Collections.Generic;
using System.Text;

namespace VatCalculator.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Vat { get; set; }
        public bool IsEuMember { get; set; }
    }
}


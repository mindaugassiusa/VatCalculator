using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using VatCalculator.Services;
using Xunit;

namespace VatCalculatorTests.DataServiceTests
{
    public class GetCustomerByIdTests
    {
        protected readonly DataService _dataService;

        public GetCustomerByIdTests()
        {
            _dataService = new DataService();
        }

        [Fact]
        public void ShouldGetCorrectCustomerById()
        {
            int id = 1;

            var result = _dataService.GetCustomerById(id);

            result.Name.ShouldBe("Maxima");
        }

        [Fact]
        public void ShouldNotGetCustomerByNotExistingId()
        {
            int id = 9;

            var result = _dataService.GetCustomerById(id);

            result.ShouldBeNull();
        }
    }
}

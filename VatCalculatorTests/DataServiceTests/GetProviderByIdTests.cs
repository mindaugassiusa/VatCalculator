using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using VatCalculator.Services;
using Xunit;

namespace VatCalculatorTests.DataServiceTests
{
    public class GetProviderByIdTests
    {
        protected readonly DataService _dataService;

        public GetProviderByIdTests()
        {
            _dataService = new DataService();
        }

        [Fact]
        public void ShouldGetCorrectCustomerById()
        {
            int id = 1;

            var result = _dataService.GetProviderById(id);

            result.Name.ShouldBe("Senukai");
        }

        [Fact]
        public void ShouldNotGetCustomerByNotExistingId()
        {
            int id = 9;

            var result = _dataService.GetProviderById(id);

            result.ShouldBeNull();
        }
    }
}

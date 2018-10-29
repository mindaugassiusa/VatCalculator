using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using VatCalculator.Services;
using Xunit;

namespace VatCalculatorTests.DataServiceTests
{
    public class GetCountryByIdTests
    {
        protected readonly DataService _dataService;

        public GetCountryByIdTests()
        {
            _dataService = new DataService();
        }

        [Fact]
        public void ShouldGetCorrectCountryById()
        {
            int id = 1;

            var result = _dataService.GetCountryById(id);

            result.Name.ShouldBe("Lithuania");
        }

        [Fact]
        public void ShouldNotGetCountryByNotExistingId()
        {
            int id = 9;

            var result = _dataService.GetCountryById(id);

            result.ShouldBeNull();
        }
    }
}

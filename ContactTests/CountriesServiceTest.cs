using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using Services;
using Xunit;
using System.Security.Cryptography.X509Certificates;

namespace ContactTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
        }

        #region AddCountry

        //When CountryAddRequest is null, AddCountry should throw ArgumentNullException
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? req = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {   //Act
                _countriesService.AddCountry(req);
            });
        }

        //When CountryName is null, empty or whitespace, AddCountry should throw ArgumentException

        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? req = new CountryAddRequest()
            { CountryName = null }; 

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {   //Act
                _countriesService.AddCountry(req);
            });
        }

        //When CountryName is duplicate, AddCountry should throw ArgumentException
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? req1 = new CountryAddRequest()
            { CountryName = "USA" };
            CountryAddRequest? req2 = new CountryAddRequest()
            { CountryName = "USA" };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {   //Act
                _countriesService.AddCountry(req1);
                _countriesService.AddCountry(req2);
            });
        }


        //When CountryName is valid, AddCountry should return CountryResponse with valid CountryID and CountryName
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest? req = new CountryAddRequest()
            {
                CountryName = "Pakistan"
            };
            
            //Act
            CountryResponse response = _countriesService.AddCountry(req);
            List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();

            //Assert
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countries_from_GetAllCountries);
        }

        #endregion

        #region GetAllCountries
        [Fact]
        // the list of countries should be empty initially
        public void GetAllCountries_EmptyList()
        {
            //Acts
            List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actual_country_response_list);
        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
            {
                new CountryAddRequest(){ CountryName= "Pakistan"},
                new CountryAddRequest(){ CountryName= "USA"},
                new CountryAddRequest(){ CountryName= "UK"},
            };

            //Act
            List<CountryResponse> actual_country_response_list = new List<CountryResponse>();

            foreach (CountryAddRequest country_req in country_request_list)
            { 
                actual_country_response_list.Add( _countriesService.AddCountry(country_req) );
            
            }

            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

            foreach(CountryResponse expected_country in actualCountryResponseList)
            {
                 Assert.Contains(expected_country,actual_country_response_list);
            }
        }
        #endregion

        #region GetCountryByCountryID
        [Fact]
        //if we supply null as countryID to GetCountryByCountryID method, it should return null as countryResponse
        public void GetCountryByCountryID_NullCountryID()
        {
            //Arrange
            Guid? countryID = null;

            //Act
            CountryResponse? countryResponse_from_get_method = _countriesService.GetCountryByCountryID(countryID);

            //Assert
            Assert.Null(countryResponse_from_get_method);
        }

        [Fact]
        // if we  supply a valid country id, it should return the matching country details as CountryResponse object
        public void GetCountryByCountryID_ValidCountryID()
        {
            //Arrange
            CountryAddRequest? req = new CountryAddRequest()
            {
                CountryName = "Pakistan"
            };
            CountryResponse countryResponse_from_add_method = _countriesService.AddCountry(req);
            Guid? countryID = countryResponse_from_add_method.CountryID;
            //Act
            CountryResponse? countryResponse_from_get_method = _countriesService.GetCountryByCountryID(countryID);
            //Assert
            Assert.NotNull(countryResponse_from_get_method);
            Assert.Equal(countryResponse_from_add_method, countryResponse_from_get_method);
        }

        #endregion
    }

}

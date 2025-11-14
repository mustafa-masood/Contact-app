using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ServiceContracts;
using ServiceContracts.Enums;
using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;

namespace ContactTests
{
    public class PersonsServiceTest
    {
        //private field
        private readonly ICountriesService _countriesService;
        private readonly IPersonsService _personService;
        private readonly ITestOutputHelper _testOutputHelper;

        //constructor
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonsService();
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson
        //when we supply null value as PersonAddRequest , AddPerson should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAdd = null;

            //Assert & Act
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService.AddPerson(personAdd);
            });
        }

        // When PersonAddRequest is null, AddPerson should throw ArgumentNullException
        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAdd = new PersonAddRequest()
            {
                PersonName = null
            };

            //Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.AddPerson(personAdd);
            });
        }

        //When we supply proper person details , it should insert the person into the persons list and it should return an object of PersonResponse type which contains newly generated PersonID
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest? personAdd = new PersonAddRequest()
            {
                PersonName = "hani",
                Email = "hani@gmail.com",
                Address = "Karachi",
                CountryID = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2003-01-01"),
                RecieveNewsLetters = true
            };


            ///Act
            PersonResponse person_response_from_add = _personService.AddPerson(personAdd);
            List<PersonResponse> persons_list = _personService.GetAllPersons();

            //Assert
            Assert.True(person_response_from_add.PersonID != Guid.Empty);
            Assert.Contains(person_response_from_add, persons_list);
        }


        #endregion


        #region GetPersonByPersonID

        // if we supply null as personID , it should return null as PersonResponse

        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Guid? personID = null;

            //Act
            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(personID);

            //Assert
            Assert.Null(person_response_from_get);
        }

        //if we supply a valid person id, it should return the valid person details as PersonResponse Object

        [Fact]
        public void GetPersonByPersonID_WithPersonID()
        {
            //Arrange
            CountryAddRequest country_req = new CountryAddRequest()
            {
                CountryName = "Canada"
            };
            CountryResponse country_response = _countriesService.AddCountry(country_req);

            //Act
            PersonAddRequest personAddReq = new PersonAddRequest()
            {
                PersonName = "Laiba",
                Email = "laiba@example.com",
                Address = "address",
                CountryID = country_response.CountryID,
                DateOfBirth = DateTime.Parse("2003-01-01"),
                Gender = GenderOptions.Female,
                RecieveNewsLetters = false
            };
            PersonResponse person_response_from_add = _personService.AddPerson(personAddReq);

            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(person_response_from_add.PersonID);
              
            //Assert
            Assert.Equal(person_response_from_add, person_response_from_get);
            
        }


        #endregion


        #region GetAllPersons
        // the GetAllPersons() should return an empty list by default

        [Fact]
        public void GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> persons_from_get = _personService.GetAllPersons();

            //Assert
            Assert.Empty(persons_from_get);
        }

        // first we will add few persons then when we call GetAllPersons(), it should return those same persons that were added

        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest country_req_1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest country_req_2 = new CountryAddRequest()
            {
                CountryName = "Pakistan"
            };

            CountryResponse country_resp_1 = _countriesService.AddCountry(country_req_1);
            CountryResponse country_resp_2 = _countriesService.AddCountry(country_req_2);

            PersonAddRequest person_req_1 = new PersonAddRequest()
            {
                PersonName = "Hani",
                Email = "hani@example.com",
                Gender = GenderOptions.Male,
                Address = "Karachi",
                CountryID = country_resp_1.CountryID,
                DateOfBirth = DateTime.Parse("2003-01-23"),
                RecieveNewsLetters = true
            };

            PersonAddRequest person_req_2 = new PersonAddRequest()
            {
                PersonName = "Laiba",
                Email = "laiba@example.com",
                Gender = GenderOptions.Female,
                Address = "Karachi",
                CountryID = country_resp_2.CountryID,
                DateOfBirth = DateTime.Parse("2003-08-19"),
                RecieveNewsLetters = true
            };

            PersonAddRequest person_req_3 = new PersonAddRequest()
            {
                PersonName = "Mustafa",
                Email = "mustafa@example.com",
                Gender = GenderOptions.Male,
                Address = "Karachi",
                CountryID = country_resp_2.CountryID,
                DateOfBirth = DateTime.Parse("2003-01-23"),
                RecieveNewsLetters = false
            };

            List<PersonAddRequest> person_reqs = new List<PersonAddRequest>()
            {
                person_req_1, person_req_2, person_req_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest personAddRequest in person_reqs)
            {
                PersonResponse person_response = _personService.AddPerson(personAddRequest);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add 
            _testOutputHelper.WriteLine("Expected : ");
            foreach(PersonResponse persons in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(persons.ToString());
            }

            //Act
            List<PersonResponse> person_response_list_from_get = _personService.GetAllPersons();

            //print person_response_list_from_get 
            _testOutputHelper.WriteLine("Actual : ");
            foreach (PersonResponse persons in person_response_list_from_get)
            {
                _testOutputHelper.WriteLine(persons.ToString());
                //the toString method just shows the classes (e.g. ServiceContracts.DTO.PersonResponse) so we have to override the implementation of toString() method in order to get the actual data
            }

            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_get)
            {
                Assert.Contains(person_response_from_add,person_response_list_from_get);
            }

        }

        #endregion
    }
} 

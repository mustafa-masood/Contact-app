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

namespace ContactTests
{
    public class PersonsServiceTest
    {
        //private field
        private readonly ICountriesService _countriesService;
        private readonly IPersonsService _personService;

        //constructor
        public PersonsServiceTest()
        {
            _personService = new PersonsService();
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
                DateOfBirth = DateTime.Parse("2003-08-19"),
                Gender = GenderOptions.Female,
                RecieveNewsLetters = false
            };
            PersonResponse person_response_from_add = _personService.AddPerson(personAddReq);

            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(person_response_from_add.CountryID);
              
            //Assert
            Assert.Equal(person_response_from_add, person_response_from_get);
            
        }


        #endregion
    }
}

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
        private readonly IPersonsService _personService;

        //constructor
        public PersonsServiceTest()
        {
            _personService = new PersonsService();
        }

        #region AddPerson
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
            Assert.Throws<ArgumentNullException>(() =>
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
    }
}

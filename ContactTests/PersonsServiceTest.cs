using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ServiceContracts;
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
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest personAdd = null;

            //Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService.AddPerson(personAdd);
            });
        }
        #endregion
    }
}

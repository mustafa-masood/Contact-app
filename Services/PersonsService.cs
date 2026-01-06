using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO; 
using ServiceContracts;
using Entities;
using System.ComponentModel.DataAnnotations;
using Services.Helpers;
using ServiceContracts.Enums;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        //private field
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        //constructor
        public PersonsService(bool init = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();

            if (init)
            {

                _persons.AddRange(new List<Person>()
                {
                    new Person()
                    {
                        PersonID = Guid.Parse("564347EA-2FB9-4F5C-83E4-7AB37DA68DC2"),
                        PersonName = "Elga",
                        Email = "emcgrale0@qq.com",
                        DateOfBirth = DateTime.Parse("2002-08-27"),
                        Gender = "Female",
                        CountryID = Guid.Parse("6CB389A3-4DF1-4C74-8908-3EEC3CA377A9"),
                        Address = "34 Sachtjen Plaza",
                        RecieveNewsLetters = true
                    },
                    new Person()
                    {
                        PersonID = Guid.Parse("7FE30633-0D67-45F8-A9E7-586B320A4B0D"),
                        PersonName = "Laiba",
                        Email = "laiba@qq.com",
                        DateOfBirth = DateTime.Parse("2003-07-20"),
                        Gender = "Female",
                        CountryID = Guid.Parse("243BD4D3-B18A-429D-8059-736B0E29DB7B"),
                        Address = "34 Sachtjen Regency",
                        RecieveNewsLetters = true
                    },
                    new Person()
                    {
                        PersonID = Guid.Parse("133C3796-FC37-41F6-B80F-269DDC598A83"),
                        PersonName = "Umair",
                        Email = "umair@qq.com",
                        DateOfBirth = DateTime.Parse("2004-01-27"),
                        Gender = "Male",
                        CountryID = Guid.Parse("D1F2FF27-94C5-4C5E-9CF5-6E8CF7599001"),
                        Address = "34 Luxury Plaza",
                        RecieveNewsLetters = true
                    },
                    new Person()
                    {
                        PersonID = Guid.Parse("C6CD9FB4-B738-42C7-A841-DC14EC1D4F5B"),
                        PersonName = "Hafsah",
                        Email = "hafsah@qq.com",
                        DateOfBirth = DateTime.Parse("2004-09-27"),
                        Gender = "Female",
                        CountryID = Guid.Parse("4C7010BE-75E0-44E0-8867-BAAA3CCC470C"),
                        Address = "Zeeshan Luxury",
                        RecieveNewsLetters = true
                    },
                    new Person()
                    {
                        PersonID = Guid.Parse("55BDEB9A-1D22-47C2-9409-1DBE1D358600"),
                        PersonName = "Hani",
                        Email = "Hani@qq.com",
                        DateOfBirth = DateTime.Parse("2003-01-23"),
                        Gender = "Male",
                        CountryID = Guid.Parse("227B9342-032C-4460-95C8-D3DDB57855FC"),
                        Address = "69 Sachtjen Glacier",
                        RecieveNewsLetters = true
                    },
                });
            }
        }
        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;
            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            // Check if personAddRequest is not null
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest), "PersonAddRequest cannot be null.");
            }

            // Model Validation
            ValidationHelper.ModelValidation(personAddRequest);

            // convert personAddRequest to Person type
            Person person = personAddRequest.ToPerson();

            //generate new PersonID
            person.PersonID = Guid.NewGuid();

            //add person to the list
            _persons.Add(person);

            //convert Person to PersonResponse type
            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(temp => temp.ToPersonResponse()).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
                return null;

            Person? person = _persons.FirstOrDefault(temp => temp.PersonID == personID);

            if (person == null)
                return null;

            return person.ToPersonResponse();
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> filteredPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
                return filteredPersons;

            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    filteredPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.PersonName) ? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Email):
                    filteredPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Email) ? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Gender):
                    filteredPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender) ? temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Address):
                    filteredPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Address) ? temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.CountryID):
                    filteredPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Country) ? temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.DateOfBirth):
                    filteredPersons = allPersons.Where(temp => (temp.DateOfBirth != null) ? temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                default: filteredPersons = allPersons; break;

            }
            return filteredPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (sortBy == null)
                return allPersons;

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth),
                SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

                (nameof(PersonResponse.RecieveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.RecieveNewsLetters).ToList(),
                (nameof(PersonResponse.RecieveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.RecieveNewsLetters).ToList(),

                _ => allPersons

            };
            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            //check if personUpdateRequest is not null
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person), "PersonUpdateRequest cannot be null.");
            }

            //validate all properties of personUpdateRequest
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object
            Person matchingPerson = _persons.FirstOrDefault(temp => temp.PersonID == personUpdateRequest.PersonID);

            if (matchingPerson == null)
            {
                throw new ArgumentException("Given PersonID does not exist");
            }

            //update all details 
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.RecieveNewsLetters = personUpdateRequest.RecieveNewsLetters;

            return matchingPerson.ToPersonResponse();

        }

        public bool DeletePerson(Guid? personID)
        {
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID), "PersonID cannot be null.");
            }

            Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonID == personID);
            if (matchingPerson == null)
            {
                return false;
            }
            _persons.RemoveAll(temp => temp.PersonID == personID);
            return true;
        }
    }
}

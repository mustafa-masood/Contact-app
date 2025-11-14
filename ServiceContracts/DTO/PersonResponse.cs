using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents the response containing details about a person.
    /// </summary>
    /// <remarks>This class is typically used to encapsulate the data returned from an operation that
    /// retrieves information about a person. The specific properties and structure of the response depend on the
    /// implementation.</remarks>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public bool RecieveNewsLetters { get; set; }
        public double? Age { get; set; }


        /// <summary>
        /// compares the current object data with the parameter object
        /// </summary>
        /// <param name="obj">The PersonResponse object to compare</param>
        /// <returns>True or False indication whether all person details are matched with the specified parameter object</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person_to_compare = (PersonResponse)obj;
            return PersonID == person_to_compare.PersonID
                && PersonName == person_to_compare.PersonName
                && Email == person_to_compare.Email
                && DateOfBirth == person_to_compare.DateOfBirth
                && Gender == person_to_compare.Gender
                && CountryID == person_to_compare.CountryID
                && Address == person_to_compare.Address
                && RecieveNewsLetters == person_to_compare.RecieveNewsLetters;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Person ID: {PersonID}, Person Name: {PersonName}, Email: {Email}, Date of Birth: {DateOfBirth?.ToString("dd MMM yyyy")}, Gender: {Gender}, Country ID: {CountryID}, Country: {Country}, Address: {Address}, Recieve News Letter: {RecieveNewsLetters}";
        }
    }
        public static class PersonExtensions
        {
            /// <summary>
            /// Converts a Person domain model object to a PersonResponse DTO object.
            /// </summary>
            /// <param name="person">The Person object to convert.</param>
            /// <returns>Returns the converted PersonResponse object</returns>
            public static PersonResponse ToPersonResponse(this Person person)
            {
                return new PersonResponse()
                {
                    PersonID = person.PersonID,
                    PersonName = person.PersonName,
                    Email = person.Email,
                    DateOfBirth = person.DateOfBirth,
                    Gender = person.Gender,
                    RecieveNewsLetters = person.RecieveNewsLetters,
                    CountryID = person.CountryID,
                    Address = person.Address,
                    Age = person.DateOfBirth.HasValue ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25, 1) : null
                };
            }
        }
    }
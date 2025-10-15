﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for Person entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// adds a person to the list of persons
        /// </summary>
        /// <param name="personAddRequest">Person to add</param>
        /// <returns>return the same person details along with newly generated PersonID</returns>
        PersonResponse AddPerson (PersonAddRequest? personAddRequest);

        /// <summary>
        /// Retrieves a list of all persons.
        /// </summary>
        /// <returns>A list of <see cref="PersonResponse"/> objects representing all persons.  The list will be empty if no
        /// persons are available.</returns>
        List<PersonResponse> GetAllPersons();
    }
}

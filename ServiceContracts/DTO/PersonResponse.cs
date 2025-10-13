using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


    }
}

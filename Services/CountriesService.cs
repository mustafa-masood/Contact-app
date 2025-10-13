using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //private field
        private readonly List<Country> _countries;

        //constructor
        public CountriesService()
        {
            _countries = new List<Country>();
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            //validation: countryAddRequest should not be null  
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //validation: CountryName should not be null, empty or whitespace
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //validation: countryName should not exist already (duplicate check)
            if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("CountryName already exists");
            }

            //convert object from countryAddRequest to country object
            Country country = countryAddRequest.ToCountry();

            //Generate new country id (Guid)
            country.CountryID = Guid.NewGuid();

            //Add country object to the list (_countries)
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            // select returns IEnumerable, so convert it to List using ToList()
            return _countries.Select(temp => temp.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null)
            {
                return null;
            }


            Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryID == countryID);
            if (country_response_from_list == null)
            {
                return null;
            }
            else
            {
                return country_response_from_list.ToCountryResponse();
            }
        }
    }
}
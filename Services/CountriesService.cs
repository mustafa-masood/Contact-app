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
        public CountriesService(bool init = true)
        {
            _countries = new List<Country>();
            if (init)
            {
                _countries.AddRange(new List<Country>()
                {


                new Country() {
                    CountryID = Guid.Parse("6CB389A3-4DF1-4C74-8908-3EEC3CA377A9"),
                    CountryName = "India"
                },

                new Country() {
                    CountryID = Guid.Parse("243BD4D3-B18A-429D-8059-736B0E29DB7B"),
                    CountryName = "Pakistan"
                },

                new Country() {
                    CountryID = Guid.Parse("D1F2FF27-94C5-4C5E-9CF5-6E8CF7599001"),
                    CountryName = "USA"
                },

                new Country() {
                    CountryID = Guid.Parse("4C7010BE-75E0-44E0-8867-BAAA3CCC470C"),
                    CountryName = "UK"
                },

                new Country() {
                    CountryID = Guid.Parse("227B9342-032C-4460-95C8-D3DDB57855FC"),
                    CountryName = "Malaysia"
                }
                });


            }
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
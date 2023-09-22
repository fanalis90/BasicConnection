using BasicConnection.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicConnection.Controllers
{
    public class CountryController
    {
        private Country _country;
        private CountryView _countryView;

        public CountryController(Country country, CountryView countryView)
        {
            _country = country;
            _countryView = countryView;
        }

        public void GetAll()
        {
            var results = _country.GetAll();
            if (!results.Any())
            {
                Console.WriteLine("No data found");
            }
            else
            {
                _countryView.List(results, "regions");
            }
        }

        public void Insert()
        {
            Country input = _countryView.InsertInput(); ;
     

            var result = _country.Insert(input);

            _countryView.Transaction(result);
        }

        public void Update()
        {
            Country country = _countryView.UpdateInput();

            var result = _country.Update(country);
            _countryView.Transaction(result);
        }

        public void Delete()
        {
            string input = _countryView.DeleteInput();
            var result = _country.Delete(input);

            _countryView.Transaction(result);

        }
    }
}

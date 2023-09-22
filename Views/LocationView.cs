using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicConnection.Views
{
    public class LocationView : GeneralView
    {
        public Location InsertInput()
        {
            int locationId;
            while (true)
            {
                Console.WriteLine("Insert id location");
                if (int.TryParse(Console.ReadLine(), out locationId))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("masukan hanya angka");
                }
            }
            Console.WriteLine("Insert Streed Address");
            var streedAddress = Console.ReadLine();
            Console.WriteLine("Insert Postal Code");
            var postalCode = Console.ReadLine();
            Console.WriteLine("Insert State Province");
            var stateProvince = Console.ReadLine();
            Console.WriteLine("Insert City");
            var city = Console.ReadLine();
            Console.WriteLine("Insert Country Id");
            var countryId = Console.ReadLine();

            return new Location { 
                Id = locationId,
                StreetAddress = streedAddress,
                PostalCode = postalCode,
                StateProvince = stateProvince,
                City = city,
                CountryId = countryId
            };
        }

        public Location UpdateInput()
        {
            int locationId;
            while (true)
            {
                Console.WriteLine("Insert id location");
                if (int.TryParse(Console.ReadLine(), out locationId))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("masukan hanya angka");
                }
            }
            Console.WriteLine("Insert Streed Address");
            var streedAddress = Console.ReadLine();
            Console.WriteLine("Insert Postal Code");
            var postalCode = Console.ReadLine();
            Console.WriteLine("Insert State Province");
            var stateProvince = Console.ReadLine();
            Console.WriteLine("Insert City");
            var city = Console.ReadLine();
            Console.WriteLine("Insert Country Id");
            var countryId = Console.ReadLine();

            return new Location
            {
                Id = locationId,
                StreetAddress = streedAddress,
                PostalCode = postalCode,
                StateProvince = stateProvince,
                City = city,
                CountryId = countryId
            };
        }

        public int DeleteInput()
        {
            Console.WriteLine("Insert Location Id");
            int id;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out id))
                {
                    return id;
                }
                else
                {
                    Console.WriteLine("Masukan id hanya angka");
                };
            }

        }
    }
}

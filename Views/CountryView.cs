using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicConnection.Views
{
    public class CountryView : GeneralView
    {
        public Country InsertInput()
        {   

            Console.WriteLine("Insert country Id");
            var id = Console.ReadLine();
            Console.WriteLine("Insert country name");
            var name = Console.ReadLine();
            int regionId;
            while (true)
            {
                Console.WriteLine("Insert regionId");
                if(int.TryParse(Console.ReadLine(), out regionId))
                {
                    break;
                } else
                {
                    Console.WriteLine("masukan hanya angka");
                }
            }
            
            return new Country
            {
                Id = id,
                Name = name,
                RegionId = regionId
            };
        }

        public Country UpdateInput()
        {
            Console.WriteLine("Insert country Id");
            var id = Console.ReadLine();
            Console.WriteLine("Insert country name");
            var name = Console.ReadLine();
            int regionId;
            while (true)
            {
                Console.WriteLine("Insert regionId");
                if (int.TryParse(Console.ReadLine(), out regionId))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("masukan hanya angka");
                }
            }

            return new Country
            {
                Id = id,
                Name = name,
                RegionId = regionId
            };
        }

        public string DeleteInput()
        {
            Console.WriteLine("Insert country Id");
            string id = Console.ReadLine();
            return id;

        }
    }
}

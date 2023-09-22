using BasicConnection.Controllers;
using BasicConnection.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicConnection
{
    public class Menu
    {
        public static void Region()
        {
            var region = new Region();
            var regionView = new RegionView();
            string tabel = "region";
            var regionController = new RegionController(region, regionView);

            var isLoop = true;
            while (isLoop)
            {
                Console.WriteLine($"1. List all {tabel}");
                Console.WriteLine($"2. Insert new {tabel}");
                Console.WriteLine($"3. Update {tabel}");
                Console.WriteLine($"4. Delete {tabel}");
                Console.WriteLine("10. Back");
                Console.Write("Enter your choice: ");
                var input2 = Console.ReadLine();
                switch (input2)
                {
                    case "1":
                        regionController.GetAll();
                        break;
                    case "2":
                        regionController.Insert();
                        break;
                    case "3":
                        regionController.Update();
                        break;
                    case "4":
                        regionController.Delete();
                        break;
                    case "10":
                        isLoop = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }

        public static void Location()
        {
            var location = new Location();
            var locationView = new LocationView();
            string tabel = "location";
            var regionController = new LocationController(location, locationView);

            var isLoop = true;
            while (isLoop)
            {
                Console.WriteLine($"1. List all {tabel}");
                Console.WriteLine($"2. Insert new {tabel}");
                Console.WriteLine($"3. Update {tabel}");
                Console.WriteLine($"4. Delete {tabel}");
                Console.WriteLine("10. Back");
                Console.Write("Enter your choice: ");
                var input2 = Console.ReadLine();
                switch (input2)
                {
                    case "1":
                        regionController.GetAll();
                        break;
                    case "2":
                        regionController.Insert();
                        break;
                    case "3":
                        regionController.Update();
                        break;
                    case "4":
                        regionController.Delete();
                        break;
                    case "10":
                        isLoop = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }

        public static void Country()
        {
            var country = new Country();
            var countryView = new CountryView();
            string tabel = "country";
            var regionController = new CountryController(country, countryView);

            var isLoop = true;
            while (isLoop)
            {
                Console.WriteLine($"1. List all {tabel}");
                Console.WriteLine($"2. Insert new {tabel}");
                Console.WriteLine($"3. Update {tabel}");
                Console.WriteLine($"4. Delete {tabel}");
                Console.WriteLine("10. Back");
                Console.Write("Enter your choice: ");
                var input2 = Console.ReadLine();
                switch (input2)
                {
                    case "1":
                        regionController.GetAll();
                        break;
                    case "2":
                        regionController.Insert();
                        break;
                    case "3":
                        regionController.Update();
                        break;
                    case "4":
                        regionController.Delete();
                        break;
                    case "10":
                        isLoop = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }

       


    }
}

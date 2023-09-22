using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicConnection.Views
{
    public class RegionView : GeneralView
    {
        public string InsertInput()
        {
            Console.WriteLine("Insert region name");
            var name = Console.ReadLine();

            return name;
        }

        public Region UpdateInput()
        {
            Console.WriteLine("Insert region id");
            var id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Insert region name");
            var name = Console.ReadLine();

            return new Region
            {
                Id = id,
                Name = name
            };
        }

        public int DeleteInput()
        {
            Console.WriteLine("Insert region Id");
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

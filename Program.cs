using System;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using BasicConnection;

namespace BasicConnection;

public class Program
{   
    // membuat string untuk configurasi koneksi ke database
    static string connectionString = "Data Source=DESKTOP-CI3320S;Integrated Security=True;Database=db_hr_dts;Connect Timeout=30;";
    static SqlConnection? connection;
    private static void Main()
    {
        var region = new Region();
        
        var region1 = region.GetById(1011);
        if (region1.Name != null)
        {
            Console.WriteLine($" data ada dengan id : {region1.Id}  dan name :{region1.Name}");
        } else
        {
            Console.WriteLine("data tidak ada");
        }
      

        //var insertRegion = region.Insert("Africa timur");
     
        //Console.WriteLine(insertRegion);
       /* var regions = region.GetAll();
        foreach (Region item in regions)
        {
            Console.WriteLine(item.Id);
            Console.WriteLine(item.Name);
        }
        Console.WriteLine(region.Update(14, "africa barat"));
        Console.WriteLine(region.Update(1011, "africa timur"));
        var region3 = region.GetById(1011);
        if (region3.Name != null)
        {
            Console.WriteLine($" data ada dengan id : {region3.Id}  dan name :{region3.Name}");
        }
        else
        {
            Console.WriteLine("data tidak ada");
        }
        Console.WriteLine(region.Delete(13));
        Console.WriteLine(region.Delete(1011));
        var region2 = region.GetById(1011);
        if (region2.Name != null)
        {
            Console.WriteLine($" data ada dengan id : {region2.Id}  dan name :{region2.Name}");
        }
        else
        {
            Console.WriteLine("data tidak ada");
        }*/
    }


}


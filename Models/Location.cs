using System.Data.SqlClient;
using System.Transactions;
using System.Xml.Linq;
using BasicConnection;

namespace BasicConnection;

public class Location
{
    public int Id { get; set; }
    public string StreetAddress { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string StateProvince { get; set; }
    public string CountryId { get; set; }

    public override string ToString()
    {
        return $"{Id} - {StreetAddress} - {PostalCode} - {City} - {StateProvince} - {CountryId}";
    }


    // GET ALL: Location
    //membuat Method get all dan mereturn list berupa kelas Location
    public List<Location> GetAll()
    {
        // mendeklarasi variabel Locations dengan list dari kelas Location
        var Locations = new List<Location>();

        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();

        //menambahkan transact query ke command
        command.CommandText = "SELECT * FROM locations";

        try
        {
            //mencoba koneksi ke database
            connection.Open();
            // menyimpan data hasil return dari method executereader ke variabel
            using var reader = command.ExecuteReader();
            //cek konidisi apabila data memiliki isi
            if (reader.HasRows)
            {
                //melakukan perulangan while untuk mengakses data
                while (reader.Read())
                {
                    //membuat dan menambahkan class Location dengan konstruktor ke dalam list Location
                    Locations.Add(new Location
                    {
                        Id = reader.GetInt32(0),
                        StreetAddress = reader.GetString(1),
                        PostalCode = reader.GetString(2),
                        City = reader.GetString(3),
                        StateProvince = reader.GetString(4),
                        CountryId = reader.GetString(5)
                    });
                }
                //menutup akses ke data
                reader.Close();
                connection.Close();

                //mereturn list Location yang memiliki isi
                return Locations;
            }
            //menutup akses ke data
            reader.Close();
            connection.Close();
            //mengembalikan list Location kosong
            return new List<Location>();
        }
        //catch error apabila program diatas me throw error
        catch (Exception ex)
        {
            //menampilkan error ke konsol
            Console.WriteLine($"Error: {ex.Message}");
        }
        //mereturn list Location kosong
        return new List<Location>();
    }

    // GET BY ID: Location
    //membuat Method get by id dan mereturn list berupa kelas Location
    public Location GetById(int id)
    {
        //melakukan instansiasi kelas Location
        var Location = new Location();
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "SELECT * FROM locations WHERE id = @id";

        try
        {
            //menambah parameter untuk command
            command.Parameters.Add(new SqlParameter("@id", id));
            //mencoba koneksi ke database
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;
                // menyimpan data hasil return dari method executereader ke variabel
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        //menambahkan isi untuk objek Location dengan menggunakan while loop
                        Location.Id = reader.GetInt32(0);
                        Location.StreetAddress = reader.GetString(1);
                        Location.PostalCode = reader.GetString(2);
                        Location.City = reader.GetString(3);
                        Location.StateProvince = reader.GetString(4);
                        Location.CountryId = reader.GetString(5);

                    }

                    //menutup akses ke koneksi dan reader
                    reader.Close();
                    connection.Close();
                    //mengembalikan Location yang sudah di isi
                    return Location;
                }

                //menutup akses ke data
                reader.Close();
                connection.Close();
                //mereturn Location kosong
                return Location;
            }


            catch (Exception ex)
            {
                //melakukan rollback apabila ada error
                transaction.Rollback();
                //menampilkan error ke konsol
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            //menampilkan error ke konsol
            Console.WriteLine($"Error: {ex.Message}");
        }


        //mereturn Location kosong
        return Location;
    }

    // INSERT: Location
    //membuat Method Insert dan mereturn string
    public string Insert(Location location )
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "INSERT INTO locations (id, street_address, postal_code, city, state_province, country_id) VALUES (@id, @streedAdd, @postalCode, @city, @stateProvince, @countryId);";

        try
        {
            //menambah parameter untuk command
            command.Parameters.Add(new SqlParameter("@id", location.Id));
            command.Parameters.Add(new SqlParameter("@streetAdd", location.StreetAddress));
            command.Parameters.Add(new SqlParameter("@postalCode", location.PostalCode));
            command.Parameters.Add(new SqlParameter("@city", location.City));
            command.Parameters.Add(new SqlParameter("@stateProvince", location.StateProvince));
            command.Parameters.Add(new SqlParameter("@countryId", location.CountryId));
            //mencoba koneksi ke database
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;
                // menyimpan data hasil return dari method executenonquery ke variabel
                var result = command.ExecuteNonQuery();
                //melakukan commit untuk semua transaksi
                transaction.Commit();
                connection.Close();
                //mereturn nilai result dengan method tostring agar menjadi string
                return result.ToString();
            }
            catch (Exception ex)
            {
                //melakukan rollback apabila ada error pada transaksi
                transaction.Rollback();
                //menampilkan error ke konsol
                return $"Error Transaction: {ex.Message}";
            }
        }
        catch (Exception ex)
        {
            //menampilkan error ke konsol
            return $"Error: {ex.Message}";
        }
    }

    // UPDATE: Location
    //membuat Method Update dan mereturn string
    public string Update(Location location)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "UPDATE locations SET street_address = @streedAdd, postal_code = @postalCode, city = @city, state_province = @stateProvince,  country_id = @countryId WHERE id = @id;";

        try
        {
            //menambah parameter untuk command
            command.Parameters.Add(new SqlParameter("@id", location.Id));
            command.Parameters.Add(new SqlParameter("@streetAdd", location.StreetAddress));
            command.Parameters.Add(new SqlParameter("@postalCode", location.PostalCode));
            command.Parameters.Add(new SqlParameter("@city", location.City));
            command.Parameters.Add(new SqlParameter("@stateProvince", location.StateProvince));
            command.Parameters.Add(new SqlParameter("@countryId", location.CountryId));
            //mencoba koneksi ke database
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;
                // menyimpan data hasil return dari method executenonquery ke variabel
                var result = command.ExecuteNonQuery();
                //melakukan commit untuk semua transaksi
                transaction.Commit();
                connection.Close();
                //mereturn nilai result dengan method tostring agar menjadi string
                return result.ToString();
            }
            catch (Exception ex)
            {
                //melakukan rollback apabila ada error pada transaksi
                transaction.Rollback();
                //menampilkan error ke konsol
                return $"Error Transaction: {ex.Message}";
            }
        }
        catch (Exception ex)
        {
            //menampilkan error ke konsol
            return $"Error: {ex.Message}";
        }
    }

    // DELETE: Location
    //membuat Method Insert dan mereturn string
    public string Delete(int id)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "DELETE FROM locations WHERE id = @id;";

        try
        {
            //menambah parameter untuk command
            command.Parameters.Add(new SqlParameter("@id", id));
            //mencoba koneksi ke database
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;
                // menyimpan data hasil return dari method executenonquery ke variabel
                var result = command.ExecuteNonQuery();
                //melakukan commit untuk semua transaksi
                transaction.Commit();
                connection.Close();

                //mereturn nilai result dengan method tostring agar menjadi string
                return result.ToString();
            }
            catch (Exception ex)
            {
                //melakukan rollback apabila ada error pada transaksi
                transaction.Rollback();
                //menampilkan error ke konsol
                return $"Error Transaction: {ex.Message}";
            }
        }
        catch (Exception ex)
        {
            //menampilkan error ke konsol
            return $"Error: {ex.Message}";
        }
    }




}
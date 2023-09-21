using System.Data.SqlClient;
using System.Transactions;
using System.Xml.Linq;
using BasicConnection;

namespace BasicConnection;

public class Country
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int RegionId { get; set; }

    public override string ToString()
    {
        return $"{Id} - {Name} - {RegionId} ";
    }


    // GET ALL: Country
    //membuat Method get all dan mereturn list berupa kelas Country
    public List<Country> GetAll()
    {
        // mendeklarasi variabel Countries dengan list dari kelas Country
        var Countries = new List<Country>();

        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();

        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();

        //menambahkan transact query ke command
        command.CommandText = "SELECT * FROM countries";

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
                    //membuat dan menambahkan class Country dengan konstruktor ke dalam list Country
                    Countries.Add(new Country
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                        RegionId = reader.GetInt32(2)
                    });
                }
                //menutup akses ke data
                reader.Close();
                connection.Close();

                //mereturn list Country yang memiliki isi
                return Countries;
            }
            //menutup akses ke data
            reader.Close();
            connection.Close();
            //mengembalikan list Country kosong
            return new List<Country>();
        }
        //catch error apabila program diatas me throw error
        catch (Exception ex)
        {
            //menampilkan error ke konsol
            Console.WriteLine($"Error: {ex.Message}");
        }
        //mereturn list Country kosong
        return new List<Country>();
    }

    // GET BY ID: Country
    //membuat Method get by id dan mereturn list berupa kelas Country
    public Country GetById(string id)
    {
        //melakukan instansiasi kelas Country
        var Country = new Country();
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "SELECT * FROM countries WHERE id = @id";

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
                        //menambahkan isi untuk objek Country dengan menggunakan while loop
                        Country.Id = reader.GetString(0);
                        Country.Name = reader.GetString(1);
                        Country.RegionId = reader.GetInt32(2);
                    }

                    //menutup akses ke koneksi dan reader
                    reader.Close();
                    connection.Close();
                    //mengembalikan Country yang sudah di isi
                    return Country;
                }

                //menutup akses ke data
                reader.Close();
                connection.Close();
                //mereturn Country kosong
                return Country;
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


        //mereturn Country kosong
        return Country;
    }

    // INSERT: Country
    //membuat Method Insert dan mereturn string
    public string Insert(string id, string name, int regionId )
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "INSERT INTO countries (id, name, region_id) VALUES (@id, @name, @regionId);";

        try
        {
            //menambah parameter untuk command
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@name", name));
            command.Parameters.Add(new SqlParameter("@regionId", regionId));
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
                //mereturn nilai dengan menggunakan if condition
                if (result > 0)
                {
                    return "Insert Success";
                }
                return "Insert Failed";
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

    // UPDATE: Country
    //membuat Method Update dan mereturn string
    public string Update(string id, string name, int regionId)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "UPDATE countries SET name = @name, region_id = @regionID  WHERE id = @id;";

        try
        {
            //menambah parameter untuk command
            command.Parameters.Add(new SqlParameter("@name", name));
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@regionId", regionId));
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
                //mereturn nilai dengan menggunakan if condition
                if (result > 0)
                {
                    return "Update Success";
                }
                return "Update Failed";
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

    // DELETE: Country
    //membuat Method Insert dan mereturn string
    public string Delete(string id)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "DELETE FROM countries WHERE id = @id;";

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

                //mereturn nilai dengan menggunakan if condition
                if (result > 0)
                {
                    return "Delete Success";
                }
                return "Delete Failed";
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
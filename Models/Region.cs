using System.Data.SqlClient;
using System.Transactions;
using System.Xml.Linq;
using BasicConnection;

namespace BasicConnection;

public class Region
{
    public int Id { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return $"{Id} - {Name}";
    }


    // GET ALL: Region
    //membuat Method get all dan mereturn list berupa kelas region
    public List<Region> GetAll()
    {
        // mendeklarasi variabel regions dengan list dari kelas region
        var regions = new List<Region>();

        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();

        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();

        //menambahkan transact query ke command
        command.CommandText = "SELECT * FROM regions";

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
                    //membuat dan menambahkan class region dengan konstruktor ke dalam list region
                    regions.Add(new Region
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
                //menutup akses ke data
                reader.Close();
                connection.Close();

                //mereturn list region yang memiliki isi
                return regions;
            }
            //menutup akses ke data
            reader.Close();
            connection.Close();
            //mengembalikan list region kosong
            return new List<Region>();
        }
        //catch error apabila program diatas me throw error
        catch (Exception ex)
        {
            //menampilkan error ke konsol
            Console.WriteLine($"Error: {ex.Message}");
        }
        //mereturn list region kosong
        return new List<Region>();
    }

    // GET BY ID: Region
    //membuat Method get by id dan mereturn list berupa kelas region
    public Region GetById(int id)
    {
        //melakukan instansiasi kelas region
        var region = new Region();
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "SELECT * FROM regions WHERE id = @id";

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
                        //menambahkan isi untuk objek region dengan menggunakan while loop
                        region.Id = reader.GetInt32(0);
                        region.Name = reader.GetString(1);
                    }

                    //menutup akses ke koneksi dan reader
                    reader.Close();
                    connection.Close();
                    //mengembalikan region yang sudah di isi
                    return region;
                }

                //menutup akses ke data
                reader.Close();
                connection.Close();
                //mereturn region kosong
                return region;
            }


            catch (Exception ex)
            {
                //melakukan rollback apabila ada error
                transaction.Rollback();
                //menampilkan error ke konsol
                Console.WriteLine($"Error: {ex.Message}");
            }
        } catch ( Exception ex)
        {
            //menampilkan error ke konsol
            Console.WriteLine($"Error: {ex.Message}");
        }


        //mereturn region kosong
        return region;
    }

    // INSERT: Region
    //membuat Method Insert dan mereturn string
    public string Insert(string name)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "INSERT INTO regions VALUES (@name);";

        try
        {
            //menambah parameter untuk command
            command.Parameters.Add(new SqlParameter("@name", name));
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

    // UPDATE: Region
    //membuat Method Update dan mereturn string
    public string Update(Region region)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "UPDATE regions SET name = @name WHERE id = @id;";

        try
        {
            //menambah parameter untuk command
            command.Parameters.Add(new SqlParameter("@name", region.Name));
            command.Parameters.Add(new SqlParameter("@id", region.Id));
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

    // DELETE: Region
    //membuat Method Insert dan mereturn string
    public string Delete(int id)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "DELETE FROM regions WHERE id = @id;";

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
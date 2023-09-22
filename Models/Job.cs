using System.Data.SqlClient;
using System.Transactions;
using System.Xml.Linq;
using BasicConnection;

namespace BasicConnection;

public class Job
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int? MinSalary { get; set; }
    public int? MaxSalary { get; set; }


    // GET ALL: Job
    //membuat Method get all dan mereturn list berupa kelas job
    public List<Job> GetAll()
    {
        // mendeklarasi variabel jobs dengan list dari kelas job
        var jobs = new List<Job>();

        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();

        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();

        //menambahkan transact query ke command
        command.CommandText = "SELECT * FROM jobs";

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
                    //membuat dan menambahkan class job dengan konstruktor ke dalam list job
                    jobs.Add(new Job
                    {
                        Id = reader.GetString(0),
                        Title = reader.GetString(1),
                        MinSalary = reader.GetInt32(2),
                        MaxSalary = reader.GetInt32(3),
                    });
                }
                //menutup akses ke data
                reader.Close();
                connection.Close();

                //mereturn list job yang memiliki isi
                return jobs;
            }
            //menutup akses ke data
            reader.Close();
            connection.Close();
            //mengembalikan list job kosong
            return new List<Job>();
        }
        //catch error apabila program diatas me throw error
        catch (Exception ex)
        {
            //menampilkan error ke konsol
            Console.WriteLine($"Error: {ex.Message}");
        }
        //mereturn list job kosong
        return new List<Job>();
    }

    // GET BY ID: Job
    //membuat Method get by id dan mereturn list berupa kelas job
    public Job GetById(int id)
    {
        //melakukan instansiasi kelas job
        var job = new Job();
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "SELECT * FROM jobs WHERE id = @id";

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
                        //menambahkan isi untuk objek job dengan menggunakan while loop
                        job.Id = reader.GetString(0);
                        job.Title = reader.GetString(1);
                        job.MinSalary = reader.GetInt32(2);
                        job.MaxSalary = reader.GetInt32(3);
                    }

                    //menutup akses ke koneksi dan reader
                    reader.Close();
                    connection.Close();
                    //mengembalikan job yang sudah di isi
                    return job;
                }

                //menutup akses ke data
                reader.Close();
                connection.Close();
                //mereturn job kosong
                return job;
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


        //mereturn job kosong
        return job;
    }

    // INSERT: Job
    //membuat Method Insert dan mereturn string
    public string Insert(string id, string title, int minSalary, int maxSalary)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "INSERT INTO jobs (id, title, min_salary, max_salary) VALUES (@id, @title, @minSalary , @maxSalary);";

        try
        {
            //menambah parameter untuk command
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@title", title));
            command.Parameters.Add(new SqlParameter("@minSalary", minSalary));
            command.Parameters.Add(new SqlParameter("@maxSalary", maxSalary));
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

    // UPDATE: Job
    //membuat Method Update dan mereturn string
    public string Update(string id, string title, int minSalary, int maxSalary)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "UPDATE jobs SET title = @title, min_salary = @minSalary, max_salary = @maxSalary WHERE id = @id;";

        try
        {
            //menambah parameter untuk command
            command.Parameters.Add(new SqlParameter("@title", title));
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@minSalary", minSalary));
            command.Parameters.Add(new SqlParameter("@maxSalary", maxSalary));
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

    // DELETE: Job
    //membuat Method Insert dan mereturn string
    public string Delete(int id)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "DELETE FROM jobs WHERE id = @id;";

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
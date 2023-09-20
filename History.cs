using System.Data.SqlClient;
using System.Transactions;
using System.Xml.Linq;
using BasicConnection;

namespace BasicConnection;

public class History
{
    public DateTime StartDate { get; set; }
    public int EmployeeId { get; set; }
    public DateTime? EndDate { get; set; }
    public int DepartmentId { get; set; }
    public string jobId { get; set; }

    private readonly string connectionString = "Data Source=DESKTOP-CI3320S;Integrated Security=True;Database=db_hr_dts;Connect Timeout=30;";


    // GET ALL: History
    //membuat Method get all dan mereturn list berupa kelas history
    public List<History> GetAll()
    {
        // mendeklarasi variabel histories dengan list dari kelas history
        var histories = new List<History>();

        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = new SqlConnection(connectionString);

        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();

        //menambahkan transact query ke command
        command.CommandText = "SELECT * FROM histories";

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
                    //membuat dan menambahkan class history dengan konstruktor ke dalam list history
                    histories.Add(new History
                    {
                        StartDate = reader.GetDateTime(2),
                        EmployeeId = reader.GetInt32(3),
                        EndDate = reader.GetDateTime(4),
                        DepartmentId = reader.GetInt32(5),
                        jobId = reader.GetString(6),

                    }); ;
                }
                //menutup akses ke data
                reader.Close();
                connection.Close();

                //mereturn list history yang memiliki isi
                return histories;
            }
            //menutup akses ke data
            reader.Close();
            connection.Close();
            //mengembalikan list history kosong
            return new List<History>();
        }
        //catch error apabila program diatas me throw error
        catch (Exception ex)
        {
            //menampilkan error ke konsol
            Console.WriteLine($"Error: {ex.Message}");
        }
        //mereturn list history kosong
        return new List<History>();
    }

    // GET BY ID: History
    //membuat Method get by id dan mereturn list berupa kelas history
    public History GetById(int id)
    {
        //melakukan instansiasi kelas history
        var history = new History();
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = new SqlConnection(connectionString);
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "SELECT * FROM histories WHERE employee_id = @id";

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
                        //menambahkan isi untuk objek history dengan menggunakan while loop
                        history.StartDate = reader.GetDateTime(0);
                        history.EmployeeId = reader.GetInt32(1);
                        history.EndDate = reader.GetDateTime(2);
                        history.DepartmentId = reader.GetInt32(3);
                        history.jobId = reader.GetString(4);

                    }

                    //menutup akses ke koneksi dan reader
                    reader.Close();
                    connection.Close();
                    //mengembalikan history yang sudah di isi
                    return history;
                }

                //menutup akses ke data
                reader.Close();
                connection.Close();
                //mereturn history kosong
                return history;
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


        //mereturn history kosong
        return history;
    }

    // INSERT: History
    //membuat Method Insert dan mereturn string
    public string Insert(DateTime startDate, int employeeId, DateTime? endDate, int departmentId, string jobId)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = new SqlConnection(connectionString);
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "INSERT INTO histories (start_date, employee_id, end_date, department_id, job_id) VALUES (@StartDate, @EmployeeId, @EndDate, @DepartmentId, @JobId)";

        try
        {
            //menambah parameter untuk command
            command.Parameters.AddWithValue("@startDate", startDate);
            command.Parameters.AddWithValue("@employeeId", employeeId);
            command.Parameters.AddWithValue("@endDate", endDate ?? (object)DBNull.Value); // Handle null endDate
            command.Parameters.AddWithValue("@departmentId", departmentId);
            command.Parameters.AddWithValue("@jobId", jobId);
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

    // UPDATE: History
    //membuat Method Update dan mereturn string
    public string Update(int employeeId, DateTime? endDate)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = new SqlConnection(connectionString);
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "UPDATE histories SET (end_date) VALUES (@endDate) WHERE employee_id = @id;";

        try
        {
            //menambah parameter untuk command
            command.Parameters.AddWithValue("@id", employeeId);
            command.Parameters.AddWithValue("@endDate", endDate ?? (object)DBNull.Value);
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

    // DELETE: History
    //membuat Method Insert dan mereturn string
    public string Delete(int id)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = new SqlConnection(connectionString);
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "DELETE FROM histories WHERE employee_id = @id;";

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
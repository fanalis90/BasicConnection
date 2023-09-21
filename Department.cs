using System.Data.SqlClient;
using System.Transactions;
using System.Xml.Linq;
using BasicConnection;

namespace BasicConnection;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int LocationId { get; set; }
    public int ManagerId { get; set; }

    private readonly string connectionString = "Data Source=DESKTOP-CI3320S;Integrated Security=True;Database=db_hr_dts;Connect Timeout=30;";

    public override string ToString()
    {
        return $"{Id} - {Name} - {LocationId} - {ManagerId} ";
    }


    // GET ALL: Department
    //membuat Method get all dan mereturn list berupa kelas department
    public List<Department> GetAll()
    {
        // mendeklarasi variabel departments dengan list dari kelas department
        var departments = new List<Department>();

        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = new SqlConnection(connectionString);

        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();

        //menambahkan transact query ke command
        command.CommandText = "SELECT * FROM departments";

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
                    //membuat dan menambahkan class department dengan konstruktor ke dalam list department
                    departments.Add(new Department
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        LocationId = reader.GetInt32(2),
                        ManagerId = reader.GetInt32(3),
                });
                }
                //menutup akses ke data
                reader.Close();
                connection.Close();

                //mereturn list department yang memiliki isi
                return departments;
            }
            //menutup akses ke data
            reader.Close();
            connection.Close();
            //mengembalikan list department kosong
            return new List<Department>();
        }
        //catch error apabila program diatas me throw error
        catch (Exception ex)
        {
            //menampilkan error ke konsol
            Console.WriteLine($"Error: {ex.Message}");
        }
        //mereturn list department kosong
        return new List<Department>();
    }

    // GET BY ID: Department
    //membuat Method get by id dan mereturn list berupa kelas department
    public Department GetById(int id)
    {
        //melakukan instansiasi kelas department
        var department = new Department();
       
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = new SqlConnection(connectionString);
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "SELECT * FROM departments WHERE id = @id";

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
                        //menambahkan isi untuk objek department dengan menggunakan while loop
                        department.Id = reader.GetInt32(0);
                        department.Name = reader.GetString(1);
                        department.LocationId = reader.GetInt32(2);
                        department.ManagerId = reader.GetInt32(3);

                    }

                    //menutup akses ke koneksi dan reader
                    reader.Close();
                    connection.Close();
                    //mengembalikan department yang sudah di isi
                    return department;
                }

                //menutup akses ke data
                reader.Close();
                connection.Close();
                //mereturn department kosong
                return department;
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


        //mereturn department kosong
        return department;
    }

    // INSERT: Department
    //membuat Method Insert dan mereturn string
    public string Insert(int id, string name, int locationId, int? managerId)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = new SqlConnection(connectionString);
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "INSERT INTO departments (id, name, location_id, manager_id) VALUES (@id, @name, @locationId, @managerId);";

        try
        {
            //menambah parameter untuk command
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@name", name));
            command.Parameters.Add(new SqlParameter("@locationId", locationId));
            command.Parameters.Add(new SqlParameter("@managerId", managerId ?? (object)DBNull.Value));
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

    // UPDATE: Department
    //membuat Method Update dan mereturn string
    public string Update(int id, string name, int locationId, int managerId)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = new SqlConnection(connectionString);
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "UPDATE departments SET name = @name, location_id = @locationId, manager_id = @managerId WHERE id = @id;";

        try
        {
            //menambah parameter untuk command
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@name", name));
            command.Parameters.Add(new SqlParameter("@locationId", locationId));
            command.Parameters.Add(new SqlParameter("@managerId", managerId));

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

    // DELETE: Department
    //membuat Method Insert dan mereturn string
    public string Delete(int id)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = new SqlConnection(connectionString);
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "DELETE FROM departments WHERE id = @id;";

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
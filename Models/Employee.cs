using System.Data.SqlClient;
using System.Transactions;
using System.Xml.Linq;
using BasicConnection;

namespace BasicConnection;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime HireDate { get; set; }
    public int Salary { get; set; }
    public decimal CommissionPct { get; set; }
    public int? ManagerId { get; set; }
    public string JobId { get; set; }
    public int DepartmentId { get; set; }

    public override string ToString()
    {
        return $"{Id} - {HireDate} - {FirstName} - {LastName} - {Email} - {PhoneNumber}";
    }


    // GET ALL: Employee
    //membuat Method get all dan mereturn list berupa kelas employee
    public List<Employee> GetAll()
    {
        // mendeklarasi variabel employees dengan list dari kelas employee
        var employees = new List<Employee>();

        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();

        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();

        //menambahkan transact query ke command
        command.CommandText = "SELECT * FROM employees";

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
                    //membuat dan menambahkan class employee dengan konstruktor ke dalam list employee
                    employees.Add(new Employee
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.IsDBNull(2) ? null : reader.GetString(2),
                        Email = reader.GetString(3),
                        PhoneNumber = reader.IsDBNull(4) ? null : reader.GetString(4),
                        HireDate = reader.GetDateTime(5),
                        Salary = reader.GetInt32(6),
                        CommissionPct = reader.GetDecimal(7),
                        ManagerId = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8),
                        JobId = reader.GetString(9),
                        DepartmentId = reader.GetInt32(10)
                    });
                }
                //menutup akses ke data
                reader.Close();
                connection.Close();

                //mereturn list employee yang memiliki isi
                return employees;
            }
            //menutup akses ke data
            reader.Close();
            connection.Close();
            //mengembalikan list employee kosong
            return new List<Employee>();
        }
        //catch error apabila program diatas me throw error
        catch (Exception ex)
        {
            //menampilkan error ke konsol
            Console.WriteLine($"Error: {ex.Message}");
        }
        //mereturn list employee kosong
        return new List<Employee>();
    }

    // GET BY ID: Employee
    //membuat Method get by id dan mereturn list berupa kelas employee
    public Employee GetById(int id)
    {
        //melakukan instansiasi kelas employee
        var employee = new Employee();
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "SELECT * FROM employees WHERE id = @id";

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
                        //menambahkan isi untuk objek employee dengan menggunakan while loop
                        employee.Id = reader.GetInt32(0);
                        employee.FirstName = reader.GetString(1);
                        employee.LastName = reader.IsDBNull(2) ? null : reader.GetString(2);
                        employee.Email = reader.GetString(3);
                        employee.PhoneNumber = reader.IsDBNull(4) ? null : reader.GetString(4);
                        employee.HireDate = reader.GetDateTime(5);
                        employee.Salary = reader.GetInt32(6);
                        employee.CommissionPct = reader.GetDecimal(7);
                        employee.ManagerId = reader.IsDBNull(8) ? null : (int?)reader.GetInt32(8);
                        employee.JobId = reader.GetString(9);
                        employee.DepartmentId = reader.GetInt32(10);
                    }

                    //menutup akses ke koneksi dan reader
                    reader.Close();
                    connection.Close();
                    //mengembalikan employee yang sudah di isi
                    return employee;
                }

                //menutup akses ke data
                reader.Close();
                connection.Close();
                //mereturn employee kosong
                return employee;
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


        //mereturn employee kosong
        return employee;
    }

    // INSERT: Employee
    //membuat Method Insert dan mereturn string
    public string Insert(Employee employee)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "INSERT INTO employees (id, first_name, last_name, email, phone_number, hire_date, salary, commission_pct, manager_id, job_id, department_id) VALUES (@id, @firstName, @lastName, @email, @phoneNumber, @hireDate, @salary, @commissionPct, @managerId, @jobId, @departmentId);";

        try
        {
            //menambah parameter untuk command
            command.Parameters.Add(new SqlParameter("@id", employee.Id));
            command.Parameters.Add(new SqlParameter("@firstName", employee.FirstName));
            command.Parameters.Add(new SqlParameter("@lastName", employee.LastName ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@email", employee.Email));
            command.Parameters.Add(new SqlParameter("@phoneNumber", employee.PhoneNumber ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@hireDate", employee.HireDate));
            command.Parameters.Add(new SqlParameter("@salary", employee.Salary));
            command.Parameters.Add(new SqlParameter("@commissionPct", employee.CommissionPct));
            command.Parameters.Add(new SqlParameter("@managerId", employee.ManagerId ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@jobId", employee.JobId));
            command.Parameters.Add(new SqlParameter("@departmentId", employee.DepartmentId));

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

    // UPDATE: Employee
    //membuat Method Update dan mereturn string
    public string Update(Employee employee)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "UPDATE employees SET first_name = @firstName, last_name = @lastName, email = @email, phone_number = @phoneNumber, hire_date = @hireDate, salary = @salary, commission_pct = @commissionPct, manager_id = @managerId, job_id = @jobId, department_id = @departmentId WHERE id = @id;";

        try
        {
            //menambah parameter untuk command
            command.Parameters.Add(new SqlParameter("@firstName", employee.FirstName));
            command.Parameters.Add(new SqlParameter("@lastName", employee.LastName ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@email", employee.Email));
            command.Parameters.Add(new SqlParameter("@phoneNumber", employee.PhoneNumber ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@hireDate", employee.HireDate));
            command.Parameters.Add(new SqlParameter("@salary", employee.Salary));
            command.Parameters.Add(new SqlParameter("@commissionPct", employee.CommissionPct));
            command.Parameters.Add(new SqlParameter("@managerId", employee.ManagerId ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@jobId", employee.JobId));
            command.Parameters.Add(new SqlParameter("@departmentId", employee.DepartmentId));
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

    // DELETE: Employee
    //membuat Method Insert dan mereturn string
    public string Delete(int id)
    {
        //koneksi ke database menggunakan library sqlconnection dengan argument connectionstring
        using var connection = Provider.GetConnection();
        // membuat command dengan method createcommand dari connection
        using var command = connection.CreateCommand();
        //menambahkan transact query ke command
        command.CommandText = "DELETE FROM employees WHERE id = @id;";

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
using System;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace BasicConnection
{
    public class Program
    {   
        // membuat string untuk configurasi koneksi ke database
        static string connectionString = "Data Source=DESKTOP-CI3320S;Integrated Security=True;Database=db_hr_dts;Connect Timeout=30;";
        static SqlConnection? connection;
        private static void Main()
        {
            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Connection Open Successfully");
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static int InsertRegions(string name)
        {
            //koneksi ke database menggunakan libray dengan inputan argumen connectionstring
            connection = new SqlConnection(connectionString);
            //membuat objek command dari kelas Sqlcommand
            SqlCommand command = new SqlCommand();
            //membuat koneksi command dengan objek connection dari kelas sqlconnect 
            command.Connection = connection;

            // text query untuk create
            command.CommandText = "INSERT INTO regions VALUES(@name)";
            try
            {
                //membuat parameter pName dengan kelas sqlParameter
                var pName = new SqlParameter();
                pName.ParameterName = "@name";
                // tipe data dari database di cocokan dengan parameter
                pName.SqlDbType = SqlDbType.VarChar;
                //value yang di input 
                pName.Value = name;

                //menambahkan parameter pada objek command dengan method add di isi objek pname
                command.Parameters.Add(pName);

                //memulai koneksi ke database dengan method open 
                connection.Open();
                // begin transaksi untuk memulai transaksi
                using var transaction = connection.BeginTransaction();
                try
                {
                    command.Transaction = transaction;
                    //execute text query dengan method non query yang akan mereturn int 
                    var result = command.ExecuteNonQuery();
                    // melaksanakan transaksi dengan method commit
                    transaction.Commit();
                    //menutup koneksi ke database
                    connection.Close();

                    //alur dimana berhasil atau tidaknya query di eksekusi berdasarkan return dari method non query
                    switch (result)
                    {
                        case >= 1:
                            Console.WriteLine("Insert Success");
                            break;
                        default:
                            Console.WriteLine("Insert Failed");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    //menjalankan rollback apabila error terjadi dan menampilkan pesan error
                    transaction.Rollback();
                    Console.WriteLine($"Error : {ex.Message}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
            }
            return 0;
        }
        public static void GetAllRegions()
        {
            //koneksi ke database menggunakan libray dengan inputan argumen connectionstring
            connection = new SqlConnection(connectionString);
            //membuat objek command dari kelas Sqlcommand
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            //membuat koneksi command dengan objek connection dari kelas sqlconnect
            command.CommandText = "SELECT * FROM regions";

            try
            {
                connection.Open();
                //menggunakan execute reader untuk mendapat return berupa objek
                SqlDataReader reader = command.ExecuteReader();
                //apabila data memiliki isi
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Id : {reader.GetInt32(0)}");
                        Console.WriteLine($"Id : {reader.GetString(1)}");
                    }
                }
                else
                {
                    Console.WriteLine("no rows found");
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
            }
        }

        public static void GetRegionById(int id)
        {
            //koneksi ke database menggunakan libray dengan inputan argumen connectionstring
            connection = new SqlConnection(connectionString);
            //membuat objek command dari kelas Sqlcommand
            SqlCommand command = new SqlCommand();
            // text query untuk getregion by id
            command.CommandText = "SELECT * FROM regions WHERE id= @id";
            //membuat koneksi command dengan objek connection dari kelas sqlconnect
            command.Connection = connection;

            var pId = new SqlParameter();
            pId.ParameterName = "@id";
            pId.Value = id;
            // mencocokan tipe data sesuai dengan database
            pId.SqlDbType = SqlDbType.Int;

            command.Parameters.Add(pId);

            connection.Open();
            var transaction = connection.BeginTransaction();

            try
            {
                command.Transaction = transaction;
                //menggunakan execute reader untuk mendapat return berupa objek 
                var reader = command.ExecuteReader();
                // menggunakan if untuk mengecek ketika reader memiliki isi atau tidak
                if (reader.HasRows)
                {
                    //menggunakan while untuk menampilkan semua data yang ada dalam reader dengan method read
                    while (reader.Read())
                    {
                        Console.WriteLine($"Id : {reader.GetInt32(0)}");
                        Console.WriteLine($"Name : {reader.GetString(1)}");
                    }
                }
                else
                {
                    //ketika reader tidak memiliki data
                    Console.WriteLine($"tidak ada data ditemukan dengan id = {id}");
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
            }
        }

        public static int UpdateRegionById(int id, string name)
        {
            //koneksi ke database menggunakan libray dengan inputan argumen connectionstring
            connection = new SqlConnection(connectionString);
            //membuat objek command dari kelas Sqlcommand
            SqlCommand command = new SqlCommand();
            command.CommandText = "UPDATE regions SET name = @nama WHERE id = @id";
            //membuat koneksi command dengan objek connection dari kelas sqlconnect
            command.Connection = connection;
            try
            {
                //mengkonfigurasi parameter id
                var pId = new SqlParameter();
                pId.ParameterName = "@id";
                pId.Value = id;
                pId.SqlDbType = SqlDbType.Int;
                command.Parameters.Add(pId);

                //mengkonfigurasi parameter name
                var pName = new SqlParameter();
                pName.ParameterName = "@nama";
                pName.Value = name;
                pName.SqlDbType = SqlDbType.VarChar;
                command.Parameters.Add(pName);

                connection.Open();
                using var transaction = connection.BeginTransaction();

                try
                {
                    command.Transaction = transaction;
                    //execute non query untuk mendapat return int untuk mengecek keberhasil transaksi
                    var result = command.ExecuteNonQuery();

                    transaction.Commit();
                    connection.Close();
                    //mengecek keberhasilan update dengan return dari methode nonquery
                    switch (result)
                    {
                        case >= 1:
                            Console.WriteLine("Update Success");
                            break;
                        default:
                            Console.WriteLine("Update Failed");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return 0;
        }

        public static int DeleteRegionById(int id)
        {
            //koneksi ke database menggunakan libray dengan inputan argumen connectionstring
            connection = new SqlConnection(connectionString);
            //membuat objek command dari kelas Sqlcommand
            SqlCommand command = new SqlCommand();
            //membuat koneksi command dengan objek connection dari kelas sqlconnect
            command.Connection = connection;
            command.CommandText = "DELETE FROM regions WHERE id = @id";
            try
            {
                //konfigurasi parameter id
                var pId = new SqlParameter();
                pId.ParameterName = "@id";
                pId.Value = id;
                pId.SqlDbType = SqlDbType.Int;

                command.Parameters.Add(pId);
                //koneksi ke database
                connection.Open();
                var transaction = connection.BeginTransaction();
                try
                {
                    command.Transaction = transaction;
                    // mengasign pengembalian dari eksekusi method non query
                    var result = command.ExecuteNonQuery();
                    transaction.Commit();
                    connection.Close();
                    //mengecek keberhasilan transaksi delete
                    switch (result)
                    {
                        case >= 1:
                            Console.WriteLine("Data berhasil dihapus");
                            break;
                        default:
                            Console.WriteLine("gagal dihapus");
                            break;
                    }


                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"Error : {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
            }
            return 0;
        }


    }


}


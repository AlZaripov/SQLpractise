using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace MSsqlConsoleApp
{
    class Database
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["StudentsDB"].ConnectionString;
        public SqlConnection SqlConnection { get; }
        private SqlDataReader reader;
        private SqlCommand command;

        public Database()
        {
            SqlConnection = new SqlConnection(connectionString);
            SqlConnection.Open();
        }

        public void GetConnectionInfo(Action<string[]> message)
        {
            message(new string[]
            {
                $"Connection information:",
                $"\tConnection string: {SqlConnection.ConnectionString}",
                $"\tDatabase: {SqlConnection.Database}",
                $"\tServer: {SqlConnection.DataSource}",
                $"\tServer version: {SqlConnection.ServerVersion}",
                $"\tConnection state: {SqlConnection.State}",
            });
        }

        public void Select(string query, Action<string[]> message)
        {
            command = new SqlCommand(query, SqlConnection);
            try
            {
                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    var list = new List<string>();
                    list.Add(string.Format("{0,-5} {1,-15} {2,-20} {3,-15} {4,-15} {5,-15} {6,-15}",
                           "Id", "FIO", "Birthday", "Institute", "GroupNumber", "Course", "AverageMark"));

                    while (reader.Read())
                    {
                        list.Add(string.Format("{0,-5} {1,-15} {2,-20} {3,-15} {4,-15} {5,-15} {6,-15}",
                            reader["Id"], reader["FIO"], reader["Birthday"], reader["Institute"], reader["GroupNumber"], reader["Course"], reader["AverageMark"]));
                    }
                    message(list.ToArray());
                }
                else
                    message(new string[] { "There is no matches" });
            }
            catch (Exception ex)
            {
                message( new string[] { ex.Message });
            }
            finally
            {
                if(reader!=null)
                    reader.Close();
            }
        }

        public void Delete(string query, Action<string> message)
        {
            command = new SqlCommand(query, SqlConnection);
            try
            {
                message($"Количество удаленных записей: {command.ExecuteNonQuery()}");
            }
            catch (Exception ex)
            {
                message(ex.Message);
            }
        }

        public void Update(string query, Action<string> message)
        {
            command = new SqlCommand(query, SqlConnection);
            try
            {
                message($"Количество измененных строк: {command.ExecuteNonQuery()}");
            }
            catch (Exception ex)
            {
                message(ex.Message);
            }

        }

        public void Insert(string query, Action<string> message)
        {
            command = new SqlCommand(query, SqlConnection);
            try
            {
                message($"Количество добавленных строк: {command.ExecuteNonQuery()}");
            }
            catch (Exception ex)
            {
                message(ex.Message);
            }

        }

        public void Exit()
        {
            if (reader != null)
                reader.Close();
            if (SqlConnection.State == ConnectionState.Open)
                SqlConnection.Close();
            Environment.Exit(0);
        }

        public static void GetHelp(Action<string[]> print)
        {
            print(new[]
            {
                "Supported comands :",
                "\t - info: get information about connection",
                "\t - help: get information about supported commands",
                "\t - exit: close application",
                "\t - clear: clear console screen",
                new string("\n\tComands to working with \"Students\" table"),
                "\t - select: select data from database",
                "\t - insert: add new record to database",
                "\t - delete: remove record from database",
                "\t - update: update data in database"
            });
        }

    }
}

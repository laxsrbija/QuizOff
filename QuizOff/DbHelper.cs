using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace QuizOff
{

    /// <summary>
    /// Helper class for communicating with the database
    /// </summary>
    class DbHelper : IDisposable
    {

        private MySqlConnection connection;

        /// <summary>
        /// DbHelper constructor
        /// </summary>
        public DbHelper()
        {

            var connectionString = "server=" + Constants.Db.DB_HOST + ";user=" + Constants.Db.DB_USER 
                + ";database=" + Constants.Db.DB_NAME + ";port=" + Constants.Db.DB_PORT + ";password=" + Constants.Db.DB_PASSWORD;

            connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        /// <summary>
        /// Executes the given query on a database
        /// </summary>
        /// <param name="query">MySQL query</param>
        /// <param name="parameters">Parameter dictionary</param>
        /// <returns>Reader of the resulting data set</returns>
        private MySqlDataReader FetchDataFromQuery(string query, Dictionary<string, string> parameters)
        {

            MySqlCommand command;

            if (parameters != null)
            {
                command = new MySqlCommand();
                command.Connection = connection;

                command.CommandText = query;
                command.Prepare();

                foreach (var key in parameters.Keys)
                {
                    command.Parameters.AddWithValue(key, parameters[key]);
                }

            } else
            {
                command = new MySqlCommand(query, connection);
            }

            return command.ExecuteReader();

        }

        /// <summary>
        /// Prepares and returns the nonexecuted command
        /// </summary>
        /// <param name="query">MySQL query</param>
        /// <param name="parameters">Parameter dictionary</param>
        /// <returns>Nonexecuted command</returns>
        private MySqlCommand PrepareNonExecuteCommand(string query, Dictionary<string, string> parameters)
        {

            MySqlCommand command;

            if (parameters != null)
            {
                command = new MySqlCommand();
                command.Connection = connection;

                command.CommandText = query;
                command.Prepare();

                foreach (var key in parameters.Keys)
                {
                    command.Parameters.AddWithValue(key, parameters[key]);
                }

            }
            else
            {
                command = new MySqlCommand(query, connection);
            }

            return command;

        }

        /// <summary>
        /// Fetches data from the database.
        /// </summary>
        /// <param name="columns">An array of columns to select from the database</param>
        /// <param name="query">Rest of the query string</param>
        /// <param name="parameters">Parameter dictionary</param>
        /// <returns>List of dictionaries. Each dictionary contains key-value pairs of data from a single row</returns>
        public List<Dictionary<string, object>> SelectMultipleRows(string[] columns, string query, Dictionary<string, string> parameters = null)
        {

            var completeQuery = "select " + String.Join(", ", columns) + " " + query;
            var reader = FetchDataFromQuery(completeQuery, parameters);
            var list = new List<Dictionary<string, object>>();

            try
            {
                while (reader.Read())
                {

                    var row = new Dictionary<string, object>();

                    for (var i = 0; i < columns.Length; i++)
                    {
                        row[columns[i]] = reader[i];
                    }

                    list.Add(row);

                }
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return list;

        }

        /// <summary>
        /// Returns a single value from a database
        /// </summary>
        /// <param name="query">MySQL query</param>
        /// <param name="parameters">Parameter dictionary</param>
        /// <returns>Resulting object</returns>
        public object SelectSingleObject(string query, Dictionary<string, string> parameters = null)
        {

            var reader = FetchDataFromQuery(query, parameters);

            try
            {
                if (reader.Read())
                {
                    return reader[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return null;

        }

        /// <summary>
        /// Inserts data into the database
        /// </summary>
        /// <param name="query">MySQL query</param>
        /// <param name="parameters">Parameter dictionary</param>
        /// <returns>ID of the last inserted row</returns>
        public long Insert(string query, Dictionary<string, string> parameters = null)
        {
            var command = PrepareNonExecuteCommand(query, parameters);
            command.ExecuteNonQuery();
            return command.LastInsertedId;
        }

        /// <summary>
        /// Updates data from te database
        /// </summary>
        /// <param name="query">MySQL query</param>
        /// <param name="parameters">Parameter dictionary</param>
        /// <returns>Number of updated rows</returns>
        public long Update(string query, Dictionary<string, string> parameters = null)
        {
            var command = PrepareNonExecuteCommand(query, parameters);
            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Called before disposing of the object. 
        /// Closes the database connection.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Closes the database connection
        /// </summary>
        public void Close()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }

    }
}

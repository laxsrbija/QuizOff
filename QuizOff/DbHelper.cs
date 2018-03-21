using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace QuizOff
{
    class DbHelper
    {

        private string connectionString;
        private MySqlConnection connection;

        public DbHelper()
        {

            connectionString = "server=" + Constants.Db.DB_HOST + ";user=" + Constants.Db.DB_USER 
                + ";database=" + Constants.Db.DB_NAME + ";port=" + Constants.Db.DB_PORT + ";password=" + Constants.Db.DB_PASSWORD;

            connection = new MySqlConnection(connectionString);

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                connection.Open();

                string sql = "SELECT idtest, testcol FROM test";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Console.WriteLine(rdr[0] + " -- " + rdr[1]);
                }

                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            
            Console.WriteLine("Done.");

        }

        // TODO: vratiti List<Dictionary<string, string>> nakon selecta sa vise polja i redova i samo string kada se ocekuje jedna vrednost
        //public List<>

    }
}

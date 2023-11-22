using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KistPack
{
    internal class KistPackDB
    {

        private static string connString = Properties.Settings.Default.KistPackDB;


        public  string getKistPackDBVersion()
        {
            string dbVersion = null;

            try
            {
                //sql connection object
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    //retrieve the SQL Server instance version
                    string query = @"SELECT value
                                     FROM Settings
                                     where Setting = 'DBVersion';";
                    //define the SqlCommand object
                    SqlCommand cmd = new SqlCommand(query, conn);

                    //open connection
                    conn.Open();

                    //execute the SQLCommand
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        dbVersion = dr.GetString(0);
                    }
                    dr.Close();

                    conn.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Verbinden zur KistPackDB: " + Environment.NewLine +  ex.Message, "Error");
                
            }
            finally
            {

            }


            return dbVersion;

        }

    

    }
}

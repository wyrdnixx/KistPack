using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
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


        public bool testInsertDb(String _charge, String _kiste, String _fallnummer, String _person, String _vorname, String _nachname, String _clientname, String _hostname)
        {
            Boolean result = false;
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
                    var value = Environment.GetEnvironmentVariable("USERNAME");



                    //open connection                    
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = "INSERT INTO Chargen (" +
                        "[Charge],[Kiste],[Fallnummer],[Person],[Vorname],[Nachname],[Scandatum],[Scanuser],[Scanclient],[Scanhostname]" +
                        ") VALUES ('" +
                        _charge + "','" +
                        _kiste + "','" +
                        _fallnummer + "','" +
                        _person + "','" +
                        _vorname + "','" +
                        _nachname + "','" +
                        DateTime.Now.ToString() + "','" +
                        Environment.GetEnvironmentVariable("USERNAME") + "','" +
                        Environment.GetEnvironmentVariable("CLIENTNAME") + "','" +
                        Environment.GetEnvironmentVariable("COMPUTERNAME") + "')";


                    //MessageBox.Show(cmd.CommandText, "info");

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Speichern in KistPackDB: " + Environment.NewLine + ex.Message, "Error");

            }

            return result;


        }

    }
}

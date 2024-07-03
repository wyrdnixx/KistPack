using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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

            return dbVersion;

        }

        /// <summary>
        /// hole verfügbare Merkmale aus der KistPackDB-Konfiguration
        /// </summary>
        /// <returns></returns>
        public string getKistPackDBMerkmale()
        {
            string merkmale= null;

            try
            {
                //sql connection object
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    //retrieve the SQL Server instance version
                    string query = @"SELECT value
                                     FROM Settings
                                     where Setting = 'Merkmale';";
                    //define the SqlCommand object
                    SqlCommand cmd = new SqlCommand(query, conn);

                    //open connection
                    conn.Open();

                    //execute the SQLCommand
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        merkmale = dr.GetString(0);
                    }
                    dr.Close();

                    conn.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Verbinden zur KistPackDB: " + Environment.NewLine + ex.Message, "Error");

            }

            return merkmale;

        }


        public List<PatientVisit> searchPat(String _Fallnummer)
        {
            List<PatientVisit> patList = new List<PatientVisit>();
            try
            {
                //sql connection object
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    //retrieve the SQL Server instance version
                    string query = @"SELECT *
                                     FROM Chargen
                                     where Fallnummer = '" + _Fallnummer + "';";
                    //define the SqlCommand object
                    SqlCommand cmd = new SqlCommand(query, conn);

                    //open connection
                    conn.Open();

                    //execute the SQLCommand
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        PatientVisit tmp = new PatientVisit(dr.GetString(1), dr.GetString(2), dr.GetString(3), dr.GetString(4), dr.GetString(5), dr.GetString(6), dr.GetString(7), dr.GetString(8), dr.GetString(9), dr.GetString(10), dr.GetString(11), dr.GetString(12));
                        patList.Add(tmp);
                    }
                    dr.Close();

                    conn.Close();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Verbinden zur KistPackDB: " + Environment.NewLine + ex.Message, "Error");

            }
            return patList;

        }

        public DataTable searchWildcard(String _SearchText)
        {
            DataTable sDT = new DataTable();
            List<String> chargeList = new List<String>();
            // Datentabelle konfigurieren
            sDT.Columns.Add("Charge");
            sDT.Columns.Add("Kiste");
            sDT.Columns.Add("Merkmal");
            sDT.Columns.Add("Fallnummer");
            sDT.Columns.Add("Person");
            sDT.Columns.Add("Gebdat");
            sDT.Columns.Add("Vorname");
            sDT.Columns.Add("Nachname");
            sDT.Columns.Add("Scanndatum");
            sDT.Columns.Add("Scanuser");
            sDT.Columns.Add("Scanclient");
            sDT.Columns.Add("Scanhostname");


            try
            {
                // Search for Charge containing the searchtext
                using (SqlConnection conn = new SqlConnection(connString))
                {                    
                    string query = @"SELECT Charge
                                     FROM Chargen
                                     where Charge like '%" + _SearchText + "%'" +
                                     "or Kiste like '%" + _SearchText + "%'" +
                                     "or Fallnummer like '%" + _SearchText + "%'" +
                                     "or Person like '%" + _SearchText + "%'" +
                                     "or Gebdat like '%" + _SearchText + "%'" +
                                     "or Vorname like '%" + _SearchText + "%'" +
                                     "or Nachname like '%" + _SearchText + "%'" +
                                     "or Scandatum like '%" + _SearchText + "%'" +
                                     "group by Charge;";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();                    
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        //PatientVisit tmp = new PatientVisit(dr.GetString(1), dr.GetString(2), dr.GetString(3), dr.GetString(4), dr.GetString(5), dr.GetString(6), dr.GetString(7), dr.GetString(8), dr.GetString(9), dr.GetString(10));
                        //MessageBox.Show("Found Charge: "+ dr.GetString(0), "Found");
                        chargeList.Add(dr.GetString(0));
                    }
                    dr.Close();
                    conn.Close();
                }

                if (chargeList.Count > 0)
                {


                    // Search for Charge containing the searchtext
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        string chargeIn = null;
                        int chargeCount = chargeList.Count;
                        int counter = 0;
                        foreach (String item in chargeList)
                        {
                            counter++;
                            if (counter < chargeCount)
                            {
                                chargeIn += "'" + item + "',";
                            }
                            else
                            {
                                chargeIn += "'" + item + "'";
                            }

                        }
                        // MessageBox.Show(chargeIn);
                        string query = @"SELECT *
                                     FROM Chargen
                                     where Charge in (" + chargeIn + ");";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        conn.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            //PatientVisit tmp = new PatientVisit(dr.GetString(1), dr.GetString(2), dr.GetString(3), dr.GetString(4), dr.GetString(5), dr.GetString(6), dr.GetString(7), dr.GetString(8), dr.GetString(9), dr.GetString(10));
                            //MessageBox.Show("Found Charge: "+ dr.GetString(0), "Found");
                            //chargeList.Add(dr.GetString(0));
                            sDT.Rows.Add(dr.GetString(1), dr.GetString(2), dr.GetString(3), dr.GetString(4), dr.GetString(5), dr.GetString(6), dr.GetString(7), dr.GetString(8), dr.GetString(9), dr.GetString(10), dr.GetString(11), dr.GetString(12));
                        }
                        dr.Close();
                        conn.Close();
                    }
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Suchen:" + Environment.NewLine + ex.Message,"Error");
            }


            return sDT;
        }


        public bool saveDtToDB(DataTable _dt)
        {

            Boolean result = false;

            String datum = DateTime.Now.ToString();
            String username = Environment.GetEnvironmentVariable("USERNAME");
            String clientname = Environment.GetEnvironmentVariable("CLIENTNAME");
            String computername = Environment.GetEnvironmentVariable("COMPUTERNAME");

            //sql connection object
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                   // Perform transactional operations here
                   SqlCommand cmd = new SqlCommand();
                   cmd.Connection = conn;
                   cmd.Transaction = transaction;
                   try
                   {
                        foreach (DataRow row in _dt.Rows)
                        {
                            cmd.CommandText = "INSERT INTO Chargen (" +
                                "[Charge],[Kiste],[Merkmal],[Fallnummer],[Person],[Gebdat],[Vorname],[Nachname],[Scandatum],[Scanuser],[Scanclient],[Scanhostname]" +
                                ") VALUES ('" +
                                row[0].ToString() + "','" +
                                row[1].ToString() + "','" +
                                row[2].ToString() + "','" +
                                row[3].ToString() + "','" +
                                row[4].ToString() + "','" +
                                row[5].ToString() + "','" +
                                row[6].ToString() + "','" +
                                row[7].ToString() + "','" +
                                datum + "','" +
                                username + "','" +
                                clientname+ "','" +
                                computername+ "')";

                            //MessageBox.Show(cmd.CommandText, "info");                        
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        result = true;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fehler beim Speichern in Datenbank: " + Environment.NewLine + ex.Message, "Error");
                    }




                }
            }
                return result;
        }


        public bool testInsertDbTransaction(String _charge, String _kiste, String _fallnummer, String _person, String _vorname, String _nachname, String _clientname, String _hostname)
        {
            Boolean result = false;

            //sql connection object
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Perform transactional operations here
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.Transaction=transaction;

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
                        cmd.ExecuteNonQuery();

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
                        cmd.ExecuteNonQuery();


                        transaction.Commit();
                        result = true;
                        
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception and roll back the transaction
                        transaction.Rollback();
                        MessageBox.Show("error: " + ex.Message, "Error");
                        return false;
                    }
                }
            }
            return result;

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


        public  Boolean databaseFilePut(string _charge, string _varFilePath)
        {
            byte[] file;
            Boolean saved = false;

            try
            {


                using (var stream = new FileStream(_varFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        file = reader.ReadBytes((int)stream.Length);
                    }
                }
                using (SqlConnection conn = new SqlConnection(connString))
                using (var sqlWrite = new SqlCommand("INSERT INTO ChargenPDF (Charge,Data) Values(@Charge,@File)", conn))
                {
                    sqlWrite.Parameters.Add("@Charge", SqlDbType.VarChar).Value = _charge;
                    sqlWrite.Parameters.Add("@File", SqlDbType.VarBinary, file.Length).Value = file;                    
                    conn.Open();
                    sqlWrite.ExecuteNonQuery();
                    conn.Close();
                    saved = true;
                }
            }catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Speichern der PDF Datei in die Datenbank." + Environment.NewLine + ex.Message);
                   
            }
            return saved;
            
        }

        public Boolean databaseFileRead(string _charge, string varPathToNewLocation)
        {
            

            try
            {


                using (SqlConnection conn = new SqlConnection(connString))
                using (var sqlQuery = new SqlCommand(@"SELECT [Data] FROM [dbo].[ChargenPDF] WHERE [Charge] = @Charge", conn))
                {
                    sqlQuery.Parameters.AddWithValue("@Charge", _charge);
                    conn.Open();
                    using (var sqlQueryResult = sqlQuery.ExecuteReader())
                    if (sqlQueryResult != null)
                        {
                            sqlQueryResult.Read();
                            var blob = new Byte[(sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue))];
                            sqlQueryResult.GetBytes(0, 0, blob, 0, blob.Length);
                            using (var fs = new FileStream(varPathToNewLocation, FileMode.Create, FileAccess.Write))
                                fs.Write(blob, 0, blob.Length);
                        }
                    conn.Close();
                }
            }catch (Exception ex) {
                MessageBox.Show("Fehler beim Laden der PDF Datei aus der Datenbank." + Environment.NewLine + ex.Message);
                return false;
            }
            return true;
        }
    }
}

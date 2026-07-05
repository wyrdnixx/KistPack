using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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

        public string getKistPackDBVersion()
        {
            string dbVersion = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = @"SELECT value FROM Settings WHERE Setting = 'DBVersion'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                                dbVersion = dr.GetString(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Verbinden zur KistPackDB: " + Environment.NewLine + ex.Message, "Error");
            }
            return dbVersion;
        }

        /// <summary>
        /// hole verfügbare Merkmale aus der KistPackDB-Konfiguration
        /// </summary>
        public string getKistPackDBMerkmale()
        {
            string merkmale = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = @"SELECT value FROM Settings WHERE Setting = 'Merkmale'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                                merkmale = dr.GetString(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Verbinden zur KistPackDB: " + Environment.NewLine + ex.Message, "Error");
            }
            return merkmale;
        }

        public string getKistPackDBExternalArchiveCall()
        {
            string externalArchiveCall = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = @"SELECT value FROM Settings WHERE Setting = 'ExternalArchiveCall'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                                externalArchiveCall = dr.GetString(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Verbinden zur KistPackDB: " + Environment.NewLine + ex.Message, "Error");
            }
            return externalArchiveCall;
        }

        public List<PatientVisit> searchPat(string _Fallnummer)
        {
            List<PatientVisit> patList = new List<PatientVisit>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = @"SELECT TOP (50) [Charge],[Kiste],[Merkmal],[Fallnummer],[Person],[Gebdat],[Vorname],[Nachname],[Scandatum],[Scanuser],[Scanclient],[Scanhostname] FROM Chargen WHERE Fallnummer = @Fallnummer";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Fallnummer", _Fallnummer);
                        conn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            string S(int i) => dr.IsDBNull(i) ? "" : dr.GetString(i);
                            while (dr.Read())
                            {
                                PatientVisit tmp = new PatientVisit(
                                    S(0), S(1),  S(2),  S(3),  S(4),  S(5),
                                    S(6), S(7),  S(8),  S(9),  S(10), S(11));
                                patList.Add(tmp);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Verbinden zur KistPackDB: " + Environment.NewLine + ex.Message, "Error");
            }
            return patList;
        }

        public DataTable searchWildcard(string _SearchText)
        {
            DataTable sDT = new DataTable();
            sDT.Columns.Add("Charge");
            sDT.Columns.Add("Kiste");
            sDT.Columns.Add("Merkmal");
            sDT.Columns.Add("Fallnummer");
            sDT.Columns.Add("Person");
            sDT.Columns.Add("Gebdat");
            sDT.Columns.Add("Vorname");
            sDT.Columns.Add("Nachname");
            sDT.Columns.Add("Scandatum");
            sDT.Columns.Add("Scanuser");
            sDT.Columns.Add("Scanclient");
            sDT.Columns.Add("Scanhostname");

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    // Sucht alle Zeilen, deren Charge mindestens eine Übereinstimmung hat.
                    // Zusammengeführt aus zwei früheren Queries, um N+1-Datenbankzugriffe zu vermeiden.
                    // Äußeres TOP entfernt: der innere DISTINCT TOP (100) begrenzt bereits auf 100 Chargen.
                    // Ein äußeres TOP würde Chargen in der Mitte abschneiden und unvollständige Ergebnisse liefern.
                    string query = @"SELECT [Charge],[Kiste],[Merkmal],[Fallnummer],[Person],
                                         [Gebdat],[Vorname],[Nachname],[Scandatum],[Scanuser],
                                         [Scanclient],[Scanhostname]
                                     FROM Chargen
                                     WHERE Charge IN (
                                         SELECT DISTINCT TOP (100) Charge
                                         FROM Chargen
                                         WHERE Charge     LIKE @Search
                                            OR Kiste      LIKE @Search
                                            OR Fallnummer LIKE @Search
                                            OR Person     LIKE @Search
                                            OR Gebdat     LIKE @Search
                                            OR Vorname    LIKE @Search
                                            OR Nachname   LIKE @Search
                                            OR Scandatum  LIKE @Search
                                     )";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Search", "%" + _SearchText + "%");
                        conn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            string S(int i) => dr.IsDBNull(i) ? "" : dr.GetString(i);
                            while (dr.Read())
                            {
                                sDT.Rows.Add(
                                    S(0), S(1),  S(2),  S(3),  S(4),  S(5),
                                    S(6), S(7),  S(8),  S(9),  S(10), S(11));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Suchen:" + Environment.NewLine + ex.Message, "Error");
            }

            return sDT;
        }

        public bool saveDtToDB(DataTable _dt)
        {
            bool result = false;

            string datum       = DateTime.Now.ToString();
            string username    = Environment.GetEnvironmentVariable("USERNAME")    ?? "";
            string clientname  = Environment.GetEnvironmentVariable("CLIENTNAME")  ?? "";
            string computername = Environment.GetEnvironmentVariable("COMPUTERNAME") ?? "";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection  = conn;
                            cmd.Transaction = transaction;
                            cmd.CommandText =
                                "INSERT INTO Chargen " +
                                "([Charge],[Kiste],[Merkmal],[Fallnummer],[Person],[Gebdat],[Vorname],[Nachname],[Scandatum],[Scanuser],[Scanclient],[Scanhostname]) " +
                                "VALUES (@Charge,@Kiste,@Merkmal,@Fallnummer,@Person,@Gebdat,@Vorname,@Nachname,@Scandatum,@Scanuser,@Scanclient,@Scanhostname)";
                            cmd.Parameters.Add("@Charge",      SqlDbType.NVarChar);
                            cmd.Parameters.Add("@Kiste",       SqlDbType.NVarChar);
                            cmd.Parameters.Add("@Merkmal",     SqlDbType.NVarChar);
                            cmd.Parameters.Add("@Fallnummer",  SqlDbType.NVarChar);
                            cmd.Parameters.Add("@Person",      SqlDbType.NVarChar);
                            cmd.Parameters.Add("@Gebdat",      SqlDbType.NVarChar);
                            cmd.Parameters.Add("@Vorname",     SqlDbType.NVarChar);
                            cmd.Parameters.Add("@Nachname",    SqlDbType.NVarChar);
                            cmd.Parameters.Add("@Scandatum",   SqlDbType.NVarChar);
                            cmd.Parameters.Add("@Scanuser",    SqlDbType.NVarChar);
                            cmd.Parameters.Add("@Scanclient",  SqlDbType.NVarChar);
                            cmd.Parameters.Add("@Scanhostname", SqlDbType.NVarChar);

                            try
                            {
                                foreach (DataRow row in _dt.Rows)
                                {
                                    cmd.Parameters["@Charge"].Value      = row[0].ToString();
                                    cmd.Parameters["@Kiste"].Value       = row[1].ToString();
                                    cmd.Parameters["@Merkmal"].Value     = row[2].ToString();
                                    cmd.Parameters["@Fallnummer"].Value  = row[3].ToString();
                                    cmd.Parameters["@Person"].Value      = row[4].ToString();
                                    cmd.Parameters["@Gebdat"].Value      = row[5].ToString();
                                    cmd.Parameters["@Vorname"].Value     = row[6].ToString();
                                    cmd.Parameters["@Nachname"].Value    = row[7].ToString();
                                    cmd.Parameters["@Scandatum"].Value   = datum;
                                    cmd.Parameters["@Scanuser"].Value    = username;
                                    cmd.Parameters["@Scanclient"].Value  = clientname;
                                    cmd.Parameters["@Scanhostname"].Value = computername;
                                    cmd.ExecuteNonQuery();
                                }
                                transaction.Commit();
                                result = true;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show("Fehler beim Speichern in Datenbank: " + Environment.NewLine + ex.Message, "Error");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Verbinden zur Datenbank: " + Environment.NewLine + ex.Message, "Error");
            }
            return result;
        }

    }
}

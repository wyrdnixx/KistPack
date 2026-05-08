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

        public string getKistPackDBVersion()
        {
            string dbVersion = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = @"SELECT value FROM Settings WHERE Setting = 'DBVersion'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                            dbVersion = dr.GetString(0);
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
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                            merkmale = dr.GetString(0);
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
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                            externalArchiveCall = dr.GetString(0);
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
                    string query = @"SELECT TOP (50) * FROM Chargen WHERE Fallnummer = @Fallnummer";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Fallnummer", _Fallnummer);
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            PatientVisit tmp = new PatientVisit(
                                dr.GetString(1), dr.GetString(2),  dr.GetString(3),
                                dr.GetString(4), dr.GetString(5),  dr.GetString(6),
                                dr.GetString(7), dr.GetString(8),  dr.GetString(9),
                                dr.GetString(10), dr.GetString(11), dr.GetString(12));
                            patList.Add(tmp);
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
            sDT.Columns.Add("Scanndatum");
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
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Search", "%" + _SearchText + "%");
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            sDT.Rows.Add(
                                dr.GetString(0), dr.GetString(1),  dr.GetString(2),
                                dr.GetString(3), dr.GetString(4),  dr.GetString(5),
                                dr.GetString(6), dr.GetString(7),  dr.GetString(8),
                                dr.GetString(9), dr.GetString(10), dr.GetString(11));
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

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    SqlCommand cmd = new SqlCommand();
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
                        MessageBox.Show("Fehler beim Speichern in Datenbank: " + Environment.NewLine + ex.Message, "Error");
                    }
                }
            }
            return result;
        }

        public bool testInsertDbTransaction(string _charge, string _kiste, string _fallnummer, string _person, string _vorname, string _nachname, string _clientname, string _hostname)
        {
            bool result = false;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection  = conn;
                        cmd.Transaction = transaction;
                        cmd.CommandText =
                            "INSERT INTO Chargen " +
                            "([Charge],[Kiste],[Fallnummer],[Person],[Vorname],[Nachname],[Scandatum],[Scanuser],[Scanclient],[Scanhostname]) " +
                            "VALUES (@Charge,@Kiste,@Fallnummer,@Person,@Vorname,@Nachname,@Scandatum,@Scanuser,@Scanclient,@Scanhostname)";
                        cmd.Parameters.AddWithValue("@Charge",      _charge);
                        cmd.Parameters.AddWithValue("@Kiste",       _kiste);
                        cmd.Parameters.AddWithValue("@Fallnummer",  _fallnummer);
                        cmd.Parameters.AddWithValue("@Person",      _person);
                        cmd.Parameters.AddWithValue("@Vorname",     _vorname);
                        cmd.Parameters.AddWithValue("@Nachname",    _nachname);
                        cmd.Parameters.AddWithValue("@Scandatum",   DateTime.Now.ToString());
                        cmd.Parameters.AddWithValue("@Scanuser",    Environment.GetEnvironmentVariable("USERNAME")    ?? "");
                        cmd.Parameters.AddWithValue("@Scanclient",  Environment.GetEnvironmentVariable("CLIENTNAME")  ?? "");
                        cmd.Parameters.AddWithValue("@Scanhostname", Environment.GetEnvironmentVariable("COMPUTERNAME") ?? "");

                        cmd.ExecuteNonQuery();
                        cmd.ExecuteNonQuery();

                        transaction.Commit();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("error: " + ex.Message, "Error");
                        return false;
                    }
                }
            }
            return result;
        }

        public bool testInsertDb(string _charge, string _kiste, string _fallnummer, string _person, string _vorname, string _nachname, string _clientname, string _hostname)
        {
            bool result = false;
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection   = conn;
                    cmd.CommandType  = CommandType.Text;
                    cmd.CommandText  =
                        "INSERT INTO Chargen " +
                        "([Charge],[Kiste],[Fallnummer],[Person],[Vorname],[Nachname],[Scandatum],[Scanuser],[Scanclient],[Scanhostname]) " +
                        "VALUES (@Charge,@Kiste,@Fallnummer,@Person,@Vorname,@Nachname,@Scandatum,@Scanuser,@Scanclient,@Scanhostname)";
                    cmd.Parameters.AddWithValue("@Charge",      _charge);
                    cmd.Parameters.AddWithValue("@Kiste",       _kiste);
                    cmd.Parameters.AddWithValue("@Fallnummer",  _fallnummer);
                    cmd.Parameters.AddWithValue("@Person",      _person);
                    cmd.Parameters.AddWithValue("@Vorname",     _vorname);
                    cmd.Parameters.AddWithValue("@Nachname",    _nachname);
                    cmd.Parameters.AddWithValue("@Scandatum",   DateTime.Now.ToString());
                    cmd.Parameters.AddWithValue("@Scanuser",    Environment.GetEnvironmentVariable("USERNAME")    ?? "");
                    cmd.Parameters.AddWithValue("@Scanclient",  Environment.GetEnvironmentVariable("CLIENTNAME")  ?? "");
                    cmd.Parameters.AddWithValue("@Scanhostname", Environment.GetEnvironmentVariable("COMPUTERNAME") ?? "");
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Speichern in KistPackDB: " + Environment.NewLine + ex.Message, "Error");
            }
            return result;
        }

        public Boolean databaseFilePut(string _charge, string _varFilePath)
        {
            byte[] file;
            Boolean saved = false;

            try
            {
                using (var stream = new FileStream(_varFilePath, FileMode.Open, FileAccess.Read))
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
                using (SqlConnection conn = new SqlConnection(connString))
                using (var sqlWrite = new SqlCommand("INSERT INTO ChargenPDF (Charge,Data) Values(@Charge,@File)", conn))
                {
                    sqlWrite.Parameters.Add("@Charge", SqlDbType.VarChar).Value = _charge;
                    sqlWrite.Parameters.Add("@File", SqlDbType.VarBinary, file.Length).Value = file;
                    conn.Open();
                    sqlWrite.ExecuteNonQuery();
                    saved = true;
                }
            }
            catch (Exception ex)
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
                    {
                        if (sqlQueryResult != null && sqlQueryResult.Read())
                        {
                            var blob = new byte[sqlQueryResult.GetBytes(0, 0, null, 0, int.MaxValue)];
                            sqlQueryResult.GetBytes(0, 0, blob, 0, blob.Length);
                            using (var fs = new FileStream(varPathToNewLocation, FileMode.Create, FileAccess.Write))
                                fs.Write(blob, 0, blob.Length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der PDF Datei aus der Datenbank." + Environment.NewLine + ex.Message);
                return false;
            }
            return true;
        }
    }
}

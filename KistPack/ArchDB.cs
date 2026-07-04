using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KistPack
{
    internal class ArchDB
    {
        private static string connString = Properties.Settings.Default.SQLDBArchive;

        public static PatientVisit GetVisitFromArchive(string _Fallnummer)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = @"SELECT FALLID, PATID, GEBDAT, VORNAME, NAME, F_STORNO
                                     FROM IDX_FRI
                                     WHERE FALLID = @Fallnummer";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Fallnummer", _Fallnummer);
                        conn.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows && dr.Read())
                            {
                                string fallnummer = dr.GetString(0);
                                string person     = dr.GetString(1);
                                string gebdat     = dr.GetDateTime(2).ToString("dd.MM.yyyy");
                                string vorname    = dr.GetString(3);
                                string nachname   = dr.GetString(4);
                                string fallstorno = dr.IsDBNull(5) ? null : dr.GetString(5);

                                return new PatientVisit(fallnummer, person, gebdat, vorname, nachname, fallstorno);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Abfragen der Archiv-Datenbank: " + ex.Message, ex);
            }

            return null;
        }
    }
}

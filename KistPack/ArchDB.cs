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




        //#region Test Async

        ////test Async
        //public delegate void doWorkCallback(int result, string error);

        //public static void doWork(int n, doWorkCallback callback)
        //{
        //    int result = 0;

        //    for (int i = 0; i < n; i++)
        //    {
        //        //Do some work....
        //        Thread.Sleep(1000);
        //        result += 10;
        //    }

        //    //Call the callback delegate which points to the displayWorkDone() method and pass it the result to be returned from the thread.
        //    callback(result,null);
        //}

        //#endregion




        //private static string connString = @"Server = MSSQL; Database = TestDB; Trusted_Connection = True;";
        private static string connString = Properties.Settings.Default.SQLDBArchive;


        public static PatientVisit GetVisitFromArchive(String _Fallnummer)
        {
            try
            {
                //sql connection object
                using (SqlConnection conn = new SqlConnection(connString))
                {

                    //retrieve the SQL Server instance version
                    string query = @"SELECT FALLID, PATID, VORNAME, NAME, F_STORNO
                                     FROM IDX_FRI  where FALLID = "+_Fallnummer + ";";
                    //define the SqlCommand object
                    SqlCommand cmd = new SqlCommand(query, conn);

                    //open connection
                    conn.Open();

                    //execute the SQLCommand
                    SqlDataReader dr = cmd.ExecuteReader();

                    Console.WriteLine(Environment.NewLine + "Retrieving data from database..." + Environment.NewLine);
                    Console.WriteLine("Retrieved records:");

                    //check if there are records
                    if (dr.HasRows)
                    {
                        

                        while (dr.Read())
                        {
                            string fallnummer = dr.GetString(0);
                            string person = dr.GetString(1);
                            string nachname = dr.GetString(2);
                            string vorname = dr.GetString(3);
                            string fallstorno = null; // initiate empty if in db null
                            
                            if (!dr.IsDBNull(4)) // if the F_STORNO field is not empty
                            { 
                                 fallstorno = dr.GetString(4);
                            } 
                                


                            PatientVisit pv = new PatientVisit(fallnummer,person,vorname,nachname,fallstorno);


                            
                            //return pv;
                            return pv;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                        //global::System.Windows.Forms.MessageBox.Show("test","no data found");
                        //return null;
                        return null;
                    }

                    //close data reader
                    dr.Close();

                    //close connection
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
                //global::System.Windows.Forms.MessageBox.Show(ex.Message,"error");
                //throw new InvalidOperationException(ex.Message);
                MessageBox.Show("Fehler beim abfragen der Archiv Datenbank: "+  ex.Message,"Error");
   
            }

            return null;
            
        }
    }
}

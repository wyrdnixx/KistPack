using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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




        private static string connString = @"Server = MSSQL; Database = TestDB; Trusted_Connection = True;";

        
        public static PatientVisit GetVisit(Int32 _Fallnummer)
        {
            try
            {
                //sql connection object
                using (SqlConnection conn = new SqlConnection(connString))
                {

                    //retrieve the SQL Server instance version
                    string query = @"SELECT e.PAT, e.PER, e.surname, e.givenname
                                     FROM pat e where e.PAT = "+_Fallnummer + ";";
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
                            int PAT = Int32.Parse(dr.GetString(0));
                            int PER = Int32.Parse(dr.GetString(1));
                            string surname = dr.GetString(2);
                            string givenname = dr.GetString(3);


                            PatientVisit pv = new PatientVisit(PAT,PER,surname,givenname);


                            
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
                global::System.Windows.Forms.MessageBox.Show(ex.Message,"error");
                return null;
            }

            return null;
            
        }
    }
}

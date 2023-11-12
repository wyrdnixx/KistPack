using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KistPack
{
    internal class ArchDB
    {

        private string connString = @"Server = MSSQL; Database = TestDB; Trusted_Connection = True;";

        public PatientVisit GetVisit(string _Fallnummer)
        {

            //DataTable dt = new DataTable();
            //dt.Columns.Add("Charge");
            //dt.Columns.Add("Kiste");
            //dt.Columns.Add("Fallnr");
            //string PAT, PER, surname, givenname;

            


            try
            {
                //sql connection object
                using (SqlConnection conn = new SqlConnection(connString))
                {

                    //retrieve the SQL Server instance version
                    string query = @"SELECT e.PAT, e.PER, e.surname, e.givenname
                                     FROM pat e;
                                     ";
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
                        List<string> list = new List<string>();

                        while (dr.Read())
                        {
                            int PAT = Int32.Parse(dr.GetString(0));
                            int PER = Int32.Parse(dr.GetString(1));
                            string surname = dr.GetString(2);
                            string givenname = dr.GetString(3);


                            PatientVisit pv = new PatientVisit(PAT,PER,surname,givenname);


                            
                            return pv;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data found.");
                        global::System.Windows.Forms.MessageBox.Show("test","no data found");
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
            }

            return null;
        }
    }
}

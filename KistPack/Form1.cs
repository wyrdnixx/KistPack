using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static KistPack.ArchDB;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KistPack
{
    public partial class Form1 : Form
    {
        public static DataSet ds;
        public static DataTable dt;
        private static ArchDB archDB;

        public Form1()
        {
            InitializeComponent();
            archDB = new ArchDB();
            //ds = new DataSet();
            dt = new DataTable();


            dt.Columns.Add("Charge");
            dt.Columns.Add("Box");
            dt.Columns.Add("Visit");
            dt.Columns.Add("Person");
            dt.Columns.Add("Givenname");
            dt.Columns.Add("Surname");
            //ds.Tables.Add(dt);
            //dgvAkten.DataSource = ds;
            dgvAkten.DataSource = dt;

        }



        private void tbCharge_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allow only numeric input
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        
        private void tbKiste_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allow only numeric input
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void btnNewCharge_Click(object sender, EventArgs e)
        {
            // TEST
            //testDBSelect("12345678");
            //testAsnyc();
            //GetVisitAsync("12345678");

                // ToDo: Prüfen ob vorhandene Charge / oder ob gespeichert wurde

            cbMandant.Enabled = true;
            cbMandant.Focus();
            cbMandant.DroppedDown = true;

            tbFallScann.Enabled = false;



            tbFallScann.Text = "";
            tbKiste.Text = "";


        }

        private void cbMandant_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnCreateCharge.Enabled = true;
            btnCreateCharge.Focus();
        }

        private void btnCreateCharge_Click(object sender, EventArgs e)
        {
            // bleibt deaktiviert
            //tbCharge.Enabled = false;           

            tbCharge.Text = cbMandant.Text + DateTime.Now.ToString("yyyyMMddHHmm"); ;
            btnNextBox.Enabled = true;
            tbKiste.Enabled = true;
            tbKiste.Focus();

            btnCreateCharge.Enabled = false;
            cbMandant.Enabled = false;
        }

        private void tbKiste_TextChanged(object sender, EventArgs e)
        {
            if (tbKiste.Text.Length ==6)
            {
                tbKiste.Enabled = false;
                btnApplyNewBox.Enabled = true;
                btnApplyNewBox.Focus();
            }
        }


        private void btnNextBox_Click(object sender, EventArgs e)
        {
            tbKiste.Enabled = true;
            tbKiste.Focus();
            tbKiste.Text = "";
            tbFallScann.Text = "";
            

        }


        private void btnApplyNewBox_Click(object sender, EventArgs e)
        {

            tbKiste.Enabled = false;
            tbFallScann.Enabled = true;
            tbFallScann.Focus();


        }

        private void tbFallScann_TextChanged(object sender, EventArgs e)
        {
            if (tbFallScann.Text.Length == 8)
            {
                // Test Textbox
                //tbTest.Text = tbTest.Text + System.Environment.NewLine + tbCharge.Text + ";" + tbKiste.Text + ";" + tbFallScann.Text;
                


                //DataRow r = dt.NewRow();
                //r[0]=tbCharge.Text;
                //r[1] = tbKiste.Text;
                //r[2] = tbFallScann.Text;
                //dt.Rows.Add(r); 
                //dt.AcceptChanges();
                //dgvAkten.DataSource= dt;
                //dgvAkten.Update();
                GetVisitAsync(tbFallScann.Text);
                tbFallScann.Text = "";

                //testDBSelect(tbFallScann.Text);
            }
        }

        //private void testDBSelect(string _Fallnr)
        //{

        //    PatientVisit pv  = archDB.GetVisit(_Fallnr);
        //    if(pv != null)
        //    {
        //        Boolean dupCheck = false;

        //        foreach (DataRow row in dt.Rows)
        //        {
        //            if ( row["Visit"].ToString() == pv.Pat.ToString())
        //            {
        //                dupCheck = true;
        //                String Errmsg = "Fallnummer " + pv.Pat.ToString() + " schon vorhanden! Bitte Akte prüfen.";
        //                MessageBox.Show(Errmsg, "error");                    }
        //        }

        //        if (!dupCheck) {
        //            dt.Rows.Add(tbCharge.Text, tbKiste.Text, pv.Pat, pv.Per, pv.Givenname, pv.Surname);
        //            dt.AcceptChanges();
        //            dgvAkten.Update();
        //        }
        //    }
        //}


        #region test Async

        private void testAsnyc()
        {
            //This delegate will be called when the thread is done executing.
            ArchDB.doWorkCallback callback = new ArchDB.doWorkCallback(displayWorkDone);

            int threadInputData = 5;

            //doWork() runs as a separate thread. We pass it the data and the callback delegate.
            Thread workThread = new Thread(() => doWork(threadInputData, callback));

            //Run thread.
            workThread.Start();

        }
        public static void displayWorkDone(int result, string _error)
        {
            Console.WriteLine("Result: " + result);
            if (_error == null)
            {
                MessageBox.Show(result.ToString(), "Result");
            } else
            {
                MessageBox.Show(_error, "Error");
            }
            
        }



        private void GetVisitAsync(string _Fallnummer)
        {
            ArchDB.GetVisitCallback callback = new GetVisitCallback(processVisit);  
            Thread worker = new Thread(() => GetVisit(_Fallnummer, callback)); worker.Start();
        }

        private  void processVisit(PatientVisit pv, Exception ex)
        {
            // no exception and no visit found 
            if (ex == null && pv == null)
            {
                MessageBox.Show("Kein Fall gefunden", "no visit found");
            }
            // exception
            else if (ex != null)
            {
                MessageBox.Show("Fehler beim Datenabruf: " + ex.Message, "Error");
            } 
            // got visit
            else
            {
                MessageBox.Show("Fall gefunden: " + pv.Surname, "Result");

                updateData(pv);
            }
        }


        private void updateData(PatientVisit pv)
        {
            Boolean dupCheck = false;

            foreach (DataRow row in dt.Rows)
            {
                if (row["Visit"].ToString() == pv.Pat.ToString())
                {
                    dupCheck = true;
                    String Errmsg = "Fallnummer " + pv.Pat.ToString() + " schon vorhanden! Bitte Akte prüfen.";
                    MessageBox.Show(Errmsg, "error");
                }
            }

            if (!dupCheck)
            {
                dt.Rows.Add(tbCharge.Text, tbKiste.Text, pv.Pat, pv.Per, pv.Givenname, pv.Surname);
                dt.AcceptChanges();
                dgvAkten.Update();
            }
        }

        #endregion

    }
}

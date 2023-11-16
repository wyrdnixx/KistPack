using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
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
        private static SoundPlayer sndplayrOK;
        private static SoundPlayer sndplayrER;

            
        public Form1()
        {
            InitializeComponent();
            archDB = new ArchDB();
            //ds = new DataSet();
            dt = new DataTable();

            sndplayrOK = new SoundPlayer(Properties.Resources.ok);
            sndplayrER = new SoundPlayer(Properties.Resources.exception);

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
            if (tbKiste.Text.Length == Properties.Settings.Default.LengthKiste)
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
            if (tbFallScann.Text.Length == Properties.Settings.Default.LenghtFallnummer)
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
                
                getVisitFromArchivDB(Int32.Parse(tbFallScann.Text));
                

                
            }
        }


        #region test async Tasks

        //private async void testTask(object sender, EventArgs e)
        private async void getVisitFromArchivDB(Int32 _Fall)
        {
            PatientVisit pv=null;
            tbFallScann.Enabled = false;
            btnNewCharge.Enabled=false;
            btnNextBox.Enabled=false;
            btnApplyNewBox.Enabled=false;
            tbStatus.BackColor = Color.Gray;
            tbStatus.Text = "Suche Fallnummer: " + _Fall.ToString();
            try
            {


                // Start a new task with parameters
                //Task<int> myTask = Task.Run(() => MyMethodWithParameters(_Fall));
                Task<PatientVisit> myTask = Task.Run(() => GetVisit(_Fall));

                // Wait for the task to complete without blocking the UI
                await myTask;


                // Set the TextBox text with the result
                //textBox3.Text = myTask.Result;
                pv = myTask.Result;

                if (pv != null)
                {

                    updateData(pv);                                        

                }
                else
                {
                    //MessageBox.Show("Fallnummer wurde nicht gefunden: " + _Fall.ToString(), "Fehler");
                    tbStatus.BackColor = Color.Red;
                    tbStatus.Text = "Fallnummer wurde nicht gefunden: " + _Fall.ToString();
                    playSoundER();
                }

            } catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "error");
                tbStatus.BackColor = Color.Red;
                tbStatus.Text = "Fehler: " + ex.Message;
                playSoundER();
            }
            finally
            {
                btnNewCharge.Enabled = true;
                btnNextBox.Enabled = true;
                btnApplyNewBox.Enabled = true;
                tbFallScann.Text = "";
                tbFallScann.Enabled = true;
                tbFallScann.Focus();
            }
           

        }


        #endregion






        #region test Async threads
      

        private void updateData(PatientVisit pv)
        {
            Boolean dupCheck = false;

            foreach (DataRow row in dt.Rows)
            {
                if (row["Visit"].ToString() == pv.Pat.ToString())
                {
                    dupCheck = true;
                    String Errmsg = "Fallnummer " + pv.Pat.ToString() + " schon vorhanden! Bitte Akte prüfen.";
                    //MessageBox.Show(Errmsg, "error");
                    tbStatus.BackColor = Color.Red;
                    tbStatus.Text = "Fehler: " + Errmsg;
                    playSoundER();
                }
            }

            if (!dupCheck)
            {
                dt.Rows.Add(tbCharge.Text, tbKiste.Text, pv.Pat, pv.Per, pv.Givenname, pv.Surname);
                dt.AcceptChanges();
                dgvAkten.Update();
                tbStatus.BackColor = Color.Green;
                tbStatus.Text = "Fall " + pv.Pat + " zur Charge hinzugefügt.";
                playSoundOK();
            }
        }

        #endregion



        #region Audio

        private void playSoundOK()
        {
            try
            {
                //SoundPlayer sndplayrOK = new SoundPlayer(Properties.Resources.ok);
                sndplayrOK.Play();
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ": " + ex.StackTrace.ToString(), "Error");
            }
        }
        private void playSoundER()
        {
            try
            {
                //SoundPlayer sndplayrOK = new SoundPlayer(Properties.Resources.ok);
                sndplayrER.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ": " + ex.StackTrace.ToString(), "Error");
            }
        }


        #endregion

        private void btnDeleteEntry_Click(object sender, EventArgs e)
        {
            if (!dgvAkten.SelectedRows[0].IsNewRow)            
                dt.Rows.RemoveAt(dgvAkten.SelectedRows[0].Index);
            dgvAkten.Update();

        }


        #region PDF Print


        private void InitializeDataGridView()
        {
            // Assuming you have already populated your DataGridView with data
            // This is just an example; replace it with your actual data
            tbCharge.Text = "testCharge_000001";
            dt.Rows.Add("TESTCharge", "100001", "10001","1","Hans","Hansen");
            dt.Rows.Add("TESTCharge", "100001", "10002", "2", "Peter", "Peterson");
            dt.Rows.Add("TESTCharge", "100001", "10003", "3", "Susi", "Susen");
            dt.Rows.Add("TESTCharge", "100002", "10004", "4", "Maja", "Majar");
            dt.Rows.Add("TESTCharge", "100002", "10005", "5", "Ede", "Edwind");
            dt.Rows.Add("TESTCharge", "100003", "10006", "6", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100003", "10007", "7", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100003", "10008", "8", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100003", "10009", "9", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100004", "10010", "10", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100004", "10011", "11", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100005", "10012", "12", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100006", "10013", "13", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100006", "10014", "14", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100006", "10015", "15", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100006", "10016", "16", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100006", "10017", "17", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100007", "10018", "18", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100008", "10019", "19", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100008", "10020", "20", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100008", "10021", "21", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100008", "10022", "22", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "100009", "10023", "23", "Max", "Maxer");

            dt.AcceptChanges();

        }

        private void ExportToPDF(DataGridView dataGridView, string pdfFilePath)
        {

            try
            {
            
            // Dictionary to store box numbers and their corresponding data
            Dictionary<string, List<string[]>> boxData = new Dictionary<string, List<string[]>>();

            PdfWriter writer = new PdfWriter(pdfFilePath);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);


            var paragraph = new Paragraph("MCB Akten - Charge: " + tbCharge.Text);
            document.Add(paragraph);



            // Iterate through DataGridView rows and group data by box number
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                    string charge = row.Cells["Charge"].Value.ToString();
                    string boxNumber = row.Cells["Box"].Value.ToString();
                    string visit = row.Cells["Visit"].Value.ToString();
                    string person = row.Cells["Person"].Value.ToString();
                    string givenname = row.Cells["Givenname"].Value.ToString();
                    string surname = row.Cells["Surname"].Value.ToString();


                    if (!boxData.ContainsKey(boxNumber))
                {
                    boxData[boxNumber] = new List<string[]>();
                }

                boxData[boxNumber].Add(new string[] { visit, person, givenname, surname });
            }

            // Create a PDF document and add tables for each box
           
               
                    foreach (var kvp in boxData)
                    {
                        // Create a table for each box
                        Table table = new Table(new float[] { 1, 1,1,1})
                            .UseAllAvailableWidth();

                        // Add header row to the table
                        table.AddHeaderCell("Visit");
                        table.AddHeaderCell("Person");
                        table.AddHeaderCell("Givenname");
                        table.AddHeaderCell("Surname");

                    // Add data rows to the table
                    foreach (var rowData in kvp.Value)
                        {
                            table.AddCell(rowData[0]); // Visit
                            table.AddCell(rowData[1]); // Person
                            table.AddCell(rowData[2]); // givenname
                            table.AddCell(rowData[3]); // surname
                    }

                // Add the table to the PDF document
                document.Add(new Paragraph($"Box: {kvp.Key}"));
                document.Add(table);
                    }


            document.Close();
                //MessageBox.Show("PDF generated successfully!");
                tbStatus.BackColor = Color.Green;
                tbStatus.Text = "PDF erfolgreich erstellt";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnFinishCharge_Click(object sender, EventArgs e)
        {
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog.Title = "Export to PDF";
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")
                {
                    //ExportToPdf(dgvAkten, saveFileDialog.FileName);
                    
                    ExportToPDF(dgvAkten, saveFileDialog.FileName);
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);

                }
            }
    }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            InitializeDataGridView();
            btnFinishCharge_Click(sender, e);
            
        }
    }
}

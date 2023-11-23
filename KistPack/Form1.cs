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
using System.Xml.Linq;
using static KistPack.ArchDB;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace KistPack
{
    public partial class Form1 : Form
    {
        public static DataSet ds;
        public static DataTable dt;
        private static ArchDB archDB;
        private static SoundPlayer sndplayrOK;
        private static SoundPlayer sndplayrER;
        KistPackDB kistPackDB;


        public Form1()
        {
            InitializeComponent();

            kistPackDB = new KistPackDB();
            string dbversion = kistPackDB.getKistPackDBVersion();
            string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            
            if (dbversion != assemblyVersion )
            {
                MessageBox.Show("Sorry..." + Environment.NewLine +
                    "Programmversion " + assemblyVersion + " stimmt nicht mit Datenbank Version " + dbversion + " überein." + Environment.NewLine +
                    "Programm wird beendet.", "Error");
                Environment.Exit(-1);
            }



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

            dgvAkten.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 12);
            



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


        #region CheckAndInserttoDataTable

        //private async void testTask(object sender, EventArgs e)
        private async void getVisitFromArchivDB(Int32 _Fall)
        {
            PatientVisit pv=null;
            tbFallScann.Enabled = false;
            btnNewCharge.Enabled=false;
            btnNextBox.Enabled=false;
            btnApplyNewBox.Enabled=false;
            tbStatus.BackColor = System.Drawing.Color.Gray;
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
                    tbStatus.BackColor = System.Drawing.Color.Red;
                    tbStatus.Text = "Fallnummer wurde nicht gefunden: " + _Fall.ToString();
                    playSoundER();
                }

            } catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "error");
                tbStatus.BackColor = System.Drawing.Color.Red;
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
                    tbStatus.BackColor = System.Drawing.Color.Red;
                    tbStatus.Text = "Fehler: " + Errmsg;
                    playSoundER();
                }
            }

            if (!dupCheck)
            {
                dt.Rows.Add(tbCharge.Text, tbKiste.Text, pv.Pat, pv.Per, pv.Givenname, pv.Surname);
                dt.AcceptChanges();
                dgvAkten.Update();
                tbStatus.BackColor = System.Drawing.Color.Green;
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
            dt.Rows.Add("TESTCharge", "101", "23001","1","Hans","Hansen");
            dt.Rows.Add("TESTCharge", "101", "23002", "2", "Peter", "Peterson");
            dt.Rows.Add("TESTCharge", "101", "23003", "3", "Susi", "Susen");
            dt.Rows.Add("TESTCharge", "101", "23004", "4", "Maja", "Majar");
            dt.Rows.Add("TESTCharge", "102", "23005", "5", "Ede", "Edwind");
            dt.Rows.Add("TESTCharge", "102", "23006", "6", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "102", "23007", "7", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "104", "23008", "8", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "104", "23009", "9", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "104", "23010", "10", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "104", "23011", "11", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "104", "23012", "12", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "104", "23013", "13", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "105", "23014", "14", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "105", "23015", "15", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "105", "23016", "16", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "105", "23017", "17", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "105", "23018", "18", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "105", "23019", "19", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "105", "23020", "20", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "105", "23021", "21", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "105", "23022", "22", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "105", "23023", "23", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "106", "23024", "1", "Hans", "Hansen");
            dt.Rows.Add("TESTCharge", "106", "23025", "2", "Peter", "Peterson");
            dt.Rows.Add("TESTCharge", "106", "23026", "3", "Susi", "Susen");
            dt.Rows.Add("TESTCharge", "106", "23027", "4", "Maja", "Majar");
            dt.Rows.Add("TESTCharge", "106", "23028", "5", "Ede", "Edwind");
            dt.Rows.Add("TESTCharge", "106", "23029", "6", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "106", "23030", "7", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "107", "23031", "8", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "107", "23032", "9", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "107", "23033", "10", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "107", "23034", "11", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "108", "23035", "12", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "108", "23036", "13", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "108", "23037", "14", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "108", "23038", "15", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "108", "23039", "16", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "108", "23040", "17", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "108", "23041", "18", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "109", "23042", "19", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "109", "23043", "20", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "109", "23044", "21", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "109", "23045", "22", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "109", "23046", "23", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "109", "23047", "1", "Hans", "Hansen");
            dt.Rows.Add("TESTCharge", "109", "23048", "2", "Peter", "Peterson");
            dt.Rows.Add("TESTCharge", "109", "23049", "3", "Susi", "Susen");
            dt.Rows.Add("TESTCharge", "109", "23050", "4", "Maja", "Majar");
            dt.Rows.Add("TESTCharge", "109", "23051", "5", "Ede", "Edwind");
            dt.Rows.Add("TESTCharge", "109", "23052", "6", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "109", "23053", "7", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "109", "23054", "8", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "109", "23055", "9", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "109", "23056", "10", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "109", "23057", "11", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "109", "23058", "12", "Max", "Maxer");
            dt.Rows.Add("TESTCharge", "109", "23059", "13", "Max", "Maxer");
            
            dt.AcceptChanges();

        }

        private void ExportToPDF(DataGridView dataGridView, string pdfFilePath)
        {
            // Dictionary to store box numbers and their corresponding data
            Dictionary<string, List<string[]>> boxData = new Dictionary<string, List<string[]>>();

            
            try
            {
                // NEW PDFSharp
                // Create a new MigraDoc document
                Document document = new Document();

     
                Section section = document.AddSection();

                // Add a header to the section
                Table headerTable = section.Headers.Primary.AddTable();
                headerTable.AddColumn(Unit.FromCentimeter(5));
                headerTable.AddColumn(Unit.FromCentimeter(8));
                headerTable.AddColumn(Unit.FromCentimeter(7));
                Row headerRow = headerTable.AddRow();
                headerRow.Cells[0].AddParagraph("MCB-Charge: " + tbCharge.Text); // Replace with your logo
                //headerRow.Cells[1].AddParagraph($"Page {i + 1}");
                //headerRow.Cells[1].AddParagraph($"Page ?");
                Paragraph headerPageIndex = headerRow.Cells[1].AddParagraph();
                headerPageIndex.AddText("Seite: ");
                headerPageIndex.AddPageField();
                headerPageIndex.AddText(" / ");
                headerPageIndex.AddNumPagesField();                
                
                Paragraph foot = section.Footers.Primary.AddParagraph();                               
                foot.AddText("Seite ");
                foot.AddPageField();
                foot.AddText(" / ");
                foot.AddNumPagesField();


                String logoFile = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\" + Properties.Settings.Default.LogoFile;
                MigraDoc.DocumentObjectModel.Shapes.Image logo = headerRow.Cells[2].AddImage(logoFile);
                logo.Width = MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2.5);




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
                    //Kistennummer 
                    section.AddParagraph(); // Leerzeile                    
                    section.AddParagraph("Kiste: " + kvp.Key.ToString());
                    section.LastParagraph.Format.Font.Bold = true;
                    section.LastParagraph.Format.Font.Size=12;
                    section.AddParagraph(); // Leerzeile

                    // Create a table for each box
                    // Add a table to the section
                    Table dataTable = section.AddTable();
                    dataTable.Borders.Width = 0.75;
                    //dataTable.Rows.Height = Unit.FromCentimeter(1.5);

                    // Add columns to the table
                    for (int j = 0; j < 4; j++)
                    {
                        dataTable.AddColumn(Unit.FromCentimeter(4));
                    }
                    // Add a row for column headers
                    Row headerRowTable = dataTable.AddRow();
                    //headerRowTable.Cells[0].AddParagraph("Box");
                    headerRowTable.Cells[0].AddParagraph("Fall");
                    headerRowTable.Cells[1].AddParagraph("Person");
                    headerRowTable.Cells[2].AddParagraph("Vorname");
                    headerRowTable.Cells[3].AddParagraph("Nachname");


                    // Add data rows to the table
                    foreach (var rowData in kvp.Value)
                    {
                        //table.AddCell(rowData[0]); // Visit
                        //table.AddCell(rowData[1]); // Person
                        //table.AddCell(rowData[2]); // givenname
                        //table.AddCell(rowData[3]); // surname
                        Row dataRow = dataTable.AddRow();
                        for (int k = 0; k < 4; k++)
                        {
                            dataRow.Cells[k].AddParagraph($"{rowData[k]}");
                        }

                    }

         
                }


                

                // Create a PDF renderer and save the document to a file
                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
                pdfRenderer.Document = document;

                pdfRenderer.RenderDocument();
                pdfRenderer.PdfDocument.Save(pdfFilePath);

                tbStatus.BackColor = System.Drawing.Color.Green;
                tbStatus.Text = "PDF erfolgreich erstellt";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
            //document.Close();

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
            //kistPackDB.testInsertDb("1001", "1", "23001", "101", "hans", "mayer", "testpc", "hostname");
            kistPackDB.testInsertDbTransaction("1001", "1", "23001", "101", "hans", "mayer", "testpc", "hostname");


            //btnFinishCharge_Click(sender, e);

        }
    }

    //class HeaderEventHandler : IEventHandler
    //{
    //    private  iText.Layout.Element.Image logo;

    //    public HeaderEventHandler(iText.Layout.Element.Image logo)
    //    {
    //        this.logo = logo;
    //    }

    //    public void HandleEvent(Event currentEvent)
    //    {
    //        var documentEvent = (PdfDocumentEvent)currentEvent;
    //        var pdf = documentEvent.GetDocument();
    //        var page = documentEvent.GetPage();
    //        var canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdf);

    //        // Add logo at the top
    //        //logo.SetFixedPosition(450, page.GetPageSize().GetTop() - 100);           
    //        logo.GetAccessibilityProperties().SetAlternateDescription("Logo");
    //        logo.SetHeight(150);
    //        canvas.AddXObjectAt(logo.GetXObject(), 400, page.GetPageSize().GetTop() - 100);
            



    //        // Add page number at the top right
    //        canvas.BeginText()
    //            .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 12)
    //            .MoveText(page.GetPageSize().GetRight() - 72, page.GetPageSize().GetTop() - 36)
    //            .ShowText("Page " + pdf.GetPageNumber(page))
    //            .EndText();

    //        canvas.Release();
    //    }
    //}
    



}

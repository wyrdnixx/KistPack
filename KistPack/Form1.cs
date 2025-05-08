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
using System.Diagnostics;

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
        private Boolean chargeWasSavedToDB;
        private String tempFilePath;
        private String[] merkmale;

        public Form1()
        {
            InitializeComponent();

            // Datenbankverbindung herstellen und testen.
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


            // vergübare Merkmale aus DB lesen und für clbMerkmale verwenden
            string merkmaleDBString = kistPackDB.getKistPackDBMerkmale();
            merkmale = merkmaleDBString.Split(';'); // array merkmale ist private global da es auch für context menü verwendet wird (#region contextMenu)

            foreach (var m in merkmale)
            {
                cblMerkmale.Items.Add(m);
            }
            //cblMerkmale.SetItemChecked(0, true);  // erstes item der Liste per Defaul aktivieren
            clbMerkmale_SelectDefault();// erstes item der Liste per Defaul aktivieren


            // Programmvariablen initialisieren
            archDB = new ArchDB();
            dt = new DataTable();
            sndplayrOK = new SoundPlayer(Properties.Resources.ok);
            sndplayrER = new SoundPlayer(Properties.Resources.exception);
            tempFilePath = Environment.GetEnvironmentVariable("TEMP") + "\\KistPack\\";
            //MessageBox.Show("temp: "+tempFilePath, "teswt");

            // Datentabelle konfigurieren
            dt.Columns.Add("Charge");
            dt.Columns.Add("Kiste");
            dt.Columns.Add("Merkmal");
            dt.Columns.Add("Fallnummer");            
            dt.Columns.Add("Person");
            dt.Columns.Add("Gebdat");
            dt.Columns.Add("Vorname");
            dt.Columns.Add("Nachname");            
            dgvAkten.DataSource = dt;

            dgvAkten.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 12);


            // Cleanup old files from temp folder
            
            bool exists = System.IO.Directory.Exists(tempFilePath);
            if (!exists)
                System.IO.Directory.CreateDirectory(tempFilePath);

            System.IO.DirectoryInfo di = new DirectoryInfo(tempFilePath);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }



            // activate context menu for datagridview
            InitializeContextMenu();
            

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
            createNewCharge();
        }

        private void createNewCharge()
        {

            // ToDo: Prüfen ob vorhandene Charge / oder ob gespeichert wurde

            chargeWasSavedToDB = false;


            cbMandant.Enabled = true;
            cbMandant.Focus();
            cbMandant.DroppedDown = true;

            tbFallScann.Enabled = false;
            tbFallScann.Text = "";
            tbKiste.Text = "";

            tbFallScann.Text = "";
            btnNextBox.Enabled = true;
            btnDeleteEntry.Enabled = true;
            btnFinishCharge.Text = "Charge abschließen";

            btnNextBox.Enabled = true;
            dt.Clear();
        }

        private void cbMandant_SelectedIndexChanged(object sender, EventArgs e)
        {
            //btnCreateCharge.Enabled = true;
            //btnCreateCharge.Focus();
            createCharge();
        }

        private void createCharge()
        {
            tbCharge.Text = cbMandant.Text + DateTime.Now.ToString("yyyyMMddHHmm"); ;
            btnNextBox.Enabled = true;
            tbKiste.Enabled = true;
            tbKiste.Focus();

            //btnCreateCharge.Enabled = false;
            cbMandant.Enabled = false;
        }

         private void btnCreateCharge_Click(object sender, EventArgs e)
        {
            // bleibt deaktiviert
            //tbCharge.Enabled = false;           

            tbCharge.Text = cbMandant.Text + DateTime.Now.ToString("yyyyMMddHHmm"); ;
            btnNextBox.Enabled = true;
            tbKiste.Enabled = true;
            tbKiste.Focus();

            //btnCreateCharge.Enabled = false;
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
            btnNextBox.Enabled = false;
            tbFallScann.Enabled = true;
            tbFallScann.Focus();
            btnApplyNewBox.Enabled = false;

        }

        private void tbFallScann_TextChanged(object sender, EventArgs e)
        {
            if (tbFallScann.Text.Length == Properties.Settings.Default.LenghtFallnummer)
            {
                // test if visit is already in list
                if (!testCurrentDataTable(tbFallScann.Text))
                {
                    insertNewVisit(tbFallScann.Text);
                
                }
                
            }
        }



        private void btnFinishCharge_Click(object sender, EventArgs e)
        {
            if (!chargeWasSavedToDB)
            {
                // First save to Database
                if (kistPackDB.saveDtToDB(dt))
                {
                    chargeWasSavedToDB = true;
                    tbFallScann.Enabled = false;
                    tbFallScann.Text = "Charge abgeschlossen.";
                    btnNextBox.Enabled = false;
                    btnDeleteEntry.Enabled = false;
                    btnFinishCharge.Text = "PDF erneut drucken";


                    //ToDo: save CSV File to DB
                    // if path in settings is set without trailing "\" append one
                    String csvPath = Properties.Settings.Default.CSVExportPath;
                    if (!csvPath.EndsWith("\\")){
                        csvPath+= "\\";
                    }

                    // export csv and if success generate pdf 
                    if(!CSVExport(dt, csvPath + tbCharge.Text + ".csv"))
                    {
                        tbStatus.BackColor = System.Drawing.Color.SeaShell;
                        tbStatus.Text = "Es ist ein Fehler beim speichern der Lieferschein CSV-Datei aufgetreten. ";
                    } else
                    {
                        // generate PDF File
                        String pdfFilePath = tempFilePath + tbCharge.Text + ".pdf";
                        if (ExportToPDF(dgvAkten,tbCharge.Text, pdfFilePath))
                        {
                            System.Diagnostics.Process.Start(pdfFilePath);

                            // Save PDF File to Database
                            // JoHe: Deaktiviert - doch nicht in DB speichern sondern neu generieren
                            //kistPackDB.databaseFilePut(tbCharge.Text, pdfFilePath);
                            createNewCharge();
                        }
                    };

                    


                    btnNewCharge.Focus();
                }else
                {
                    tbStatus.BackColor = System.Drawing.Color.SeaShell;
                    tbStatus.Text = "Es ist ein Fehler beim speichern in die Datenbank aufgetreten.";
                }
            }                 
            

            

        }


      

            #region CheckAndInserttoDataTable

            //private async void testTask(object sender, EventArgs e)
            private async void insertNewVisit(String _Fall)
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
                //btnCreateCharge.Enabled = false;
                btnFinishCharge.Enabled = false;
                // test if the visit exists in the Archvie Database
                Task<PatientVisit> myTask = Task.Run(() => GetVisitFromArchive(_Fall));
                // Wait for the task to complete without blocking the UI
                await myTask;
                pv = myTask.Result;
                //btnCreateCharge.Enabled = false;
                btnFinishCharge.Enabled = true;

                // setze markiertes Merkmal für den Eintrag
                foreach (var item in cblMerkmale.CheckedItems)
                {
                    pv.Merkmal += item.ToString();
                }


                if (pv != null && pv.Fallstorno ==null)
                {
                    // Test if visit already exists in database / visit hast already been scanned
                    List<PatientVisit> foundList = kistPackDB.searchPat(_Fall.ToString());
                    if (foundList.Count > 0 && pv.Merkmal != "Nachlaufender-Befund" && pv.Merkmal != "Neulieferung") { 

                        String str = null;
                        foreach (PatientVisit v in foundList)
                        {
                            str += v.Charge + " | " + v.Kiste + " | " + v.Scandatum + Environment.NewLine;
                        }
                        playSoundER();
                        DialogResult question = MessageBox.Show("Die Fallnummer " + _Fall.ToString() + " wurde bereits versendet: " +  Environment.NewLine +
                            "Charge         |      Kiste      |     Datum " + Environment.NewLine + 
                            str + Environment.NewLine +
                            "Fallnummer / Akte als Nachlaufenden Befund versenden?"

                            ,
                               "Fall bereits gescannt...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        switch (question)
                        {
                            case DialogResult.Yes:

                                pv.Merkmal = "Nachlaufender-Befund";

                                updateData(pv);
                                    break;
                            case DialogResult.No:
                                break;
                        }


                    } else
                    {


                        // trage Daten in Tabelle ein
                        updateData(pv);
                    }
                                                           

                }else if (pv != null && pv.Fallstorno != null)
                {
                    tbStatus.BackColor = System.Drawing.Color.SeaShell;
                    tbStatus.Text = "Fallnummer ist ein Stornierter Fall: " + _Fall.ToString();
                    playSoundER();
                }
                else
                {
                    //MessageBox.Show("Fallnummer wurde nicht gefunden: " + _Fall.ToString(), "Fehler");
                    tbStatus.BackColor = System.Drawing.Color.SeaShell;
                    tbStatus.Text = "Fallnummer wurde nicht gefunden: " + _Fall.ToString();
                    playSoundER();
                }

            } catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "error");
                tbStatus.BackColor = System.Drawing.Color.SeaShell;
                tbStatus.Text = "Fehler: " + ex.Message;
                playSoundER();
            }
            finally
            {
                btnNewCharge.Enabled = true;
                btnNextBox.Enabled = true;
                
                tbFallScann.Text = "";
                tbFallScann.Enabled = true;
                tbFallScann.Focus();
                clbMerkmale_SelectDefault();// erstes item der Liste per Defaul aktivieren
            }
           

        }
      
        private bool testCurrentDataTable(String _fallnummer)
        {
            Boolean exists = false;


            foreach (DataRow row in dt.Rows)
            {
                if (row["Fallnummer"].ToString() == _fallnummer)
                {
                    exists = true;
                    String Errmsg = "Fallnummer " + _fallnummer + " schon vorhanden! Bitte Akte prüfen.";
                    //MessageBox.Show(Errmsg, "error");
                    tbStatus.BackColor = System.Drawing.Color.SeaShell;
                    tbStatus.Text = "Fehler: " + Errmsg;
                    playSoundER();
                    tbFallScann.Text = "";
                    tbFallScann.Focus();
                }
            }
            return exists;
        }


        /// <summary>
        /// Eintragen der Fallnummer in die Datentabelle zur Ansicht
        /// </summary>
        /// <param name="pv"></param>
        private void updateData(PatientVisit pv)
        {            
                dt.Rows.Add(tbCharge.Text, tbKiste.Text, pv.Merkmal, pv.Fallnummer, pv.Person,pv.Gebdat, pv.Vorname, pv.Nachname);
                dt.AcceptChanges();
                dgvAkten.Update();
                tbStatus.BackColor = System.Drawing.Color.LimeGreen;
                tbStatus.Text = "Fall " + pv.Fallnummer + " zur Charge hinzugefügt.";
                playSoundOK();
            
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
            if (dgvAkten.SelectedRows.Count != 0)
            {
                if (!dgvAkten.SelectedRows[0].IsNewRow)
                    dt.Rows.RemoveAt(dgvAkten.SelectedRows[0].Index);
                dgvAkten.Update();
            }           

            if(dt.Rows.Count != 0)
            {
                btnFinishCharge.Enabled = true;
            } else
            {
                btnFinishCharge.Enabled = false;
            }
        }


        #region PDF Print


        private void generateTestData()
        {
            // Assuming you have already populated your DataGridView with data
            // This is just an example; replace it with your actual data
            //tbCharge.Text = "testCharge_000001";
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

        private Boolean ExportToPDF(DataGridView dataGridView, string _chargenNummer, string pdfFilePath)
        {
            Boolean result = false;
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
                headerTable.AddColumn(Unit.FromCentimeter(10));
                headerTable.AddColumn(Unit.FromCentimeter(7));
                Row headerRow = headerTable.AddRow();

                Paragraph headerPageIndex = headerRow.Cells[0].AddParagraph();
                headerPageIndex.AddText("Seite: ");
                headerPageIndex.AddPageField();
                headerPageIndex.AddText(" / ");
                headerPageIndex.AddNumPagesField();

                headerRow.Cells[1].AddParagraph("MCB-Charge: " + _chargenNummer); // ToDo: use charge from Searchfield
                headerRow.Cells[1].Format.Font.Bold = true;
                headerRow.Cells[1].Format.Font.Size = 12;
                         
                
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
                    string boxNumber = row.Cells["Kiste"].Value.ToString();
                    string merkmal = row.Cells["Merkmal"].Value.ToString();
                    string visit = row.Cells["Fallnummer"].Value.ToString();
                    //string person = row.Cells["Person"].Value.ToString();
                    string gebdat = row.Cells["Gebdat"].Value.ToString();
                    //string givenname = row.Cells["Vorname"].Value.ToString();
                    //string surname = row.Cells["Nachname"].Value.ToString();


                    if (!boxData.ContainsKey(boxNumber))
                {
                    boxData[boxNumber] = new List<string[]>();
                }

                    //boxData[boxNumber].Add(new string[] { visit, person, gebdat, givenname, surname });
                    boxData[boxNumber].Add(new string[] { merkmal, visit,  gebdat });
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

                    // Alle Felder (mit Name)
                    //dataTable.AddColumn(Unit.FromCentimeter(2.5));
                    //dataTable.AddColumn(Unit.FromCentimeter(2.5));
                    //dataTable.AddColumn(Unit.FromCentimeter(2.5));
                    //dataTable.AddColumn(Unit.FromCentimeter(5));
                    //dataTable.AddColumn(Unit.FromCentimeter(5));


                    dataTable.AddColumn(Unit.FromCentimeter(5));
                    dataTable.AddColumn(Unit.FromCentimeter(5));
                    dataTable.AddColumn(Unit.FromCentimeter(5));
                    

                    // Add a row for column headers
                    Row headerRowTable = dataTable.AddRow();
                    //headerRowTable.Cells[0].AddParagraph("Box");
                    headerRowTable.Cells[0].AddParagraph("Merkmal");
                    headerRowTable.Cells[1].AddParagraph("Fall");
                    //headerRowTable.Cells[2].AddParagraph("Person");
                    headerRowTable.Cells[2].AddParagraph("Gebdat");
                    //headerRowTable.Cells[3].AddParagraph("Vorname");
                    //headerRowTable.Cells[4].AddParagraph("Nachname");


                    // Add data rows to the table
                    foreach (var rowData in kvp.Value)
                    {
                      
                        Row dataRow = dataTable.AddRow();
                        for (int k = 0; k < 3; k++)
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
                result = true;

                tbStatus.BackColor = System.Drawing.Color.LimeGreen;
                tbStatus.Text = "PDF erfolgreich erstellt";
                
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            return result;

        }
        #endregion


        #region CSV-Export


        private bool CSVExport(DataTable _dt, String strFilePath )
        {
            // ToDo
            try
            {


                StreamWriter sw = new StreamWriter(strFilePath, false);
                //headers
                for (int i = 0; i < _dt.Columns.Count; i++)
                {
                    sw.Write(_dt.Columns[i]);
                    if (i < _dt.Columns.Count - 1)
                    {
                        sw.Write(";");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in _dt.Rows)
                {
                    for (int i = 0; i < _dt.Columns.Count; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            string value = dr[i].ToString();
                            if (value.Contains(';'))
                            {
                                value = String.Format("\"{0}\"", value);
                                sw.Write(value);
                            }
                            else
                            {
                                sw.Write(dr[i].ToString());
                            }
                        }
                        if (i < _dt.Columns.Count - 1)
                        {
                            sw.Write(";");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                playSoundER();
                MessageBox.Show("Fehler beim Speichern der CSV-Datei: " + Environment.NewLine + ex.Message, "Error");
            }
            return false;

        }

        #endregion



        #region SearchTab


        private void tbSearchText_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && tbSearchText.Text.Length != 0)
            {
                // Tu was
                DataTable dtSearchResult  =  kistPackDB.searchWildcard(tbSearchText.Text);
                dgvSearchResults.DataSource = dtSearchResult;
                dgvSearchResults.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 12);
                dgvSearchResults.ClearSelection();

                if (dtSearchResult != null )
                {
                    // Optional: Clear previous selections
                    

                    // Iterate through each row in the DataGridView
                    foreach (DataGridViewRow row in dgvSearchResults.Rows)
                    {
                        // Iterate through each cell in the current row
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            // Check if the cell value is "test"
                            //if (cell.Value != null && cell.Value.ToString() == tbSearchText.Text)
                            if (cell.Value != null && cell.Value.ToString().ToLower().Contains(tbSearchText.Text.ToLower()))
                                {
                                // Select the entire row
                                //row.Selected = true;
                                cell.Selected = true;
                                //break; // Break out of the inner loop since we've found "test" in the row
                            }
                        }
                    }
                }
            }
        }


        #endregion

        // get the file from DB : JoHe: deaktiviert - soll doch neu generiert werden.
        //private void btnFetchPDFfromArchive_Click(object sender, EventArgs e)
        //{
        //    if(dgvSearchResults.SelectedCells.Count>1)
        //    {
        //        MessageBox.Show("Bitte einzelnen Eintrag Auswählen dessen Charge Sie abrufen wollen.", "Bitte wählen");
        //    } else
        //    {                
        //        string cellValue = dgvSearchResults.Rows[dgvSearchResults.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
        //        //MessageBox.Show(cellValue);
        //        if(kistPackDB.databaseFileRead(cellValue, tempFilePath + cellValue + ".pdf"))
        //        {
        //            System.Diagnostics.Process.Start(tempFilePath + cellValue + ".pdf");
        //        }

        //    }
        //}
        private void btnRegenPDF_Click(object sender, EventArgs e)
        {
            if (dgvSearchResults.SelectedCells.Count > 1 || dgvSearchResults.SelectedCells.Count ==0)
            {
                MessageBox.Show("Bitte einzelnen Eintrag Auswählen dessen Charge Sie abrufen wollen.", "Bitte wählen");
            }
            else
            {
                string selectedChargeNumber = dgvSearchResults.Rows[dgvSearchResults.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
                dgvSearchResults.DataSource = null;

                DataTable tmpDT = kistPackDB.searchWildcard(selectedChargeNumber);
                dgvSearchResults.DataSource = tmpDT;
                // generate PDF File
                String pdfFilePath = tempFilePath + selectedChargeNumber + ".pdf";
                if (ExportToPDF(dgvSearchResults, selectedChargeNumber, pdfFilePath))
                {
                    System.Diagnostics.Process.Start(pdfFilePath);
                }
                

            }
        }
        private void btnRegenCSV_Click(object sender, EventArgs e)
        {
            if (dgvSearchResults.SelectedCells.Count > 1 || dgvSearchResults.SelectedCells.Count == 0)
            {
                MessageBox.Show("Bitte einzelnen Eintrag Auswählen dessen Charge Sie abrufen wollen.", "Bitte wählen");
            }
            else
            {
                string selectedChargeNumber = dgvSearchResults.Rows[dgvSearchResults.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
                dgvSearchResults.DataSource = null;

                DataTable tmpDT = kistPackDB.searchWildcard(selectedChargeNumber);
                dgvSearchResults.DataSource = tmpDT;


                if (cb_SubmitCsv.Checked)  // wenn erneuete Übermittlung der CSV ausgewählt wurde
                {
                    String csvPath = Properties.Settings.Default.CSVExportPath;
                    if (!csvPath.EndsWith("\\"))
                    {
                        csvPath += "\\";
                    }
                    
                    if (!CSVExport(dt, csvPath + selectedChargeNumber + ".csv"))
                    {                        
                        MessageBox.Show("Es ist ein Fehler beim speichern der Lieferschein CSV - Datei aufgetreten. " + csvPath + selectedChargeNumber + ".csv", "Fehler");                        
                    } else
                    {
                        MessageBox.Show("Die CSV Lieferscheindatei wurde erfolgreich neu erstellt:  "+ csvPath + selectedChargeNumber + ".csv", "OK");
                    }
                } else  // wenn keine erneute Übermittlung ausgewählt wurde csv Datei nur anzeigen.
                {

                    String csvFilePath = tempFilePath + selectedChargeNumber + ".csv";
                    if (CSVExport(tmpDT, csvFilePath))
                    {
                        System.Diagnostics.Process.Start("notepad.exe", csvFilePath);
                    }
                }
                

                // checkbox zum übermitteln wieder auf false setzen
                cb_SubmitCsv.Checked = false;
            }
        }

        private void clbMerkmale_SelectedIndexChanged(object sender, EventArgs e)
        {
            // deaktiviere alle items bis auf das im moment gewählte (nur ein Eintrag darf aktiv sein
            int idx = cblMerkmale.SelectedIndex;
            for (int i = 0; i < cblMerkmale.Items.Count; i++)
            {
                if (i != idx)
                {
                    cblMerkmale.SetItemChecked(i, false);
                    

                }
            }

            // Fokus zurück auf Fallnummernscan Feld
            tbFallScann.Focus();
        }

        private void clbMerkmale_SelectDefault()
        {
            
            for (int i = 0; i < cblMerkmale.Items.Count; i++)
            {
                if (i == 0)
                {
                    cblMerkmale.SetItemChecked(i, true);
                    
                }else
                {
                    cblMerkmale.SetItemChecked(i, false);
                }
            }

            
        }


        #region ContextMenu for datagridview

        private void InitializeContextMenu()
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();


            foreach (string merkmal in merkmale)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(merkmal);
                menuItem.Click += MenuItem_Click;
                contextMenuStrip.Items.Add(menuItem);
            }
                        
            // Create and add menu items
            //ToolStripMenuItem showTextMenuItem = new ToolStripMenuItem("Show Text");
            //showTextMenuItem.Click += ShowTextMenuItem_Click;

            // Add a separator
            contextMenuStrip.Items.Add(new ToolStripSeparator());

            // Create and add menu items
            ToolStripMenuItem OpenInArchiveMenuItem = new ToolStripMenuItem("öffnen im Archiv");
            OpenInArchiveMenuItem.Click += OpenInArchiveMenuItem_Click;
            contextMenuStrip.Items.Add(OpenInArchiveMenuItem);

            // Add a separator
            contextMenuStrip.Items.Add(new ToolStripSeparator());


            ToolStripMenuItem deleteRowMenuItem = new ToolStripMenuItem("Delete Row");
            deleteRowMenuItem.Click += DeleteRowMenuItem_Click;

            contextMenuStrip.Items.AddRange(new ToolStripItem[] { deleteRowMenuItem });

            // Assign the context menu to the DataGridView
            dgvAkten.ContextMenuStrip = contextMenuStrip;

            // Subscribe to the MouseDown event of the DataGridView
            dgvAkten.MouseDown += DataGridView1_MouseDown;

        }
        
        /// <summary>
        /// select the row if an right click was made
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the right mouse button was clicked
            if (e.Button == MouseButtons.Right)
            {
                // Get the row index under the mouse cursor
                DataGridView.HitTestInfo hit = dgvAkten.HitTest(e.X, e.Y);

                // If a row was clicked
                if (hit.RowIndex >= 0)
                {
                    // Select the row
                    dgvAkten.ClearSelection();
                    dgvAkten.Rows[hit.RowIndex].Selected = true;
                }
            }
        }

        /// <summary>
        /// set the "merkmal" to the selected option from the contextmenu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, EventArgs e)
        {
            if (dgvAkten.SelectedRows.Count > 0)
            {
                // Get the selected menu item
                ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

                // Update the value of the third column in the selected row - Cell 2 is merkmal
                dgvAkten.SelectedRows[0].Cells[2].Value = menuItem.Text;
            }
        }

               
        private void OpenInArchiveMenuItem_Click(object sender, EventArgs e)
        {

            string visitNo = dgvAkten.SelectedRows[0].Cells[3].Value.ToString();

            string ExternalArchiveCall = kistPackDB.getKistPackDBExternalArchiveCall();

            // Create a new process
            Process process = new Process();

            // Set the process start information - always use cmd to start the programm
            process.StartInfo.FileName = "cmd.exe";
            //string parameters = "/c echo  ping #FALLNUMMER && pause ";
            string parameters = "/c " + ExternalArchiveCall; // use the entry from the database to call the external programm

            parameters = parameters.Replace("#FALLNUMMER", visitNo);  // replace the variable from the settings string

            //process.StartInfo.Arguments = "www.google.com";
            process.StartInfo.Arguments = parameters;

            // Optionally, you can configure the process to not create a window
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.UseShellExecute = false;

            // Start the process asynchronously
            process.Start();

        }
        private void DeleteRowMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvAkten.SelectedRows.Count > 0)
            {
                // Get the selected row index
                int rowIndex = dgvAkten.SelectedRows[0].Index;

                // Remove the row from the DataTable
                dt.Rows.RemoveAt(rowIndex);

                // Refresh the DataGridView
                dgvAkten.DataSource = null;
                dgvAkten.DataSource = dt;
            }
        }
        #endregion
    }





}

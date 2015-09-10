using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;

namespace Tool
{
    public partial class MainForm : Form
    {
        private readonly Workflow _flow = new Workflow();

        //parameters from the popup window textboxes with default values
        private string _prefix0 = "ID_step1_";
        private string _prefix1 = "ID_step2_";
        private string _prefix2 = "ID_step3_";
        private string _prefix3 = "ID_step4_";

        private Boolean _preprocessing = true;
        private string _noaa = "X;*";
        private string _minSeqLength = "100";

        private string _cdhitPercentageHomolog = "0.95";
        private string _cdhitPercentageStep0 = "1.0";
        private string _cdhitPercentageStep1 = "0.95";
        private string _cdhitPercentageStep2 = "0.95";

        private string _percentage1 = "0.6";
        private string _evalue1 = "10e-10";
        private string _bitscore1 = "100";
        private string _queryLength1 = "100";
        private string _alignmentLength1 = "100";

        private string _percentage2 = "0.6";
        private string _evalue2 = "10e-10";
        private string _bitscore2 = "100";
        private string _queryLength2 = "100";
        private string _alignmentLength2 = "100";

        private string _percentage3 = "0.6";
        private string _evalue3 = "10e-10";
        private string _bitscore3 = "100";
        private string _queryLength3 = "100";
        private string _alignmentLength3 = "100";

        //for message boxes
        private readonly Regex _warningRegex = new Regex("Warning:\n(.*)");
        private readonly Regex _informationRegex = new Regex("Information:\n(.*)");
        private const string PathValidationWarningTitel = "Path Validation Warning";
        private const string InformationTitel = "Information";


        public MainForm()
        {
            InitializeComponent();
        }
        
        //
        //load form
        //
        private void Form1_Load(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(storage_label,  "Folder path to store the output.");
            ToolTip1.SetToolTip(blast_label,    "Folder path containing blastp.exe and makeblastdb.exe.");
            ToolTip1.SetToolTip(cdhit_label,    "Folder path containing cd_hit.exe.");
            ToolTip1.SetToolTip(original_label, "File path to input set.");
            ToolTip1.SetToolTip(homolog_label,  "File path to scaffold set.");
        }
        //
        //question before closing
        //
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            // Confirm user wants to close
            switch (MessageBox.Show(this, "Are you sure you want to close?", "Confirm closing", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    break;
            }
        }
        //
        //textboxes for files and browse buttons
        //
        private void storage_textbox_TextChanged(object sender, EventArgs e){}
        private void storage_textbox_Enter(object sender, EventArgs e) 
        { 
            storage_textbox.BackColor = Color.White; 
            start_button.Enabled = true;
        }
        private void storage_textbox_Leave(object sender, EventArgs e){}
        private void storage_browse_button_Click(object sender, EventArgs e)
        {
            storage_textbox.BackColor = Color.White;
            start_button.Enabled = true;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            string message = "";
            string shortenedMessage = "";
            List<string> files = new List<string>();
            if (result == DialogResult.OK)
            {
                storage_textbox.Text = string.Format("{0}", folderBrowserDialog1.SelectedPath);
                message = IsValidPath(storage_textbox, "storage folder", files, false);
                if (message != "")
                {
                    shortenedMessage += _warningRegex.Match(message).Groups[1];
                    MessageBox.Show(shortenedMessage, PathValidationWarningTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void blast_textbox_TextChanged(object sender, EventArgs e){}
        private void blast_textbox_Enter(object sender, EventArgs e)
        {
            blast_textbox.BackColor = Color.White; 
            start_button.Enabled = true;
        }
        private void blast_textbox_Leave(object sender, EventArgs e){}
        private void blastp_browse_button_Click(object sender, EventArgs e)
        {
            blast_textbox.BackColor = Color.White;
            start_button.Enabled = true;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            string message = "";
            string shortenedMessage = "";
            List<string> files = new List<string>();
            files.Add("blastp.exe");
            files.Add("makeblastdb.exe");
            if (result == DialogResult.OK)
            {
                blast_textbox.Text = string.Format("{0}", folderBrowserDialog1.SelectedPath);
                message = IsValidPath(blast_textbox, "blast folder", files, true);
                if (message != "")
                {
                    shortenedMessage += _warningRegex.Match(message).Groups[1];
                    MessageBox.Show(shortenedMessage, PathValidationWarningTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void cdhit_textbox_TextChanged(object sender, EventArgs e){}
        private void cdhit_textbox_Enter(object sender, EventArgs e)
        {
            cdhit_textbox.BackColor = Color.White; 
            start_button.Enabled = true;
        }
        private void cdhit_textbox_Leave(object sender, EventArgs e){}
        private void cdhit_browse_button_Click(object sender, EventArgs e)
        {
            cdhit_textbox.BackColor = Color.White;
            start_button.Enabled = true;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            string message = "";
            string shortenedMessage = "";
            List<string> files = new List<string>();
            files.Add("cd_hit.exe");
            if (result == DialogResult.OK)
            {
                cdhit_textbox.Text = string.Format("{0}", folderBrowserDialog1.SelectedPath);
                message = IsValidPath(cdhit_textbox, "cd-hit folder", files, true);
                if (message != "")
                {
                    shortenedMessage += _warningRegex.Match(message).Groups[1];
                    MessageBox.Show(shortenedMessage, PathValidationWarningTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void sequenceSet_textbox_TextChanged(object sender, EventArgs e){}
        private void sequenceSet_textbox_Enter(object sender, EventArgs e)
        {
            input_textbox.BackColor = Color.White; 
            start_button.Enabled = true;
        }
        private void sequenceSet_textbox_Leave(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(input_textbox.Text))){
                ValidateFastaFile(input_textbox,"input set");
            }
        }
        private void sequenceSet_browse_button_Click(object sender, EventArgs e)
        {
            input_textbox.BackColor = Color.White;
            start_button.Enabled = true;
            DialogResult result = openFileDialog1.ShowDialog();
            if(result==DialogResult.OK)
            {
                input_textbox.BackColor = Color.White;
                input_textbox.Text = string.Format("{0}", openFileDialog1.FileName);
                ValidateFastaFile(input_textbox, "input set");
            }
        }

        private void homolog_textbox_TextChanged(object sender, EventArgs e){}
        private void homolog_textbox_Enter(object sender, EventArgs e)
        {
            scaffold_textbox.BackColor = Color.White; 
            start_button.Enabled = true;
        }
        private void homolog_textbox_Leave(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(scaffold_textbox.Text)))
            {
                ValidateFastaFile(scaffold_textbox, "scaffold set");
            }

        }
        private void homolog_browse_button_Click(object sender, EventArgs e)
        {
            start_button.Enabled = true;
            scaffold_textbox.BackColor = Color.White;
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                scaffold_textbox.BackColor = Color.White;
                scaffold_textbox.Text = string.Format("{0}", openFileDialog1.FileName);
                ValidateFastaFile(scaffold_textbox, "scaffold set");
            }
        }
        //
        //optional parameters
        //
        private void optionalParam_button_Click(object sender, EventArgs e)
        {
            //create popup window with values
            //first opening default values than whatever was entered
            PopupOptionalParams popup = new PopupOptionalParams();
                
            popup.prefix_step1_textbox.Text = _prefix0;
            popup.prefix_step2_textbox.Text = _prefix1;
            popup.prefix_step3_textbox.Text = _prefix2;
            popup.prefix_step4_textbox.Text = _prefix3;

            popup.preprocessing_checkbox.Checked = _preprocessing;
            popup.noaa_textbox.Text = _noaa;
            popup.minSeqLength_textbox.Text = _minSeqLength;

            popup.cdhit_param_scaffold_textbox.Text = _cdhitPercentageHomolog;
            popup.cdhit_param_step1_textbox.Text = _cdhitPercentageStep0;
            popup.cdhit_param_step2_textbox.Text = _cdhitPercentageStep1;
            popup.cdhit_param_step3_textbox.Text = _cdhitPercentageStep2;

            popup.percent2_textbox.Text = _percentage1;
            popup.evalue2_textbox.Text = _evalue1;
            popup.bitscore2_textbox.Text = _bitscore1;
            popup.queryLength2_textbox.Text = _queryLength1;
            popup.alignmentLength2_textbox.Text = _alignmentLength1;

            popup.percent3_textbox.Text = _percentage2;
            popup.evalue3_textbox.Text = _evalue2;
            popup.bitscore3_textbox.Text = _bitscore2;
            popup.queryLength3_textbox.Text = _queryLength2;
            popup.alignmentLength3_textbox.Text = _alignmentLength2;

            popup.percent4_textbox.Text = _percentage3;
            popup.evalue4_textbox.Text = _evalue3;
            popup.bitscore4_textbox.Text = _bitscore3;
            popup.queryLength4_textbox.Text = _queryLength3;
            popup.alignmentLength4_textbox.Text = _alignmentLength3;

            //starting the dialogue with the popup window
            

            if (!start_button.Enabled)
            {
                popup.resetAll_button.Enabled = false;
                popup.prefixes_reset_button.Enabled = false;
                popup.cdhit_reset_button.Enabled = false;
                popup.blast_reset_button.Enabled = false;
            
                popup.prefix_step1_textbox.Enabled = false;
                popup.prefix_step2_textbox.Enabled = false;
                popup.prefix_step3_textbox.Enabled = false;
                popup.prefix_step4_textbox.Enabled = false;

                popup.preprocessing_checkbox.Enabled = false;
                popup.noaa_textbox.Enabled = false;
                popup.minSeqLength_textbox.Enabled = false;

                popup.cdhit_param_scaffold_textbox.Enabled = false;
                popup.cdhit_param_step1_textbox.Enabled = false;
                popup.cdhit_param_step2_textbox.Enabled = false;
                popup.cdhit_param_step3_textbox.Enabled = false;

                popup.percent2_textbox.Enabled = false;
                popup.evalue2_textbox.Enabled = false;
                popup.bitscore2_textbox.Enabled = false;
                popup.queryLength2_textbox.Enabled = false;
                popup.alignmentLength2_textbox.Enabled = false;

                popup.percent3_textbox.Enabled = false;
                popup.evalue3_textbox.Enabled = false;
                popup.bitscore3_textbox.Enabled = false;
                popup.queryLength3_textbox.Enabled = false;
                popup.alignmentLength3_textbox.Enabled = false;

                popup.percent4_textbox.Enabled = false;
                popup.evalue4_textbox.Enabled = false;
                popup.bitscore4_textbox.Enabled = false;
                popup.queryLength4_textbox.Enabled = false;
                popup.alignmentLength4_textbox.Enabled = false;
                
                
            }
            DialogResult dialogresult = popup.ShowDialog();
            if (start_button.Enabled && dialogresult == DialogResult.OK)
            {
                //collecting the values from the popup window
                _prefix0 = popup.prefix_step1_textbox.Text;
                _prefix1 = popup.prefix_step2_textbox.Text;
                _prefix2 = popup.prefix_step3_textbox.Text;
                _prefix3 = popup.prefix_step4_textbox.Text;

                _preprocessing = popup.preprocessing_checkbox.Checked;
                _noaa = popup.noaa_textbox.Text;
                _minSeqLength = popup.minSeqLength_textbox.Text;

                _cdhitPercentageHomolog = popup.cdhit_param_scaffold_textbox.Text;
                _cdhitPercentageStep0 = popup.cdhit_param_step1_textbox.Text;
                _cdhitPercentageStep1 = popup.cdhit_param_step2_textbox.Text;
                _cdhitPercentageStep2 = popup.cdhit_param_step3_textbox.Text;

                _percentage1 = popup.percent2_textbox.Text;
                _evalue1 = popup.evalue2_textbox.Text;
                _bitscore1 = popup.bitscore2_textbox.Text;
                _queryLength1 = popup.queryLength2_textbox.Text;
                _alignmentLength1 = popup.alignmentLength2_textbox.Text;

                _percentage2 = popup.percent3_textbox.Text;
                _evalue2 = popup.evalue3_textbox.Text;
                _bitscore2 = popup.bitscore3_textbox.Text;
                _queryLength2 = popup.queryLength3_textbox.Text;
                _alignmentLength2 = popup.alignmentLength3_textbox.Text;

                _percentage3 = popup.percent4_textbox.Text;
                _evalue3 = popup.evalue4_textbox.Text;
                _bitscore3 = popup.bitscore4_textbox.Text;
                _queryLength3 = popup.queryLength4_textbox.Text;
                _alignmentLength3 = popup.alignmentLength4_textbox.Text;
            }
            popup.Dispose();
            
            
        }
        //
        //start button
        //
        private void start_button_Click(object sender, EventArgs e)
        {
            //check for red textboxes
            string validity = "";
            List<string> emptyFileList = new List<string>();
            List<string> blastFiles = new List<string>();
            blastFiles.Add("blastp.exe");
            blastFiles.Add("makeblastdb.exe");
            List<string> cdhitFiles = new List<string>();
            string storageMessage = IsValidPath(storage_textbox, "storage folder", emptyFileList, false);
            string blastMessage = IsValidPath(blast_textbox, "blast folder", blastFiles, true);
            string cdhitMessage = IsValidPath(cdhit_textbox, "cd-hit folder", cdhitFiles, true);
            string seqSetMessage = IsFastaFile(input_textbox, "sequence set");
            string homologSetMessage = IsFastaFile(scaffold_textbox, "homolog sequence set");

            if (_warningRegex.Match(storageMessage).Success || _warningRegex.Match(blastMessage).Success ||
                _warningRegex.Match(cdhitMessage).Success || _warningRegex.Match(seqSetMessage).Success ||
                _warningRegex.Match(homologSetMessage).Success)
            {
                start_button.Enabled = false;

                if (_warningRegex.Match(storageMessage).Success)
                {
                    validity += _warningRegex.Match(storageMessage).Groups[1] + "\n\n";
                }
                if (_warningRegex.Match(blastMessage).Success)
                {
                    validity += _warningRegex.Match(blastMessage).Groups[1] + "\n\n";
                }
                if (_warningRegex.Match(cdhitMessage).Success)
                {
                    validity += _warningRegex.Match(cdhitMessage).Groups[1] + "\n\n";
                }
                if (_warningRegex.Match(seqSetMessage).Success)
                {
                    validity += _warningRegex.Match(seqSetMessage).Groups[1] + "\n\n";
                }
                if (_warningRegex.Match(homologSetMessage).Success)
                {
                    validity += _warningRegex.Match(homologSetMessage).Groups[1] + "\n\n";
                }
                MessageBox.Show(validity, PathValidationWarningTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            else
            {
                //check if folder alread contains NOmESS output
                if (Directory.Exists(storage_textbox.Text + "\\NOmESS_Output"))
                {
                    string message =
                        "Storage folder already contains a NOmESS output.\nDo you want to overwrite it and continue?";
                    var confirmResult = MessageBox.Show(message, InformationTitel, MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.No){}
                    else
                    {
                        if (backgroundWorkerProgressBar.IsBusy == false && backgroundWorkerProcess.IsBusy == false)
                        {
                            DisableMainFormOptions();
                            backgroundWorkerProcess.RunWorkerAsync();
                            //progressBar1.Style = ProgressBarStyle.Marquee;
                            //progressBar1.MarqueeAnimationSpeed = 18;
                            //progressBar1.Value += 50;
                            //progressBar1.Show();
                            //backgroundWorker1.RunWorkerAsync();
                            //backgroundWorkerProgressBar.RunWorkerAsync();
                            
                            //Workflow logic = new Workflow();
                            //_flow.Progress += new Workflow.ProgressDelegate(DisplayProgess);
                            //_flow.StartWorkflow();
                        }
                    } //end else overwrite
                }
                else
                {
                    if (backgroundWorkerProgressBar.IsBusy == false && backgroundWorkerProcess.IsBusy == false)
                    {
                        DisableMainFormOptions();
                        backgroundWorkerProcess.RunWorkerAsync();
                        //progressBar1.Style = ProgressBarStyle.Marquee;
                        //progressBar1.MarqueeAnimationSpeed = 18;
                        //backgroundWorker1.RunWorkerAsync();
                        //backgroundWorkerProgressBar.RunWorkerAsync();
                        
                        //Workflow logic = new Workflow();
                        //_flow.Progress += new Workflow.ProgressDelegate(DisplayProgess);
                        //_flow.StartWorkflow();
                    }
                }//end else paths are fine
            }
        }//end method

//
//
//-----------------------------------------------------------------------------------------------
//METHODS
//
//
        //folderDialog
        private string IsValidPath(TextBox txtbox, string fileSpecification, List<string> fileNames, Boolean needFiles)
        {
            string message = "";

            if (String.IsNullOrEmpty(txtbox.Text) || !(Directory.Exists(txtbox.Text)))
            {
                txtbox.BackColor = Color.Red;
                start_button.Enabled = false;
                message="Warning:\nPath to the "+fileSpecification+" is missing or wrong!\nPlease enter the correct path.";
            }
            else
            {
                if (needFiles)
                {
                    Boolean first = true;
                    Boolean printMessage = false;
                    string missingFiles = "";
                    string verb = "";
                    foreach (string file in fileNames)
                    {
                        if (!(File.Exists(txtbox.Text + "\\" + file)))
                        {
                            printMessage = true;
                            txtbox.BackColor = Color.Red;
                            start_button.Enabled = false;
                            if (first)
                            {
                                missingFiles += file;
                                verb = " does";
                                first = false;
                            }
                            else
                            {
                                missingFiles += " and " + file;
                                verb = " do";
                            }
                        }

                    } //end foreach

                    if (printMessage)
                    { 
                        message = "Warning:\n The entered folder does not contain " + missingFiles + "!" + "\nPlease enter the correct folder.";
                        
                    }
                   
                }//end if needFiles
                else
                {
                    txtbox.BackColor = Color.White;
                    start_button.Enabled = true;
                }
            }
            return message;
        }
        
        //file dialog
        private string IsFastaFile(TextBox txtbox, string fileSpecification)
        {
            Regex fastaHeader = new Regex("^>.*");
            String line = "";
            int numberSequences = 0;
            string message = "";

            //no path or file does not exist
            if (String.IsNullOrEmpty(txtbox.Text) || !(File.Exists(txtbox.Text)))
            {
                txtbox.BackColor = Color.Red;
                start_button.Enabled = false;
                message="Warning:\nPath for "+fileSpecification+" is missing or wrong!\nPlease enter a correct path.";
            }
            //read in file and check if it is fasta format --> cursor to wait
            else
            {
                //set the current cursor to the wait and continue
                Cursor currentCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    StreamReader inFile = new StreamReader(txtbox.Text);
                    while ((line = inFile.ReadLine()) != null)
                    {
                        line = line.Replace("\n", "");
                        Match mHeader = fastaHeader.Match(line);

                        //check if line is a fasta header
                        if (mHeader.Success)
                        {
                            numberSequences++;
                        }
                    } //end while

                    if(numberSequences==0)
                    {
                        txtbox.BackColor = Color.Red;
                        start_button.Enabled = false;
                        message="Warning:\nThe file for the "+fileSpecification+" you entered is not in fasta format!\nPlease enter file with the correct format.";
                    }
                    else
                    {
                        txtbox.BackColor = Color.White;
                        start_button.Enabled = true;
                        message="Information:\nThe "+fileSpecification+" contains "+numberSequences+" sequences";
                    }
                }//end try
                finally
                {
                    // Swap the current cursor back to the original cursor
                    Cursor.Current = currentCursor;
                }//end finally
            }//end else
            return message;
        }//end IsFastaFile
        
        //validate file path
        private void ValidateFastaFile(TextBox txtbox, string boxDescription)
        {
            string message = "";
            string shortenedMessage = "";

            message = IsFastaFile(txtbox, boxDescription);
            if (_warningRegex.Match(message).Success)
            {
                shortenedMessage += _warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, PathValidationWarningTitel, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                shortenedMessage += _informationRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, InformationTitel, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }
        
        //start run
        private void StartNomess()
        {
            //set the current cursor to the wait and continue
            //setting all the parameters, because input is fine
            string storagePath = storage_textbox.Text;
            string blastPath = blast_textbox.Text;
            string cdhitPath = cdhit_textbox.Text;
            string seqsetPath = input_textbox.Text;
            string homologPath = scaffold_textbox.Text;

            if (!(storagePath.Last().Equals("\\")))
            {
                storagePath += "\\";
            }
            if (!(blastPath.Last().Equals("\\")))
            {
                blastPath += "\\";
            }
            if (!cdhitPath.Last().Equals("\\"))
            {
                cdhitPath += "\\";
            }

            _flow.SetRequiredParameters(storagePath, blastPath, cdhitPath, seqsetPath, homologPath);

            _flow.SetOptionalParameters(_prefix0, _prefix1, _prefix2, _prefix3,
                                    _preprocessing, _noaa, Convert.ToInt32(_minSeqLength),
                                    Convert.ToDouble(_cdhitPercentageHomolog),
                                    Convert.ToDouble(_cdhitPercentageStep0),
                                    Convert.ToDouble(_cdhitPercentageStep1),
                                    Convert.ToDouble(_cdhitPercentageStep2),
                                    Convert.ToDouble(_percentage1), Convert.ToDouble(_evalue1),
                                    Convert.ToDouble(_bitscore1), Convert.ToInt32(_queryLength1),
                                    Convert.ToInt32(_alignmentLength1),
                                    Convert.ToDouble(_percentage2), Convert.ToDouble(_evalue2),
                                    Convert.ToDouble(_bitscore2), Convert.ToInt32(_queryLength2),
                                    Convert.ToInt32(_alignmentLength2),
                                    Convert.ToDouble(_percentage3), Convert.ToDouble(_evalue3),
                                    Convert.ToDouble(_bitscore3), Convert.ToInt32(_queryLength3),
                                    Convert.ToInt32(_alignmentLength3)
            );
            _flow.Progress += DisplayProgess;
            _flow.StartWorkflow();
            // Swap the current cursor back to the original cursor

        }

        //disable buttons when process is running
        private void DisableMainFormOptions()
        {
            //main form
            start_button.Enabled = false;
            storage_browse.Enabled = false;
            blast_browse.Enabled = false;
            cdhit_browse.Enabled = false;
            input_browse.Enabled = false;
            scaffold_browse.Enabled = false;
            storage_textbox.Enabled = false;
            blast_textbox.Enabled = false;
            cdhit_textbox.Enabled = false;
            input_textbox.Enabled = false;
            scaffold_textbox.Enabled = false;
        }

        //update progress bar according to progress in Workflow
        public void DisplayProgess(string message, int percent)
        {
            if (InvokeRequired)
            {
                Invoke(new Workflow.ProgressDelegate(DisplayProgess), new Object[] { message, percent });
            }
            else
            {
                label3.Text = message;
                progressBar1.Value = percent;
                progressBar1.Show();
            }
        }

        private void BackgroundWorkerProcess_DoWork(object sender, DoWorkEventArgs e)
        {
            StartNomess();
        }




        /*
        private void timer1_Tick(object sender, EventArgs e){}

        private void BackgroundWorkerProgressBar_DoWork(object sender, DoWorkEventArgs e)
        {
            //progressBar1.Value += _progress;
            //progressBar1.Show();
            //backgroundWorkerProgressBar.ReportProgress(50);
        }

        private void BackgroundWorkerProgressBar_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            progressBar1.Value = e.ProgressPercentage;

        }
        */
        

        
    }
}

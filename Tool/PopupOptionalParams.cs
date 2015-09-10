using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Tool
{
    public partial class PopupOptionalParams : Form
    {
        private String _noaa;
        private String _minSeqLength;

        //for message boxes
        Regex warningRegex = new Regex("Warning:\n(.*)");
        Regex informationRegex=new Regex("Information:\n(.*)");
        private String _warningTitle = "Wrong Parameter Setting Warning";
        private String _informationTitel = "Information";
        private List<TextBox> prefixList;
        private List<TextBox> cdhitList;
        private List<TextBox> blastList; 
       
        public PopupOptionalParams()
        {
            InitializeComponent();
            prefixList = new List<TextBox>
                             {prefix_step1_textbox, prefix_step2_textbox, prefix_step3_textbox, prefix_step4_textbox};
            cdhitList = new List<TextBox>
                            {
                                cdhit_param_scaffold_textbox,
                                cdhit_param_step1_textbox,
                                cdhit_param_step2_textbox,
                                cdhit_param_step3_textbox
                            };
            blastList = new List<TextBox>
                            {
                                percent2_textbox,
                                evalue2_textbox,
                                bitscore2_textbox,
                                queryLength2_textbox,
                                alignmentLength2_textbox,
                                percent3_textbox,
                                evalue3_textbox,
                                bitscore3_textbox,
                                queryLength3_textbox,
                                alignmentLength3_textbox,
                                percent4_textbox,
                                evalue4_textbox,
                                bitscore4_textbox,
                                queryLength4_textbox,
                                alignmentLength4_textbox
                            };
        }

        //
        //load form popup
        //
        private void PopupOptionalParams_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();  
            //tab 1
            ToolTip1.SetToolTip(prefix1_label, "Fasta header prefix of sequences after step 1.");
            ToolTip1.SetToolTip(prefix2_label, "Fasta header prefix of sequences after step 2.");
            ToolTip1.SetToolTip(prefix3_label, "Fasta header prefix of sequences after step 3.");
            ToolTip1.SetToolTip(prefix4_label, "Fasta header prefix of sequences after step 4.");
            //tab 2
            ToolTip1.SetToolTip(preprocessing_label, "Whether preprocessing step should be applied.");
            ToolTip1.SetToolTip(noaa_label,          "Non amino acid characters in the input sequences for preprocessing step. \nSequences are cut before and after these characters.");
            //tab 3
            ToolTip1.SetToolTip(cdhit_scaffold_label, "Identity threshold for clustering scaffold set.");
            ToolTip1.SetToolTip(cdhit_step1_label,   "Identity threshold for clustering sequence set after step 1.");
            ToolTip1.SetToolTip(cdhit_step2_label,   "Identity threshold for clustering sequence set after step 2.");
            ToolTip1.SetToolTip(cdhit_step3_label,   "Identity threshold for clustering sequence set after step 3.");
            //tab 4
            ToolTip1.SetToolTip(blast2_label,   "Filtering parameters for blast hits for step 2.");
            ToolTip1.SetToolTip(percent2_label, "Minimal percentage identity of a blast hit.");
            ToolTip1.SetToolTip(evalue2_label,    "Minimal e-value of a blast.");
            ToolTip1.SetToolTip(bitscore2_label,"Minimal bit score of a blast hit.");
            ToolTip1.SetToolTip(queryLength2_label,  "Minimal query length of a blast hit.");
            ToolTip1.SetToolTip(alignmentLength2_label,    "Minimal alignment length of a blast hit.");

            ToolTip1.SetToolTip(blast3_label,   "Filtering parameters for blast hits for step 3.");
            ToolTip1.SetToolTip(percent3_label, "Minimal percentage identity of a blast hit.");
            ToolTip1.SetToolTip(evalue3_label,    "Minimal e-value of a blast.");
            ToolTip1.SetToolTip(bitscore3_label,"Minimal bit score of a blast hit.");
            ToolTip1.SetToolTip(queryLength3_label,  "Minimal query length of a blast hit.");
            ToolTip1.SetToolTip(alignmentLength3_label,    "Minimal alignment length of a blast hit.");

            ToolTip1.SetToolTip(blast4_label,   "Filtering parameters for blast hits for step 4.");
            ToolTip1.SetToolTip(percent4_label, "Minimal percentage identity of a blast hit.");
            ToolTip1.SetToolTip(evalue4_label,    "Minimal e-value of a blast.");
            ToolTip1.SetToolTip(bitscore4_label,"Minimal bit score of a blast hit.");
            ToolTip1.SetToolTip(queryLength4_label,  "Minimal query length of a blast hit.");
            ToolTip1.SetToolTip(alignmentLength4_label,    "Minimal alignment length of a blast hit.");
        }
        
        //
        //tab 1: id prefixes
        //
        private void prefix_step0_textbox_TextChanged(object sender, EventArgs e){}
        private void prefix_step0_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(prefixList);
            prefix_step1_textbox.BackColor = Color.White;
        }
        private void prefix_step0_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckIdPrefix(prefix_step1_textbox, prefix_step2_textbox, prefix_step3_textbox,
                                           prefix_step4_textbox);
            if(warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage+=warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void prefix_step1_textbox_TextChanged(object sender, EventArgs e){}
        private void prefix_step1_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(prefixList);
            prefix_step2_textbox.BackColor = Color.White;
        }
        private void prefix_step1_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckIdPrefix(prefix_step2_textbox, prefix_step3_textbox, prefix_step4_textbox, prefix_step1_textbox);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void prefix_step2_textbox_TextChanged(object sender, EventArgs e){}
        private void prefix_step2_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(prefixList);
            prefix_step3_textbox.BackColor = Color.White;
        }
        private void prefix_step2_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckIdPrefix(prefix_step3_textbox, prefix_step4_textbox, prefix_step1_textbox, prefix_step2_textbox);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void prefix_step3_textbox_TextChanged(object sender, EventArgs e){}
        private void prefix_step3_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(prefixList);
            prefix_step4_textbox.BackColor = Color.White;
        }
        private void prefix_step3_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckIdPrefix(prefix_step4_textbox, prefix_step1_textbox, prefix_step2_textbox, prefix_step3_textbox);
            if (warningRegex.Match(message).Success)
                if (warningRegex.Match(message).Success)
                {
                    string shortenedMessage = "";
                    shortenedMessage += warningRegex.Match(message).Groups[1];
                    MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
        }
        //
        //tab 2: preprocessing
        //
        private void preproc_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            _noaa = "X;*";
            _minSeqLength = "100";

            if (!(preprocessing_checkbox.Checked))
            {
                noaa_textbox.Clear();
                _minSeqLength = "0";                
            }
            else
            {
                noaa_textbox.Text=_noaa;                
            }
            minSeqLength_textbox.Text = _minSeqLength;
        }

        private void noaa_textbox_TextChanged(object sender, EventArgs e){}
        private void noaa_textbox_Enter(object sender, EventArgs e)
        {
            noaa_textbox.BackColor = Color.White;
        }
        private void noaa_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNonAminoAcid(noaa_textbox);
            if (!(warningRegex.Match(message).Success) && message != "")
            {
                string shortenedMessage = "";
                shortenedMessage += informationRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _informationTitel, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void minSeqLength_textbox_TextChanged(object sender, EventArgs e){}
        private void minSeqLength_textbox_Enter(object sender, EventArgs e)
        {
            minSeqLength_textbox.BackColor = Color.White;
        }
        private void minSeqLength_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(minSeqLength_textbox, false);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        //
        //tab 3: cdhit percentages
        //
        private void cdhit_param_homolog_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(cdhitList);
            cdhit_param_scaffold_textbox.BackColor = Color.White;
        }
        private void cdhit_param_homolog_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(cdhit_param_scaffold_textbox,true);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void cdhit_param_homolog_TextChanged(object sender, EventArgs e){}
        
        private void cdhit_param_step0_TextChanged(object sender, EventArgs e){}
        private void cdhit_param_step0_Enter(object sender, EventArgs e)
        {
            EnableOkButton(cdhitList);
            cdhit_param_step1_textbox.BackColor = Color.White;
        }
        private void cdhit_param_step0_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(cdhit_param_step1_textbox, true);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cdhit_param_step1_TextChanged(object sender, EventArgs e){}
        private void cdhit_param_step1_Enter(object sender, EventArgs e)
        {
            EnableOkButton(cdhitList);
            cdhit_param_step2_textbox.BackColor = Color.White;
        }
        private void cdhit_param_step1_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(cdhit_param_step2_textbox, true);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cdhit_param_step2_TextChanged(object sender, EventArgs e){}
        private void cdhit_param_step2_Enter(object sender, EventArgs e)
        {
            EnableOkButton(cdhitList);
            cdhit_param_step3_textbox.BackColor = Color.White;
        }
        private void cdhit_param_step2_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(cdhit_param_step3_textbox, true);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //
        //tab 4: blast parameters
        //
        private void percent1_textbox_TextChanged(object sender, EventArgs e){}
        private void percent1_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(blastList);
            percent2_textbox.BackColor = Color.White;
        }
        private void percent1_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(percent2_textbox,true);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void eval1_textbox_TextChanged(object sender, EventArgs e){}
        private void eval1_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(blastList);
            evalue2_textbox.BackColor = Color.White;
        }
        private void eval1_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(evalue2_textbox, false);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void bitscore1_textbox_TextChanged(object sender, EventArgs e){}
        private void bitscore1_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(blastList);
            bitscore2_textbox.BackColor = Color.White;
        }
        private void bitscore1_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(bitscore2_textbox, false);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void queryL1_textbox_TextChanged(object sender, EventArgs e){}
        private void queryL1_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(blastList);
            queryLength2_textbox.BackColor = Color.White;
        }
        private void queryL1_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(queryLength2_textbox, false);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void aliL1_textbox_TextChanged(object sender, EventArgs e){}
        private void aliL1_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(blastList);
            alignmentLength2_textbox.BackColor = Color.White;
        }
        private void aliL1_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(alignmentLength2_textbox, false);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void percent2_textbox_TextChanged(object sender, EventArgs e){}
        private void percent2_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(blastList);
            percent3_textbox.BackColor = Color.White;
        }
        private void percent2_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(percent3_textbox,true);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void eval2_textbox_TextChanged(object sender, EventArgs e){}
        private void eval2_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(blastList);
            evalue3_textbox.BackColor = Color.White;
        }
        private void eval2_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(evalue3_textbox, false);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void bitscore2_textbox_TextChanged(object sender, EventArgs e){}
        private void bitscore2_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(blastList);
            bitscore3_textbox.BackColor = Color.White;
        }
        private void bitscore2_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(bitscore3_textbox, false);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void queryL2_textbox_TextChanged(object sender, EventArgs e){}
        private void queryL2_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(blastList);
            queryLength3_textbox.BackColor = Color.White;
        }
        private void queryL2_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(queryLength3_textbox, false);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void aliL2_textbox_TextChanged(object sender, EventArgs e){}
        private void aliL2_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(blastList);
            alignmentLength3_textbox.BackColor = Color.White;
        }
        private void aliL2_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(alignmentLength3_textbox, false);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void percent3_textbox_TextChanged(object sender, EventArgs e){}
        private void percent3_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(blastList);
            percent4_textbox.BackColor = Color.White;
        }
        private void percent3_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(percent4_textbox,true);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void eval3_textbox_TextChanged(object sender, EventArgs e){}
        private void eval3_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(blastList);
            evalue4_textbox.BackColor = Color.White;
        }
        private void eval3_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(evalue4_textbox, false);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void bitscore3_textbox_TextChanged(object sender, EventArgs e){}
        private void bitscore3_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(blastList);
            bitscore4_textbox.BackColor = Color.White;
        }
        private void bitscore3_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(bitscore4_textbox, false);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void queryL3_textbox_TextChanged(object sender, EventArgs e){}
        private void queryL3_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(blastList);
            queryLength4_textbox.BackColor = Color.White;
        }
        private void queryL3_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(queryLength4_textbox, false);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void aliL3_textbox_TextChanged(object sender, EventArgs e){}
        private void aliL3_textbox_Enter(object sender, EventArgs e)
        {
            EnableOkButton(blastList);
            alignmentLength4_textbox.BackColor = Color.White;
        }
        private void aliL3_textbox_Leave(object sender, EventArgs e)
        {
            string message = CheckNumberValues(alignmentLength4_textbox, false);
            if (warningRegex.Match(message).Success)
            {
                string shortenedMessage = "";
                shortenedMessage += warningRegex.Match(message).Groups[1];
                MessageBox.Show(shortenedMessage, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //
        //reset buttons -> get default values back
        //
        private void prefixes_reset_button_Click(object sender, EventArgs e){DefaultPrefixes(prefix_step1_textbox,prefix_step2_textbox,prefix_step3_textbox,prefix_step4_textbox);}
        private void preprocessing_reset_button1_Click(object sender, EventArgs e){DefaultPreprocessing(preprocessing_checkbox,noaa_textbox,minSeqLength_textbox);}
        private void cdhit_reset_button_Click(object sender, EventArgs e){DefaultCdhit(cdhit_param_scaffold_textbox,cdhit_param_step1_textbox,cdhit_param_step2_textbox,cdhit_param_step3_textbox);}
        private void blast_reset_button_Click(object sender, EventArgs e)
        {
            DefaultBlast(percent2_textbox, evalue2_textbox, bitscore2_textbox, queryLength2_textbox, alignmentLength2_textbox);
            DefaultBlast(percent3_textbox, evalue3_textbox, bitscore3_textbox, queryLength3_textbox, alignmentLength3_textbox);
            DefaultBlast(percent4_textbox, evalue4_textbox, bitscore4_textbox, queryLength4_textbox, alignmentLength4_textbox);
        }
        private void resetAll_button_Click(object sender, EventArgs e)
        {
            DefaultPrefixes(prefix_step1_textbox, prefix_step2_textbox, prefix_step3_textbox, prefix_step4_textbox);
            DefaultPreprocessing(preprocessing_checkbox, noaa_textbox,minSeqLength_textbox);
            DefaultCdhit(cdhit_param_scaffold_textbox, cdhit_param_step1_textbox, cdhit_param_step2_textbox, cdhit_param_step3_textbox);
            DefaultBlast(percent2_textbox, evalue2_textbox, bitscore2_textbox, queryLength2_textbox, alignmentLength2_textbox);
            DefaultBlast(percent3_textbox, evalue3_textbox, bitscore3_textbox, queryLength3_textbox, alignmentLength3_textbox);
            DefaultBlast(percent4_textbox, evalue4_textbox, bitscore4_textbox, queryLength4_textbox, alignmentLength4_textbox);
        }
        //
        //ok button -> quit form
        //
        private void ok_button_Click(object sender, EventArgs e)
        {
            
        }
        //
        //
        //---------------------------------------------------------------------------------------------
        //METHODS
        //
        //
        //check prefix 
        private string CheckIdPrefix(TextBox txtboxCurrent, TextBox txtBoxRest1, TextBox txtBoxRest2, TextBox txtBoxRest3)
        {
            List<TextBox> boxes = new List<TextBox> {txtBoxRest1, txtBoxRest2, txtBoxRest3 };
            Dictionary<TextBox,String> boxDict = new Dictionary<TextBox,String>();
            boxDict.Add(txtBoxRest1, txtBoxRest1.Text);
            boxDict.Add(txtBoxRest2, txtBoxRest2.Text);
            boxDict.Add(txtBoxRest3, txtBoxRest3.Text);
            string message = "";
            //empty string
            if (String.IsNullOrEmpty(txtboxCurrent.Text))
            {
                txtboxCurrent.BackColor = Color.Red;
                ok_button.Enabled = false;
                message = "Warning:\nEmpty string as prefix is not permitted!";
            }
            //same strings
            else if (boxDict.ContainsValue(txtboxCurrent.Text))
            {
                foreach (TextBox box in boxDict.Keys)
                {
                    if (box.Text == txtboxCurrent.Text)
                    {
                        box.BackColor = Color.Red;
                    }
                }
                txtboxCurrent.BackColor = Color.Red;
                ok_button.Enabled = false;
                message = "Warning:\nSame string for different prefixes is not permitted!";
            }
            else
            {
                txtboxCurrent.BackColor = Color.White;
                //txtBoxRest1.BackColor = Color.White;
                //txtBoxRest2.BackColor = Color.White;
                //txtBoxRest3.BackColor = Color.White;
                ok_button.Enabled = true;
            }
            return message;
        }
        //
        //check no amino acid character
        private string CheckNonAminoAcid(TextBox txtbox)
        {
            string message = "";
            //character entered is amino acid
            if (((new Regex("[a,c-i,k-n,p-t,v,w,y,A,C-I,K-N,P-T,V,W,Y]")).Match(txtbox.Text)).Success)
            {
                txtbox.BackColor = Color.Red;
                //ok_button.Enabled = false;
                message = "Information:\nAmong thr entered characters are amino acid(s)!";
                //MessageBox.Show(message, _informationTitel, MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtbox.BackColor = Color.White;
            }
            return message;
        }
        //
        //check double values -> cd-hit parameter (percentage) and blast parameters
        //
        private string CheckNumberValues(TextBox txtbox, Boolean percentage)
        {
            string message = "";
            //empty
            if (String.IsNullOrEmpty(txtbox.Text))
            {
                txtbox.BackColor = Color.Red;
                ok_button.Enabled = false;
                message = "Warning:\nEmpty string as value is not permitted!\nInput must be a number!";
                //MessageBox.Show(message, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //letter 
            else if ((((new Regex("[^0-9\\+-\\.e]")).Match(txtbox.Text)).Success))
            {
                txtbox.BackColor = Color.Red;
                ok_button.Enabled = false;
                message = "Warning:\nInput must be a number!";
                //MessageBox.Show(message, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            //is number
            else
            {
                if (percentage)
                {
                    //not between 0 & 1
                    if ((((new Regex("-")).Match(txtbox.Text)).Success) || (Convert.ToDouble(txtbox.Text) < 0) || (Convert.ToDouble(txtbox.Text) > 1))
                    {
                        txtbox.BackColor = Color.Red;
                        ok_button.Enabled = false;
                        message = "Warning:\nPercentage must be a value between 0 and 1";
                        //MessageBox.Show(message, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    //fine
                    else if (Convert.ToDouble(txtbox.Text) >= 0 && Convert.ToDouble(txtbox.Text) <= 1)
                    {
                        txtbox.BackColor = Color.White;
                        ok_button.Enabled = true;
                    }
                }
                //no percentage value
                else
                {
                    //negative number
                    if(Convert.ToDouble(txtbox.Text) < 0)
                    {
                        txtbox.BackColor = Color.Red;
                        ok_button.Enabled = false;
                        message = "Warning:\nValue must be greater or equal 0";
                        //MessageBox.Show(message, _warningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        txtbox.BackColor = Color.White;
                        ok_button.Enabled = true;
                    }
                }
            }
            return message;
        }
        //
        //reset prefixes
        //
        private void DefaultPrefixes(TextBox pref0, TextBox pref1, TextBox pref2, TextBox pref3)
        {
            pref0.Text = @"ID_step1_"; pref0.BackColor = Color.White;
            pref1.Text = @"ID_step2_"; pref1.BackColor = Color.White;
            pref2.Text = @"ID_step3_"; pref2.BackColor = Color.White;
            pref3.Text = @"ID_step4_"; pref3.BackColor = Color.White;
            ok_button.Enabled = true;
        }
        //
        //reset preprocessing
        //
        private void DefaultPreprocessing(CheckBox preproc, TextBox noaaCharacters, TextBox minSeqLength)
        {
            preproc.Checked = true;
            noaaCharacters.Text = @"X;*";noaaCharacters.BackColor = Color.White;
            minSeqLength.Text = "100";minSeqLength.BackColor = Color.White;
            ok_button.Enabled = true;
        }
        //
        //reset cdhit
        //
        private void DefaultCdhit(TextBox homo, TextBox cdhit0, TextBox cdhit1, TextBox cdhit2)
        {
            homo.Text = @"0.95";homo.BackColor = Color.White;
            cdhit0.Text = @"1.0";cdhit0.BackColor = Color.White;
            cdhit1.Text = @"0.95"; cdhit1.BackColor = Color.White;
            cdhit2.Text = @"0.95"; cdhit2.BackColor = Color.White;
            ok_button.Enabled = true;
        }
        //
        //reset blast
        //
        private void DefaultBlast(TextBox percentage, TextBox evalue, TextBox bitscore, TextBox queryLength, TextBox alignmentLength)
       {
           percentage.Text = @"0.6";percentage.BackColor = Color.White;
           evalue.Text = @"10e-10"; evalue.BackColor = Color.White;
           bitscore.Text = @"100"; bitscore.BackColor = Color.White;
           queryLength.Text = @"100"; queryLength.BackColor = Color.White;
           alignmentLength.Text = @"100"; alignmentLength.BackColor = Color.White;
           ok_button.Enabled = true;
       }
        //
        ////enable or diable the OK button when entering a textbox
        //
        private void EnableOkButton(List<TextBox> boxes )
        {
            Boolean enable = true;
            foreach (TextBox txtBox in boxes)
            {
                if (txtBox.BackColor == Color.Red)
                {
                    enable = false;
                    break;
                }
            }//end foreach
            ok_button.Enabled = enable;
        }




    }
}

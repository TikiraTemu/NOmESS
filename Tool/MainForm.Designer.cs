using System.ComponentModel;

namespace Tool
{
    public partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.storage_textbox = new System.Windows.Forms.TextBox();
            this.storage_label = new System.Windows.Forms.Label();
            this.storage_browse = new System.Windows.Forms.Button();
            this.blast_browse = new System.Windows.Forms.Button();
            this.blast_label = new System.Windows.Forms.Label();
            this.blast_textbox = new System.Windows.Forms.TextBox();
            this.start_button = new System.Windows.Forms.Button();
            this.optional_input_button = new System.Windows.Forms.Button();
            this.cdhit_browse = new System.Windows.Forms.Button();
            this.cdhit_label = new System.Windows.Forms.Label();
            this.cdhit_textbox = new System.Windows.Forms.TextBox();
            this.scaffold_browse = new System.Windows.Forms.Button();
            this.homolog_label = new System.Windows.Forms.Label();
            this.scaffold_textbox = new System.Windows.Forms.TextBox();
            this.input_browse = new System.Windows.Forms.Button();
            this.original_label = new System.Windows.Forms.Label();
            this.input_textbox = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorkerProgressBar = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerProcess = new System.ComponentModel.BackgroundWorker();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Required input";
            // 
            // storage_textbox
            // 
            this.storage_textbox.Location = new System.Drawing.Point(135, 39);
            this.storage_textbox.Name = "storage_textbox";
            this.storage_textbox.Size = new System.Drawing.Size(414, 20);
            this.storage_textbox.TabIndex = 1;
            this.storage_textbox.TextChanged += new System.EventHandler(this.storage_textbox_TextChanged);
            this.storage_textbox.Leave += new System.EventHandler(this.storage_textbox_Leave);
            this.storage_textbox.Enter += new System.EventHandler(this.storage_textbox_Enter);
            // 
            // storage_label
            // 
            this.storage_label.AutoSize = true;
            this.storage_label.Location = new System.Drawing.Point(19, 46);
            this.storage_label.Name = "storage_label";
            this.storage_label.Size = new System.Drawing.Size(84, 13);
            this.storage_label.TabIndex = 2;
            this.storage_label.Text = "Storage location";
            // 
            // storage_browse
            // 
            this.storage_browse.Location = new System.Drawing.Point(583, 36);
            this.storage_browse.Name = "storage_browse";
            this.storage_browse.Size = new System.Drawing.Size(71, 25);
            this.storage_browse.TabIndex = 3;
            this.storage_browse.Text = "Browse";
            this.storage_browse.UseVisualStyleBackColor = true;
            this.storage_browse.Click += new System.EventHandler(this.storage_browse_button_Click);
            // 
            // blast_browse
            // 
            this.blast_browse.Location = new System.Drawing.Point(583, 62);
            this.blast_browse.Name = "blast_browse";
            this.blast_browse.Size = new System.Drawing.Size(71, 25);
            this.blast_browse.TabIndex = 6;
            this.blast_browse.Text = "Browse";
            this.blast_browse.UseVisualStyleBackColor = true;
            this.blast_browse.Click += new System.EventHandler(this.blastp_browse_button_Click);
            // 
            // blast_label
            // 
            this.blast_label.AutoSize = true;
            this.blast_label.Location = new System.Drawing.Point(19, 72);
            this.blast_label.Name = "blast_label";
            this.blast_label.Size = new System.Drawing.Size(47, 13);
            this.blast_label.TabIndex = 5;
            this.blast_label.Tag = "";
            this.blast_label.Text = "BLASTp";
            // 
            // blast_textbox
            // 
            this.blast_textbox.Location = new System.Drawing.Point(135, 65);
            this.blast_textbox.Name = "blast_textbox";
            this.blast_textbox.Size = new System.Drawing.Size(414, 20);
            this.blast_textbox.TabIndex = 4;
            this.blast_textbox.TextChanged += new System.EventHandler(this.blast_textbox_TextChanged);
            this.blast_textbox.Leave += new System.EventHandler(this.blast_textbox_Leave);
            this.blast_textbox.Enter += new System.EventHandler(this.blast_textbox_Enter);
            // 
            // start_button
            // 
            this.start_button.Location = new System.Drawing.Point(22, 232);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(97, 32);
            this.start_button.TabIndex = 7;
            this.start_button.Text = "Start";
            this.start_button.UseVisualStyleBackColor = true;
            this.start_button.Click += new System.EventHandler(this.start_button_Click);
            // 
            // optional_input_button
            // 
            this.optional_input_button.Location = new System.Drawing.Point(145, 233);
            this.optional_input_button.Name = "optional_input_button";
            this.optional_input_button.Size = new System.Drawing.Size(113, 31);
            this.optional_input_button.TabIndex = 8;
            this.optional_input_button.Text = "Optional input";
            this.optional_input_button.UseVisualStyleBackColor = true;
            this.optional_input_button.Click += new System.EventHandler(this.optionalParam_button_Click);
            // 
            // cdhit_browse
            // 
            this.cdhit_browse.Location = new System.Drawing.Point(583, 88);
            this.cdhit_browse.Name = "cdhit_browse";
            this.cdhit_browse.Size = new System.Drawing.Size(71, 25);
            this.cdhit_browse.TabIndex = 11;
            this.cdhit_browse.Text = "Browse";
            this.cdhit_browse.UseVisualStyleBackColor = true;
            this.cdhit_browse.Click += new System.EventHandler(this.cdhit_browse_button_Click);
            // 
            // cdhit_label
            // 
            this.cdhit_label.AutoSize = true;
            this.cdhit_label.Location = new System.Drawing.Point(19, 98);
            this.cdhit_label.Name = "cdhit_label";
            this.cdhit_label.Size = new System.Drawing.Size(34, 13);
            this.cdhit_label.TabIndex = 10;
            this.cdhit_label.Text = "Cd-hit";
            // 
            // cdhit_textbox
            // 
            this.cdhit_textbox.Location = new System.Drawing.Point(135, 91);
            this.cdhit_textbox.Name = "cdhit_textbox";
            this.cdhit_textbox.Size = new System.Drawing.Size(414, 20);
            this.cdhit_textbox.TabIndex = 9;
            this.cdhit_textbox.TextChanged += new System.EventHandler(this.cdhit_textbox_TextChanged);
            this.cdhit_textbox.Leave += new System.EventHandler(this.cdhit_textbox_Leave);
            this.cdhit_textbox.Enter += new System.EventHandler(this.cdhit_textbox_Enter);
            // 
            // scaffold_browse
            // 
            this.scaffold_browse.Location = new System.Drawing.Point(583, 140);
            this.scaffold_browse.Name = "scaffold_browse";
            this.scaffold_browse.Size = new System.Drawing.Size(71, 25);
            this.scaffold_browse.TabIndex = 17;
            this.scaffold_browse.Text = "Browse";
            this.scaffold_browse.UseVisualStyleBackColor = true;
            this.scaffold_browse.Click += new System.EventHandler(this.homolog_browse_button_Click);
            // 
            // homolog_label
            // 
            this.homolog_label.AutoSize = true;
            this.homolog_label.Location = new System.Drawing.Point(19, 150);
            this.homolog_label.Name = "homolog_label";
            this.homolog_label.Size = new System.Drawing.Size(63, 13);
            this.homolog_label.TabIndex = 16;
            this.homolog_label.Text = "Scaffold set";
            // 
            // scaffold_textbox
            // 
            this.scaffold_textbox.Location = new System.Drawing.Point(135, 143);
            this.scaffold_textbox.Name = "scaffold_textbox";
            this.scaffold_textbox.Size = new System.Drawing.Size(414, 20);
            this.scaffold_textbox.TabIndex = 15;
            this.scaffold_textbox.TextChanged += new System.EventHandler(this.homolog_textbox_TextChanged);
            this.scaffold_textbox.Leave += new System.EventHandler(this.homolog_textbox_Leave);
            this.scaffold_textbox.Enter += new System.EventHandler(this.homolog_textbox_Enter);
            // 
            // input_browse
            // 
            this.input_browse.Location = new System.Drawing.Point(583, 114);
            this.input_browse.Name = "input_browse";
            this.input_browse.Size = new System.Drawing.Size(71, 25);
            this.input_browse.TabIndex = 14;
            this.input_browse.Text = "Browse";
            this.input_browse.UseVisualStyleBackColor = true;
            this.input_browse.Click += new System.EventHandler(this.sequenceSet_browse_button_Click);
            // 
            // original_label
            // 
            this.original_label.AutoSize = true;
            this.original_label.Location = new System.Drawing.Point(19, 124);
            this.original_label.Name = "original_label";
            this.original_label.Size = new System.Drawing.Size(48, 13);
            this.original_label.TabIndex = 13;
            this.original_label.Text = "Input set";
            // 
            // input_textbox
            // 
            this.input_textbox.Location = new System.Drawing.Point(135, 117);
            this.input_textbox.Name = "input_textbox";
            this.input_textbox.Size = new System.Drawing.Size(414, 20);
            this.input_textbox.TabIndex = 12;
            this.input_textbox.TextChanged += new System.EventHandler(this.sequenceSet_textbox_TextChanged);
            this.input_textbox.Leave += new System.EventHandler(this.sequenceSet_textbox_Leave);
            this.input_textbox.Enter += new System.EventHandler(this.sequenceSet_textbox_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(592, 242);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Version 1.0";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(511, 241);
            this.progressBar1.MarqueeAnimationSpeed = 0;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(75, 14);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 19;
            this.progressBar1.UseWaitCursor = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 250;
            // 
            // backgroundWorkerProgressBar
            // 
            this.backgroundWorkerProgressBar.WorkerReportsProgress = true;
            // 
            // backgroundWorkerProcess
            // 
            this.backgroundWorkerProcess.WorkerReportsProgress = true;
            this.backgroundWorkerProcess.WorkerSupportsCancellation = true;
            this.backgroundWorkerProcess.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorkerProcess_DoWork);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(313, 242);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 20;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 266);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.scaffold_browse);
            this.Controls.Add(this.homolog_label);
            this.Controls.Add(this.scaffold_textbox);
            this.Controls.Add(this.input_browse);
            this.Controls.Add(this.original_label);
            this.Controls.Add(this.input_textbox);
            this.Controls.Add(this.cdhit_browse);
            this.Controls.Add(this.cdhit_label);
            this.Controls.Add(this.cdhit_textbox);
            this.Controls.Add(this.optional_input_button);
            this.Controls.Add(this.start_button);
            this.Controls.Add(this.blast_browse);
            this.Controls.Add(this.blast_label);
            this.Controls.Add(this.blast_textbox);
            this.Controls.Add(this.storage_browse);
            this.Controls.Add(this.storage_label);
            this.Controls.Add(this.storage_textbox);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "NOmESS";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox storage_textbox;
        private System.Windows.Forms.Label storage_label;
        private System.Windows.Forms.Button storage_browse;
        private System.Windows.Forms.Button blast_browse;
        private System.Windows.Forms.Label blast_label;
        private System.Windows.Forms.TextBox blast_textbox;
        private System.Windows.Forms.Button start_button;
        private System.Windows.Forms.Button optional_input_button;
        private System.Windows.Forms.Button cdhit_browse;
        private System.Windows.Forms.Label cdhit_label;
        private System.Windows.Forms.TextBox cdhit_textbox;
        private System.Windows.Forms.Button scaffold_browse;
        private System.Windows.Forms.Label homolog_label;
        private System.Windows.Forms.TextBox scaffold_textbox;
        private System.Windows.Forms.Button input_browse;
        private System.Windows.Forms.Label original_label;
        private System.Windows.Forms.TextBox input_textbox;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker backgroundWorkerProgressBar;
        private BackgroundWorker backgroundWorkerProcess;
        internal System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label3;
    }
}


namespace Havecenter_Adressebog
{
    partial class Loading
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
            this.xml_progressBar = new System.Windows.Forms.ProgressBar();
            this.cancel_xml_load = new System.Windows.Forms.Button();
            this.backgroundWorker_xml = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // xml_progressBar
            // 
            this.xml_progressBar.Location = new System.Drawing.Point(78, 161);
            this.xml_progressBar.Name = "xml_progressBar";
            this.xml_progressBar.Size = new System.Drawing.Size(100, 23);
            this.xml_progressBar.TabIndex = 0;
            // 
            // cancel_xml_load
            // 
            this.cancel_xml_load.Location = new System.Drawing.Point(93, 93);
            this.cancel_xml_load.Name = "cancel_xml_load";
            this.cancel_xml_load.Size = new System.Drawing.Size(75, 23);
            this.cancel_xml_load.TabIndex = 1;
            this.cancel_xml_load.Text = "Afbryd";
            this.cancel_xml_load.UseVisualStyleBackColor = true;
            this.cancel_xml_load.Click += new System.EventHandler(this.cancel_xml_load_Click);
            // 
            // backgroundWorker_xml
            // 
            this.backgroundWorker_xml.WorkerReportsProgress = true;
            this.backgroundWorker_xml.WorkerSupportsCancellation = true;
            this.backgroundWorker_xml.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_xml_DoWork);
            this.backgroundWorker_xml.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_xml_ProgressChanged);
            this.backgroundWorker_xml.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_xml_RunWorkerCompleted);
            // 
            // Loading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.cancel_xml_load);
            this.Controls.Add(this.xml_progressBar);
            this.Name = "Loading";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "loading";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar xml_progressBar;
        private System.Windows.Forms.Button cancel_xml_load;
        private System.ComponentModel.BackgroundWorker backgroundWorker_xml;
    }
}
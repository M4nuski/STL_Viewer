namespace STLViewer
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.renderPanel = new System.Windows.Forms.Panel();
            this.compCtrlPanel = new System.Windows.Forms.Panel();
            this.trackBarX = new System.Windows.Forms.TrackBar();
            this.trackBarZ = new System.Windows.Forms.TrackBar();
            this.trackBarY = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker_UniqueVertex = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker_Outline = new System.ComponentModel.BackgroundWorker();
            this.renderPanel.SuspendLayout();
            this.compCtrlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarY)).BeginInit();
            this.SuspendLayout();
            // 
            // renderPanel
            // 
            this.renderPanel.BackColor = System.Drawing.Color.White;
            this.renderPanel.Controls.Add(this.compCtrlPanel);
            this.renderPanel.Controls.Add(this.label2);
            this.renderPanel.Controls.Add(this.label1);
            this.renderPanel.Location = new System.Drawing.Point(0, 0);
            this.renderPanel.Name = "renderPanel";
            this.renderPanel.Size = new System.Drawing.Size(1024, 720);
            this.renderPanel.TabIndex = 0;
            this.renderPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.renderPanel_Paint);
            this.renderPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.renderPanel.MouseLeave += new System.EventHandler(this.panel1_MouseLeave);
            this.renderPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.renderPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // compCtrlPanel
            // 
            this.compCtrlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.compCtrlPanel.BackColor = System.Drawing.Color.Transparent;
            this.compCtrlPanel.Controls.Add(this.trackBarX);
            this.compCtrlPanel.Controls.Add(this.trackBarZ);
            this.compCtrlPanel.Controls.Add(this.trackBarY);
            this.compCtrlPanel.Controls.Add(this.label5);
            this.compCtrlPanel.Controls.Add(this.label4);
            this.compCtrlPanel.Controls.Add(this.label3);
            this.compCtrlPanel.Location = new System.Drawing.Point(3, 562);
            this.compCtrlPanel.Name = "compCtrlPanel";
            this.compCtrlPanel.Size = new System.Drawing.Size(510, 122);
            this.compCtrlPanel.TabIndex = 5;
            this.compCtrlPanel.Visible = false;
            // 
            // trackBarX
            // 
            this.trackBarX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackBarX.AutoSize = false;
            this.trackBarX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(105)))), ((int)(((byte)(219)))));
            this.trackBarX.Location = new System.Drawing.Point(26, 3);
            this.trackBarX.Maximum = 40;
            this.trackBarX.Minimum = -40;
            this.trackBarX.Name = "trackBarX";
            this.trackBarX.Size = new System.Drawing.Size(480, 35);
            this.trackBarX.TabIndex = 2;
            this.trackBarX.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // trackBarZ
            // 
            this.trackBarZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackBarZ.AutoSize = false;
            this.trackBarZ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(105)))), ((int)(((byte)(219)))));
            this.trackBarZ.Location = new System.Drawing.Point(26, 84);
            this.trackBarZ.Maximum = 40;
            this.trackBarZ.Minimum = -40;
            this.trackBarZ.Name = "trackBarZ";
            this.trackBarZ.Size = new System.Drawing.Size(480, 35);
            this.trackBarZ.TabIndex = 4;
            this.trackBarZ.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // trackBarY
            // 
            this.trackBarY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackBarY.AutoSize = false;
            this.trackBarY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(105)))), ((int)(((byte)(219)))));
            this.trackBarY.Location = new System.Drawing.Point(26, 44);
            this.trackBarY.Maximum = 40;
            this.trackBarY.Minimum = -40;
            this.trackBarY.Name = "trackBarY";
            this.trackBarY.Size = new System.Drawing.Size(480, 35);
            this.trackBarY.TabIndex = 3;
            this.trackBarY.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 24);
            this.label5.TabIndex = 7;
            this.label5.Text = "Z";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 24);
            this.label4.TabIndex = 6;
            this.label4.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "X";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(295, 294);
            this.label2.TabIndex = 1;
            this.label2.Text = resources.GetString("label2.Text");
            this.label2.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(105)))), ((int)(((byte)(219)))));
            this.label1.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 687);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(403, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Drag and drop file to load, F1 for help";
            // 
            // backgroundWorker_UniqueVertex
            // 
            this.backgroundWorker_UniqueVertex.WorkerReportsProgress = true;
            this.backgroundWorker_UniqueVertex.WorkerSupportsCancellation = true;
            this.backgroundWorker_UniqueVertex.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_Uniques_DoWork);
            this.backgroundWorker_UniqueVertex.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_Uniques_ProgressChanged);
            this.backgroundWorker_UniqueVertex.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_Uniques_RunWorkerCompleted);
            // 
            // backgroundWorker_Outline
            // 
            this.backgroundWorker_Outline.WorkerReportsProgress = true;
            this.backgroundWorker_Outline.WorkerSupportsCancellation = true;
            this.backgroundWorker_Outline.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_Outline_DoWork);
            this.backgroundWorker_Outline.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_Outline_ProgressChanged);
            this.backgroundWorker_Outline.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_Outline_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 721);
            this.Controls.Add(this.renderPanel);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "STL Viewer v1.1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ClientSizeChanged += new System.EventHandler(this.Form1_ClientSizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.renderPanel.ResumeLayout(false);
            this.renderPanel.PerformLayout();
            this.compCtrlPanel.ResumeLayout(false);
            this.compCtrlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarY)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel renderPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBarX;
        private System.Windows.Forms.Panel compCtrlPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBarZ;
        private System.Windows.Forms.TrackBar trackBarY;
        private System.ComponentModel.BackgroundWorker backgroundWorker_UniqueVertex;
        private System.ComponentModel.BackgroundWorker backgroundWorker_Outline;
    }
}


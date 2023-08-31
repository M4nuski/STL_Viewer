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
            this.holeCompPanel = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.holeCompModeComboBox = new System.Windows.Forms.ComboBox();
            this.holeAxisRadioButtonY = new System.Windows.Forms.RadioButton();
            this.holeAxisRadioButtonZ = new System.Windows.Forms.RadioButton();
            this.holeAxisRadioButtonX = new System.Windows.Forms.RadioButton();
            this.holeLimitModeComboBox = new System.Windows.Forms.ComboBox();
            this.centerCompTrackBar = new System.Windows.Forms.TrackBar();
            this.centerLimitTrackBar = new System.Windows.Forms.TrackBar();
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
            this.MesurementsPanel = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelP1X = new System.Windows.Forms.Label();
            this.labelP1Y = new System.Windows.Forms.Label();
            this.labelP1Z = new System.Windows.Forms.Label();
            this.labelP2Z = new System.Windows.Forms.Label();
            this.labelP2Y = new System.Windows.Forms.Label();
            this.labelP2X = new System.Windows.Forms.Label();
            this.labelP3Z = new System.Windows.Forms.Label();
            this.labelP3Y = new System.Windows.Forms.Label();
            this.labelP3X = new System.Windows.Forms.Label();
            this.labelLEN31 = new System.Windows.Forms.Label();
            this.labelLEN23 = new System.Windows.Forms.Label();
            this.labelLEN12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.labelMID31Z = new System.Windows.Forms.Label();
            this.labelMID31Y = new System.Windows.Forms.Label();
            this.labelMID31X = new System.Windows.Forms.Label();
            this.labelMID23Z = new System.Windows.Forms.Label();
            this.labelMID23Y = new System.Windows.Forms.Label();
            this.labelMID23X = new System.Windows.Forms.Label();
            this.labelMID12Z = new System.Windows.Forms.Label();
            this.labelMID12Y = new System.Windows.Forms.Label();
            this.labelMID12X = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.labelLEN3112 = new System.Windows.Forms.Label();
            this.labelLEN2331 = new System.Windows.Forms.Label();
            this.labelLEN1223 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.labelDIA123 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelCEN123Z = new System.Windows.Forms.Label();
            this.labelCEN123Y = new System.Windows.Forms.Label();
            this.labelCEN123X = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.labelNOR123Z = new System.Windows.Forms.Label();
            this.labelNOR123Y = new System.Windows.Forms.Label();
            this.labelNOR123X = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.renderPanel.SuspendLayout();
            this.holeCompPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.centerCompTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerLimitTrackBar)).BeginInit();
            this.compCtrlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarY)).BeginInit();
            this.MesurementsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // renderPanel
            // 
            this.renderPanel.BackColor = System.Drawing.Color.White;
            this.renderPanel.Controls.Add(this.MesurementsPanel);
            this.renderPanel.Controls.Add(this.holeCompPanel);
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
            // holeCompPanel
            // 
            this.holeCompPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.holeCompPanel.BackColor = System.Drawing.Color.Transparent;
            this.holeCompPanel.Controls.Add(this.comboBox1);
            this.holeCompPanel.Controls.Add(this.radioButton1);
            this.holeCompPanel.Controls.Add(this.label7);
            this.holeCompPanel.Controls.Add(this.label6);
            this.holeCompPanel.Controls.Add(this.holeCompModeComboBox);
            this.holeCompPanel.Controls.Add(this.holeAxisRadioButtonY);
            this.holeCompPanel.Controls.Add(this.holeAxisRadioButtonZ);
            this.holeCompPanel.Controls.Add(this.holeAxisRadioButtonX);
            this.holeCompPanel.Controls.Add(this.holeLimitModeComboBox);
            this.holeCompPanel.Controls.Add(this.centerCompTrackBar);
            this.holeCompPanel.Controls.Add(this.centerLimitTrackBar);
            this.holeCompPanel.Location = new System.Drawing.Point(4, 579);
            this.holeCompPanel.Name = "holeCompPanel";
            this.holeCompPanel.Size = new System.Drawing.Size(535, 102);
            this.holeCompPanel.TabIndex = 6;
            this.holeCompPanel.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Comp";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Limit";
            // 
            // holeCompModeComboBox
            // 
            this.holeCompModeComboBox.FormattingEnabled = true;
            this.holeCompModeComboBox.Items.AddRange(new object[] {
            "mm",
            "PCT"});
            this.holeCompModeComboBox.Location = new System.Drawing.Point(3, 80);
            this.holeCompModeComboBox.Name = "holeCompModeComboBox";
            this.holeCompModeComboBox.Size = new System.Drawing.Size(45, 21);
            this.holeCompModeComboBox.TabIndex = 12;
            this.holeCompModeComboBox.Text = "mm";
            this.holeCompModeComboBox.SelectedValueChanged += new System.EventHandler(this.centerTrackBar_ValueChanged);
            // 
            // holeAxisRadioButtonY
            // 
            this.holeAxisRadioButtonY.AutoSize = true;
            this.holeAxisRadioButtonY.Checked = true;
            this.holeAxisRadioButtonY.Location = new System.Drawing.Point(127, 4);
            this.holeAxisRadioButtonY.Name = "holeAxisRadioButtonY";
            this.holeAxisRadioButtonY.Size = new System.Drawing.Size(32, 17);
            this.holeAxisRadioButtonY.TabIndex = 11;
            this.holeAxisRadioButtonY.TabStop = true;
            this.holeAxisRadioButtonY.Tag = "Y";
            this.holeAxisRadioButtonY.Text = "Y";
            this.holeAxisRadioButtonY.UseVisualStyleBackColor = true;
            this.holeAxisRadioButtonY.CheckedChanged += new System.EventHandler(this.centerTrackBar_ValueChanged);
            // 
            // holeAxisRadioButtonZ
            // 
            this.holeAxisRadioButtonZ.AutoSize = true;
            this.holeAxisRadioButtonZ.Location = new System.Drawing.Point(165, 4);
            this.holeAxisRadioButtonZ.Name = "holeAxisRadioButtonZ";
            this.holeAxisRadioButtonZ.Size = new System.Drawing.Size(32, 17);
            this.holeAxisRadioButtonZ.TabIndex = 10;
            this.holeAxisRadioButtonZ.Tag = "Z";
            this.holeAxisRadioButtonZ.Text = "Z";
            this.holeAxisRadioButtonZ.UseVisualStyleBackColor = true;
            this.holeAxisRadioButtonZ.CheckedChanged += new System.EventHandler(this.centerTrackBar_ValueChanged);
            // 
            // holeAxisRadioButtonX
            // 
            this.holeAxisRadioButtonX.AutoSize = true;
            this.holeAxisRadioButtonX.Location = new System.Drawing.Point(89, 4);
            this.holeAxisRadioButtonX.Name = "holeAxisRadioButtonX";
            this.holeAxisRadioButtonX.Size = new System.Drawing.Size(32, 17);
            this.holeAxisRadioButtonX.TabIndex = 9;
            this.holeAxisRadioButtonX.Tag = "X";
            this.holeAxisRadioButtonX.Text = "X";
            this.holeAxisRadioButtonX.UseVisualStyleBackColor = true;
            this.holeAxisRadioButtonX.CheckedChanged += new System.EventHandler(this.centerTrackBar_ValueChanged);
            // 
            // holeLimitModeComboBox
            // 
            this.holeLimitModeComboBox.FormattingEnabled = true;
            this.holeLimitModeComboBox.Items.AddRange(new object[] {
            "Max",
            "Min"});
            this.holeLimitModeComboBox.Location = new System.Drawing.Point(3, 39);
            this.holeLimitModeComboBox.Name = "holeLimitModeComboBox";
            this.holeLimitModeComboBox.Size = new System.Drawing.Size(45, 21);
            this.holeLimitModeComboBox.TabIndex = 8;
            this.holeLimitModeComboBox.Text = "Max";
            this.holeLimitModeComboBox.SelectedValueChanged += new System.EventHandler(this.centerTrackBar_ValueChanged);
            // 
            // centerCompTrackBar
            // 
            this.centerCompTrackBar.AutoSize = false;
            this.centerCompTrackBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(105)))), ((int)(((byte)(219)))));
            this.centerCompTrackBar.Location = new System.Drawing.Point(49, 66);
            this.centerCompTrackBar.Maximum = 40;
            this.centerCompTrackBar.Minimum = -40;
            this.centerCompTrackBar.Name = "centerCompTrackBar";
            this.centerCompTrackBar.Size = new System.Drawing.Size(480, 35);
            this.centerCompTrackBar.TabIndex = 4;
            this.centerCompTrackBar.Tag = "NoLR";
            this.centerCompTrackBar.ValueChanged += new System.EventHandler(this.centerTrackBar_ValueChanged);
            // 
            // centerLimitTrackBar
            // 
            this.centerLimitTrackBar.AutoSize = false;
            this.centerLimitTrackBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(105)))), ((int)(((byte)(219)))));
            this.centerLimitTrackBar.Location = new System.Drawing.Point(49, 25);
            this.centerLimitTrackBar.Maximum = 400;
            this.centerLimitTrackBar.Minimum = 1;
            this.centerLimitTrackBar.Name = "centerLimitTrackBar";
            this.centerLimitTrackBar.Size = new System.Drawing.Size(480, 35);
            this.centerLimitTrackBar.TabIndex = 3;
            this.centerLimitTrackBar.Tag = "NoLR";
            this.centerLimitTrackBar.Value = 15;
            this.centerLimitTrackBar.ValueChanged += new System.EventHandler(this.centerTrackBar_ValueChanged);
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
            this.compCtrlPanel.Location = new System.Drawing.Point(4, 562);
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
            this.trackBarX.Tag = "NoLR";
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
            this.trackBarZ.Tag = "NoLR";
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
            this.trackBarY.Tag = "NoLR";
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
            this.label2.Size = new System.Drawing.Size(314, 329);
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
            // MesurementsPanel
            // 
            this.MesurementsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MesurementsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MesurementsPanel.Controls.Add(this.labelNOR123Z);
            this.MesurementsPanel.Controls.Add(this.labelNOR123Y);
            this.MesurementsPanel.Controls.Add(this.labelNOR123X);
            this.MesurementsPanel.Controls.Add(this.label21);
            this.MesurementsPanel.Controls.Add(this.labelCEN123Z);
            this.MesurementsPanel.Controls.Add(this.labelCEN123Y);
            this.MesurementsPanel.Controls.Add(this.labelCEN123X);
            this.MesurementsPanel.Controls.Add(this.label22);
            this.MesurementsPanel.Controls.Add(this.labelDIA123);
            this.MesurementsPanel.Controls.Add(this.label12);
            this.MesurementsPanel.Controls.Add(this.labelLEN3112);
            this.MesurementsPanel.Controls.Add(this.labelLEN2331);
            this.MesurementsPanel.Controls.Add(this.labelLEN1223);
            this.MesurementsPanel.Controls.Add(this.label17);
            this.MesurementsPanel.Controls.Add(this.label18);
            this.MesurementsPanel.Controls.Add(this.label19);
            this.MesurementsPanel.Controls.Add(this.labelMID31Z);
            this.MesurementsPanel.Controls.Add(this.labelMID31Y);
            this.MesurementsPanel.Controls.Add(this.labelMID31X);
            this.MesurementsPanel.Controls.Add(this.labelMID23Z);
            this.MesurementsPanel.Controls.Add(this.labelMID23Y);
            this.MesurementsPanel.Controls.Add(this.labelMID23X);
            this.MesurementsPanel.Controls.Add(this.labelMID12Z);
            this.MesurementsPanel.Controls.Add(this.labelMID12Y);
            this.MesurementsPanel.Controls.Add(this.labelMID12X);
            this.MesurementsPanel.Controls.Add(this.label23);
            this.MesurementsPanel.Controls.Add(this.label24);
            this.MesurementsPanel.Controls.Add(this.label25);
            this.MesurementsPanel.Controls.Add(this.labelLEN31);
            this.MesurementsPanel.Controls.Add(this.labelLEN23);
            this.MesurementsPanel.Controls.Add(this.labelLEN12);
            this.MesurementsPanel.Controls.Add(this.label14);
            this.MesurementsPanel.Controls.Add(this.label15);
            this.MesurementsPanel.Controls.Add(this.label16);
            this.MesurementsPanel.Controls.Add(this.labelP3Z);
            this.MesurementsPanel.Controls.Add(this.labelP3Y);
            this.MesurementsPanel.Controls.Add(this.labelP3X);
            this.MesurementsPanel.Controls.Add(this.labelP2Z);
            this.MesurementsPanel.Controls.Add(this.labelP2Y);
            this.MesurementsPanel.Controls.Add(this.labelP2X);
            this.MesurementsPanel.Controls.Add(this.labelP1Z);
            this.MesurementsPanel.Controls.Add(this.labelP1Y);
            this.MesurementsPanel.Controls.Add(this.labelP1X);
            this.MesurementsPanel.Controls.Add(this.label10);
            this.MesurementsPanel.Controls.Add(this.label9);
            this.MesurementsPanel.Controls.Add(this.label8);
            this.MesurementsPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MesurementsPanel.Location = new System.Drawing.Point(713, 12);
            this.MesurementsPanel.Name = "MesurementsPanel";
            this.MesurementsPanel.Size = new System.Drawing.Size(301, 312);
            this.MesurementsPanel.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 18);
            this.label8.TabIndex = 0;
            this.label8.Text = "Point 1";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(103, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 18);
            this.label9.TabIndex = 1;
            this.label9.Text = "Point 2";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(203, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 18);
            this.label10.TabIndex = 2;
            this.label10.Text = "Point 3";
            // 
            // labelP1X
            // 
            this.labelP1X.AutoSize = true;
            this.labelP1X.Location = new System.Drawing.Point(3, 18);
            this.labelP1X.Name = "labelP1X";
            this.labelP1X.Size = new System.Drawing.Size(70, 18);
            this.labelP1X.TabIndex = 3;
            this.labelP1X.Text = "X: 225.00";
            // 
            // labelP1Y
            // 
            this.labelP1Y.AutoSize = true;
            this.labelP1Y.Location = new System.Drawing.Point(3, 36);
            this.labelP1Y.Name = "labelP1Y";
            this.labelP1Y.Size = new System.Drawing.Size(69, 18);
            this.labelP1Y.TabIndex = 4;
            this.labelP1Y.Text = "Y: 225.00";
            // 
            // labelP1Z
            // 
            this.labelP1Z.AutoSize = true;
            this.labelP1Z.Location = new System.Drawing.Point(3, 54);
            this.labelP1Z.Name = "labelP1Z";
            this.labelP1Z.Size = new System.Drawing.Size(69, 18);
            this.labelP1Z.TabIndex = 5;
            this.labelP1Z.Text = "Z: 225.00";
            // 
            // labelP2Z
            // 
            this.labelP2Z.AutoSize = true;
            this.labelP2Z.Location = new System.Drawing.Point(103, 54);
            this.labelP2Z.Name = "labelP2Z";
            this.labelP2Z.Size = new System.Drawing.Size(69, 18);
            this.labelP2Z.TabIndex = 8;
            this.labelP2Z.Text = "Z: 225.00";
            // 
            // labelP2Y
            // 
            this.labelP2Y.AutoSize = true;
            this.labelP2Y.Location = new System.Drawing.Point(103, 36);
            this.labelP2Y.Name = "labelP2Y";
            this.labelP2Y.Size = new System.Drawing.Size(69, 18);
            this.labelP2Y.TabIndex = 7;
            this.labelP2Y.Text = "Y: 225.00";
            // 
            // labelP2X
            // 
            this.labelP2X.AutoSize = true;
            this.labelP2X.Location = new System.Drawing.Point(103, 18);
            this.labelP2X.Name = "labelP2X";
            this.labelP2X.Size = new System.Drawing.Size(70, 18);
            this.labelP2X.TabIndex = 6;
            this.labelP2X.Text = "X: 225.00";
            // 
            // labelP3Z
            // 
            this.labelP3Z.AutoSize = true;
            this.labelP3Z.Location = new System.Drawing.Point(203, 54);
            this.labelP3Z.Name = "labelP3Z";
            this.labelP3Z.Size = new System.Drawing.Size(69, 18);
            this.labelP3Z.TabIndex = 11;
            this.labelP3Z.Text = "Z: 225.00";
            // 
            // labelP3Y
            // 
            this.labelP3Y.AutoSize = true;
            this.labelP3Y.Location = new System.Drawing.Point(203, 36);
            this.labelP3Y.Name = "labelP3Y";
            this.labelP3Y.Size = new System.Drawing.Size(69, 18);
            this.labelP3Y.TabIndex = 10;
            this.labelP3Y.Text = "Y: 225.00";
            // 
            // labelP3X
            // 
            this.labelP3X.AutoSize = true;
            this.labelP3X.Location = new System.Drawing.Point(203, 18);
            this.labelP3X.Name = "labelP3X";
            this.labelP3X.Size = new System.Drawing.Size(70, 18);
            this.labelP3X.TabIndex = 9;
            this.labelP3X.Text = "X: 225.00";
            // 
            // labelLEN31
            // 
            this.labelLEN31.AutoSize = true;
            this.labelLEN31.Location = new System.Drawing.Point(202, 90);
            this.labelLEN31.Name = "labelLEN31";
            this.labelLEN31.Size = new System.Drawing.Size(52, 18);
            this.labelLEN31.TabIndex = 17;
            this.labelLEN31.Text = "225.00";
            // 
            // labelLEN23
            // 
            this.labelLEN23.AutoSize = true;
            this.labelLEN23.Location = new System.Drawing.Point(102, 90);
            this.labelLEN23.Name = "labelLEN23";
            this.labelLEN23.Size = new System.Drawing.Size(52, 18);
            this.labelLEN23.TabIndex = 16;
            this.labelLEN23.Text = "225.00";
            // 
            // labelLEN12
            // 
            this.labelLEN12.AutoSize = true;
            this.labelLEN12.Location = new System.Drawing.Point(2, 90);
            this.labelLEN12.Name = "labelLEN12";
            this.labelLEN12.Size = new System.Drawing.Size(52, 18);
            this.labelLEN12.TabIndex = 15;
            this.labelLEN12.Text = "225.00";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(202, 72);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 18);
            this.label14.TabIndex = 14;
            this.label14.Text = "Length 3-1";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(102, 72);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 18);
            this.label15.TabIndex = 13;
            this.label15.Text = "Length 2-3";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(2, 72);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 18);
            this.label16.TabIndex = 12;
            this.label16.Text = "Length 1-2";
            // 
            // labelMID31Z
            // 
            this.labelMID31Z.AutoSize = true;
            this.labelMID31Z.Location = new System.Drawing.Point(202, 167);
            this.labelMID31Z.Name = "labelMID31Z";
            this.labelMID31Z.Size = new System.Drawing.Size(69, 18);
            this.labelMID31Z.TabIndex = 29;
            this.labelMID31Z.Text = "Z: 225.00";
            // 
            // labelMID31Y
            // 
            this.labelMID31Y.AutoSize = true;
            this.labelMID31Y.Location = new System.Drawing.Point(202, 149);
            this.labelMID31Y.Name = "labelMID31Y";
            this.labelMID31Y.Size = new System.Drawing.Size(69, 18);
            this.labelMID31Y.TabIndex = 28;
            this.labelMID31Y.Text = "Y: 225.00";
            // 
            // labelMID31X
            // 
            this.labelMID31X.AutoSize = true;
            this.labelMID31X.Location = new System.Drawing.Point(202, 131);
            this.labelMID31X.Name = "labelMID31X";
            this.labelMID31X.Size = new System.Drawing.Size(70, 18);
            this.labelMID31X.TabIndex = 27;
            this.labelMID31X.Text = "X: 225.00";
            // 
            // labelMID23Z
            // 
            this.labelMID23Z.AutoSize = true;
            this.labelMID23Z.Location = new System.Drawing.Point(102, 167);
            this.labelMID23Z.Name = "labelMID23Z";
            this.labelMID23Z.Size = new System.Drawing.Size(69, 18);
            this.labelMID23Z.TabIndex = 26;
            this.labelMID23Z.Text = "Z: 225.00";
            // 
            // labelMID23Y
            // 
            this.labelMID23Y.AutoSize = true;
            this.labelMID23Y.Location = new System.Drawing.Point(102, 149);
            this.labelMID23Y.Name = "labelMID23Y";
            this.labelMID23Y.Size = new System.Drawing.Size(69, 18);
            this.labelMID23Y.TabIndex = 25;
            this.labelMID23Y.Text = "Y: 225.00";
            // 
            // labelMID23X
            // 
            this.labelMID23X.AutoSize = true;
            this.labelMID23X.Location = new System.Drawing.Point(102, 131);
            this.labelMID23X.Name = "labelMID23X";
            this.labelMID23X.Size = new System.Drawing.Size(70, 18);
            this.labelMID23X.TabIndex = 24;
            this.labelMID23X.Text = "X: 225.00";
            // 
            // labelMID12Z
            // 
            this.labelMID12Z.AutoSize = true;
            this.labelMID12Z.Location = new System.Drawing.Point(2, 167);
            this.labelMID12Z.Name = "labelMID12Z";
            this.labelMID12Z.Size = new System.Drawing.Size(69, 18);
            this.labelMID12Z.TabIndex = 23;
            this.labelMID12Z.Text = "Z: 225.00";
            // 
            // labelMID12Y
            // 
            this.labelMID12Y.AutoSize = true;
            this.labelMID12Y.Location = new System.Drawing.Point(2, 149);
            this.labelMID12Y.Name = "labelMID12Y";
            this.labelMID12Y.Size = new System.Drawing.Size(69, 18);
            this.labelMID12Y.TabIndex = 22;
            this.labelMID12Y.Text = "Y: 225.00";
            // 
            // labelMID12X
            // 
            this.labelMID12X.AutoSize = true;
            this.labelMID12X.Location = new System.Drawing.Point(2, 131);
            this.labelMID12X.Name = "labelMID12X";
            this.labelMID12X.Size = new System.Drawing.Size(70, 18);
            this.labelMID12X.TabIndex = 21;
            this.labelMID12X.Text = "X: 225.00";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(202, 113);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(86, 18);
            this.label23.TabIndex = 20;
            this.label23.Text = "MidPoint 31";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(102, 113);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(86, 18);
            this.label24.TabIndex = 19;
            this.label24.Text = "MidPoint 23";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(2, 113);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(86, 18);
            this.label25.TabIndex = 18;
            this.label25.Text = "MidPoint 12";
            // 
            // labelLEN3112
            // 
            this.labelLEN3112.AutoSize = true;
            this.labelLEN3112.Location = new System.Drawing.Point(202, 203);
            this.labelLEN3112.Name = "labelLEN3112";
            this.labelLEN3112.Size = new System.Drawing.Size(52, 18);
            this.labelLEN3112.TabIndex = 35;
            this.labelLEN3112.Text = "225.00";
            // 
            // labelLEN2331
            // 
            this.labelLEN2331.AutoSize = true;
            this.labelLEN2331.Location = new System.Drawing.Point(102, 203);
            this.labelLEN2331.Name = "labelLEN2331";
            this.labelLEN2331.Size = new System.Drawing.Size(52, 18);
            this.labelLEN2331.TabIndex = 34;
            this.labelLEN2331.Text = "225.00";
            // 
            // labelLEN1223
            // 
            this.labelLEN1223.AutoSize = true;
            this.labelLEN1223.Location = new System.Drawing.Point(2, 203);
            this.labelLEN1223.Name = "labelLEN1223";
            this.labelLEN1223.Size = new System.Drawing.Size(52, 18);
            this.labelLEN1223.TabIndex = 33;
            this.labelLEN1223.Text = "225.00";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(202, 185);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(93, 18);
            this.label17.TabIndex = 32;
            this.label17.Text = "Length 31-12";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(102, 185);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(93, 18);
            this.label18.TabIndex = 31;
            this.label18.Text = "Length 23-31";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(2, 185);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(93, 18);
            this.label19.TabIndex = 30;
            this.label19.Text = "Length 12-23";
            // 
            // labelDIA123
            // 
            this.labelDIA123.AutoSize = true;
            this.labelDIA123.Location = new System.Drawing.Point(3, 244);
            this.labelDIA123.Name = "labelDIA123";
            this.labelDIA123.Size = new System.Drawing.Size(52, 18);
            this.labelDIA123.TabIndex = 37;
            this.labelDIA123.Text = "225.00";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 226);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(96, 18);
            this.label12.TabIndex = 36;
            this.label12.Text = "Diameter 123";
            // 
            // labelCEN123Z
            // 
            this.labelCEN123Z.AutoSize = true;
            this.labelCEN123Z.Location = new System.Drawing.Point(102, 280);
            this.labelCEN123Z.Name = "labelCEN123Z";
            this.labelCEN123Z.Size = new System.Drawing.Size(69, 18);
            this.labelCEN123Z.TabIndex = 41;
            this.labelCEN123Z.Text = "Z: 225.00";
            // 
            // labelCEN123Y
            // 
            this.labelCEN123Y.AutoSize = true;
            this.labelCEN123Y.Location = new System.Drawing.Point(102, 262);
            this.labelCEN123Y.Name = "labelCEN123Y";
            this.labelCEN123Y.Size = new System.Drawing.Size(69, 18);
            this.labelCEN123Y.TabIndex = 40;
            this.labelCEN123Y.Text = "Y: 225.00";
            // 
            // labelCEN123X
            // 
            this.labelCEN123X.AutoSize = true;
            this.labelCEN123X.Location = new System.Drawing.Point(102, 244);
            this.labelCEN123X.Name = "labelCEN123X";
            this.labelCEN123X.Size = new System.Drawing.Size(70, 18);
            this.labelCEN123X.TabIndex = 39;
            this.labelCEN123X.Text = "X: 225.00";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(102, 226);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(80, 18);
            this.label22.TabIndex = 38;
            this.label22.Text = "Center 123";
            // 
            // labelNOR123Z
            // 
            this.labelNOR123Z.AutoSize = true;
            this.labelNOR123Z.Location = new System.Drawing.Point(203, 280);
            this.labelNOR123Z.Name = "labelNOR123Z";
            this.labelNOR123Z.Size = new System.Drawing.Size(69, 18);
            this.labelNOR123Z.TabIndex = 45;
            this.labelNOR123Z.Text = "Z: 225.00";
            // 
            // labelNOR123Y
            // 
            this.labelNOR123Y.AutoSize = true;
            this.labelNOR123Y.Location = new System.Drawing.Point(203, 262);
            this.labelNOR123Y.Name = "labelNOR123Y";
            this.labelNOR123Y.Size = new System.Drawing.Size(69, 18);
            this.labelNOR123Y.TabIndex = 44;
            this.labelNOR123Y.Text = "Y: 225.00";
            // 
            // labelNOR123X
            // 
            this.labelNOR123X.AutoSize = true;
            this.labelNOR123X.Location = new System.Drawing.Point(203, 244);
            this.labelNOR123X.Name = "labelNOR123X";
            this.labelNOR123X.Size = new System.Drawing.Size(70, 18);
            this.labelNOR123X.TabIndex = 43;
            this.labelNOR123X.Text = "X: 225.00";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(203, 226);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(85, 18);
            this.label21.TabIndex = 42;
            this.label21.Text = "Normal 123";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(359, 4);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(170, 17);
            this.radioButton1.TabIndex = 15;
            this.radioButton1.Tag = "M";
            this.radioButton1.Text = "Mesured P1-P2-P3 Center Axis";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Model",
            "P1",
            "P2",
            "P3",
            "Mid12",
            "Mid23",
            "Mid31",
            "Center123"});
            this.comboBox1.Location = new System.Drawing.Point(3, 2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(80, 21);
            this.comboBox1.TabIndex = 16;
            this.comboBox1.Text = "Model";
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
            this.Text = "STL Viewer v1.2";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ClientSizeChanged += new System.EventHandler(this.Form1_ClientSizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.renderPanel.ResumeLayout(false);
            this.renderPanel.PerformLayout();
            this.holeCompPanel.ResumeLayout(false);
            this.holeCompPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.centerCompTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.centerLimitTrackBar)).EndInit();
            this.compCtrlPanel.ResumeLayout(false);
            this.compCtrlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarY)).EndInit();
            this.MesurementsPanel.ResumeLayout(false);
            this.MesurementsPanel.PerformLayout();
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
        private System.Windows.Forms.Panel holeCompPanel;
        private System.Windows.Forms.TrackBar centerCompTrackBar;
        private System.Windows.Forms.TrackBar centerLimitTrackBar;
        private System.Windows.Forms.RadioButton holeAxisRadioButtonY;
        private System.Windows.Forms.RadioButton holeAxisRadioButtonZ;
        private System.Windows.Forms.RadioButton holeAxisRadioButtonX;
        private System.Windows.Forms.ComboBox holeLimitModeComboBox;
        private System.Windows.Forms.ComboBox holeCompModeComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel MesurementsPanel;
        private System.Windows.Forms.Label labelP1X;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelP1Z;
        private System.Windows.Forms.Label labelP1Y;
        private System.Windows.Forms.Label labelMID31Z;
        private System.Windows.Forms.Label labelMID31Y;
        private System.Windows.Forms.Label labelMID31X;
        private System.Windows.Forms.Label labelMID23Z;
        private System.Windows.Forms.Label labelMID23Y;
        private System.Windows.Forms.Label labelMID23X;
        private System.Windows.Forms.Label labelMID12Z;
        private System.Windows.Forms.Label labelMID12Y;
        private System.Windows.Forms.Label labelMID12X;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label labelLEN31;
        private System.Windows.Forms.Label labelLEN23;
        private System.Windows.Forms.Label labelLEN12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label labelP3Z;
        private System.Windows.Forms.Label labelP3Y;
        private System.Windows.Forms.Label labelP3X;
        private System.Windows.Forms.Label labelP2Z;
        private System.Windows.Forms.Label labelP2Y;
        private System.Windows.Forms.Label labelP2X;
        private System.Windows.Forms.Label labelNOR123Z;
        private System.Windows.Forms.Label labelNOR123Y;
        private System.Windows.Forms.Label labelNOR123X;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label labelCEN123Z;
        private System.Windows.Forms.Label labelCEN123Y;
        private System.Windows.Forms.Label labelCEN123X;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label labelDIA123;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelLEN3112;
        private System.Windows.Forms.Label labelLEN2331;
        private System.Windows.Forms.Label labelLEN1223;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}


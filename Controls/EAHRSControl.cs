using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MissionPlanner.Controls
{
    public partial class EAHRSControl : UserControl
    {
        private AidingData aidingDataDialog = new AidingData();

        public EAHRSControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.EAHRSButtonsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.BUT_externalAHRS_gnss_enable = new MissionPlanner.Controls.MyButton();
            this.BUT_externalAHRS_gnss_disable = new MissionPlanner.Controls.MyButton();
            this.BUT_externalAHRS_vg3dclb_flight_start = new MissionPlanner.Controls.MyButton();
            this.BUT_externalAHRS_vg3dclb_flight_stop = new MissionPlanner.Controls.MyButton();
            this.BUT_externalAHRS_start = new MissionPlanner.Controls.MyButton();
            this.BUT_externalAHRS_stop = new MissionPlanner.Controls.MyButton();
            this.BUT_externalAHRS_aiding_data = new MissionPlanner.Controls.MyButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.CB_gnss_position = new System.Windows.Forms.CheckBox();
            this.CB_ins_pos_estimation = new System.Windows.Forms.CheckBox();
            this.CB_gcs_distance = new System.Windows.Forms.CheckBox();
            this.NUD_gcs_distance_around = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.quickView1 = new MissionPlanner.Controls.QuickView();
            this.quickView2 = new MissionPlanner.Controls.QuickView();
            this.quickView3 = new MissionPlanner.Controls.QuickView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.quickView4 = new MissionPlanner.Controls.QuickView();
            this.quickView5 = new MissionPlanner.Controls.QuickView();
            this.quickView6 = new MissionPlanner.Controls.QuickView();
            this.tableLayoutPanel1.SuspendLayout();
            this.EAHRSButtonsTableLayoutPanel.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_gcs_distance_around)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.04124F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.95876F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.tableLayoutPanel1.Controls.Add(this.EAHRSButtonsTableLayoutPanel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 77.72926F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.27074F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(562, 250);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // EAHRSButtonsTableLayoutPanel
            // 
            this.EAHRSButtonsTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EAHRSButtonsTableLayoutPanel.ColumnCount = 4;
            this.EAHRSButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.62385F));
            this.EAHRSButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.37615F));
            this.EAHRSButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.EAHRSButtonsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 77F));
            this.EAHRSButtonsTableLayoutPanel.Controls.Add(this.BUT_externalAHRS_gnss_enable, 0, 0);
            this.EAHRSButtonsTableLayoutPanel.Controls.Add(this.BUT_externalAHRS_gnss_disable, 1, 0);
            this.EAHRSButtonsTableLayoutPanel.Controls.Add(this.BUT_externalAHRS_vg3dclb_flight_start, 2, 0);
            this.EAHRSButtonsTableLayoutPanel.Controls.Add(this.BUT_externalAHRS_vg3dclb_flight_stop, 3, 0);
            this.EAHRSButtonsTableLayoutPanel.Controls.Add(this.BUT_externalAHRS_start, 2, 3);
            this.EAHRSButtonsTableLayoutPanel.Controls.Add(this.BUT_externalAHRS_stop, 3, 3);
            this.EAHRSButtonsTableLayoutPanel.Controls.Add(this.BUT_externalAHRS_aiding_data, 3, 1);
            this.EAHRSButtonsTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.EAHRSButtonsTableLayoutPanel.Name = "EAHRSButtonsTableLayoutPanel";
            this.EAHRSButtonsTableLayoutPanel.RowCount = 4;
            this.EAHRSButtonsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.EAHRSButtonsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.EAHRSButtonsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.EAHRSButtonsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.EAHRSButtonsTableLayoutPanel.Size = new System.Drawing.Size(286, 188);
            this.EAHRSButtonsTableLayoutPanel.TabIndex = 1;
            // 
            // BUT_externalAHRS_gnss_enable
            // 
            this.BUT_externalAHRS_gnss_enable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BUT_externalAHRS_gnss_enable.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BUT_externalAHRS_gnss_enable.Location = new System.Drawing.Point(3, 3);
            this.BUT_externalAHRS_gnss_enable.Name = "BUT_externalAHRS_gnss_enable";
            this.BUT_externalAHRS_gnss_enable.Size = new System.Drawing.Size(61, 58);
            this.BUT_externalAHRS_gnss_enable.TabIndex = 0;
            this.BUT_externalAHRS_gnss_enable.Text = "Enable GNSS";
            this.BUT_externalAHRS_gnss_enable.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_externalAHRS_gnss_enable.UseVisualStyleBackColor = true;
            this.BUT_externalAHRS_gnss_enable.Click += new System.EventHandler(this.BUT_externalAHRS_gnss_enable_Click);
            // 
            // BUT_externalAHRS_gnss_disable
            // 
            this.BUT_externalAHRS_gnss_disable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BUT_externalAHRS_gnss_disable.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BUT_externalAHRS_gnss_disable.Location = new System.Drawing.Point(70, 3);
            this.BUT_externalAHRS_gnss_disable.Name = "BUT_externalAHRS_gnss_disable";
            this.BUT_externalAHRS_gnss_disable.Size = new System.Drawing.Size(64, 58);
            this.BUT_externalAHRS_gnss_disable.TabIndex = 1;
            this.BUT_externalAHRS_gnss_disable.Text = "Disable GNSS";
            this.BUT_externalAHRS_gnss_disable.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_externalAHRS_gnss_disable.UseVisualStyleBackColor = true;
            this.BUT_externalAHRS_gnss_disable.Click += new System.EventHandler(this.BUT_externalAHRS_gnss_disable_Click);
            // 
            // BUT_externalAHRS_vg3dclb_flight_start
            // 
            this.BUT_externalAHRS_vg3dclb_flight_start.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BUT_externalAHRS_vg3dclb_flight_start.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BUT_externalAHRS_vg3dclb_flight_start.Location = new System.Drawing.Point(140, 3);
            this.BUT_externalAHRS_vg3dclb_flight_start.Name = "BUT_externalAHRS_vg3dclb_flight_start";
            this.BUT_externalAHRS_vg3dclb_flight_start.Size = new System.Drawing.Size(65, 58);
            this.BUT_externalAHRS_vg3dclb_flight_start.TabIndex = 2;
            this.BUT_externalAHRS_vg3dclb_flight_start.Text = "Start VG3D clb in flight";
            this.BUT_externalAHRS_vg3dclb_flight_start.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_externalAHRS_vg3dclb_flight_start.UseVisualStyleBackColor = true;
            this.BUT_externalAHRS_vg3dclb_flight_start.Click += new System.EventHandler(this.BUT_externalAHRS_vg3dclb_flight_start_Click);
            // 
            // BUT_externalAHRS_vg3dclb_flight_stop
            // 
            this.BUT_externalAHRS_vg3dclb_flight_stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BUT_externalAHRS_vg3dclb_flight_stop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BUT_externalAHRS_vg3dclb_flight_stop.Location = new System.Drawing.Point(211, 3);
            this.BUT_externalAHRS_vg3dclb_flight_stop.Name = "BUT_externalAHRS_vg3dclb_flight_stop";
            this.BUT_externalAHRS_vg3dclb_flight_stop.Size = new System.Drawing.Size(72, 58);
            this.BUT_externalAHRS_vg3dclb_flight_stop.TabIndex = 3;
            this.BUT_externalAHRS_vg3dclb_flight_stop.Text = "Stop VG3D clb in flight";
            this.BUT_externalAHRS_vg3dclb_flight_stop.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_externalAHRS_vg3dclb_flight_stop.UseVisualStyleBackColor = true;
            this.BUT_externalAHRS_vg3dclb_flight_stop.Click += new System.EventHandler(this.BUT_externalAHRS_vg3dclb_flight_stop_Click);
            // 
            // BUT_externalAHRS_start
            // 
            this.BUT_externalAHRS_start.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BUT_externalAHRS_start.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BUT_externalAHRS_start.Location = new System.Drawing.Point(140, 151);
            this.BUT_externalAHRS_start.Name = "BUT_externalAHRS_start";
            this.BUT_externalAHRS_start.Size = new System.Drawing.Size(65, 34);
            this.BUT_externalAHRS_start.TabIndex = 5;
            this.BUT_externalAHRS_start.Text = "Start";
            this.BUT_externalAHRS_start.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_externalAHRS_start.UseVisualStyleBackColor = true;
            this.BUT_externalAHRS_start.Click += new System.EventHandler(this.BUT_externalAHRS_start_Click);
            // 
            // BUT_externalAHRS_stop
            // 
            this.BUT_externalAHRS_stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BUT_externalAHRS_stop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BUT_externalAHRS_stop.Location = new System.Drawing.Point(211, 151);
            this.BUT_externalAHRS_stop.Name = "BUT_externalAHRS_stop";
            this.BUT_externalAHRS_stop.Size = new System.Drawing.Size(72, 34);
            this.BUT_externalAHRS_stop.TabIndex = 4;
            this.BUT_externalAHRS_stop.Text = "Stop";
            this.BUT_externalAHRS_stop.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_externalAHRS_stop.UseVisualStyleBackColor = true;
            this.BUT_externalAHRS_stop.Click += new System.EventHandler(this.BUT_externalAHRS_stop_Click);
            // 
            // BUT_externalAHRS_aiding_data
            // 
            this.BUT_externalAHRS_aiding_data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BUT_externalAHRS_aiding_data.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BUT_externalAHRS_aiding_data.Location = new System.Drawing.Point(211, 67);
            this.BUT_externalAHRS_aiding_data.Name = "BUT_externalAHRS_aiding_data";
            this.BUT_externalAHRS_aiding_data.Size = new System.Drawing.Size(72, 39);
            this.BUT_externalAHRS_aiding_data.TabIndex = 4;
            this.BUT_externalAHRS_aiding_data.Text = "Aiding data";
            this.BUT_externalAHRS_aiding_data.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_externalAHRS_aiding_data.UseVisualStyleBackColor = true;
            this.BUT_externalAHRS_aiding_data.Click += new System.EventHandler(this.BUT_externalAHRS_aiding_data_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.3986F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.6014F));
            this.tableLayoutPanel2.Controls.Add(this.CB_gnss_position, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.CB_ins_pos_estimation, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.CB_gcs_distance, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.NUD_gcs_distance_around, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 197);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(286, 50);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // CB_gnss_position
            // 
            this.CB_gnss_position.AutoSize = true;
            this.CB_gnss_position.Location = new System.Drawing.Point(3, 3);
            this.CB_gnss_position.Name = "CB_gnss_position";
            this.CB_gnss_position.Size = new System.Drawing.Size(118, 19);
            this.CB_gnss_position.TabIndex = 0;
            this.CB_gnss_position.Text = "GNSS Position";
            this.CB_gnss_position.UseVisualStyleBackColor = true;
            this.CB_gnss_position.CheckedChanged += new System.EventHandler(this.CB_gnss_position_CheckedChanged);
            // 
            // CB_ins_pos_estimation
            // 
            this.CB_ins_pos_estimation.AutoSize = true;
            this.CB_ins_pos_estimation.Location = new System.Drawing.Point(3, 28);
            this.CB_ins_pos_estimation.Name = "CB_ins_pos_estimation";
            this.CB_ins_pos_estimation.Size = new System.Drawing.Size(141, 19);
            this.CB_ins_pos_estimation.TabIndex = 1;
            this.CB_ins_pos_estimation.Text = "INS position estimation";
            this.CB_ins_pos_estimation.UseVisualStyleBackColor = true;
            this.CB_ins_pos_estimation.CheckedChanged += new System.EventHandler(this.CB_ins_pos_estimation_CheckedChanged);
            // 
            // CB_gcs_distance
            // 
            this.CB_gcs_distance.AutoSize = true;
            this.CB_gcs_distance.Location = new System.Drawing.Point(150, 3);
            this.CB_gcs_distance.Name = "CB_gcs_distance";
            this.CB_gcs_distance.Size = new System.Drawing.Size(133, 19);
            this.CB_gcs_distance.TabIndex = 2;
            this.CB_gcs_distance.Text = "GCS distance (m)";
            this.CB_gcs_distance.UseVisualStyleBackColor = true;
            this.CB_gcs_distance.CheckedChanged += new System.EventHandler(this.CB_gcs_distance_CheckedChanged);
            // 
            // NUD_gcs_distance_around
            // 
            this.NUD_gcs_distance_around.Location = new System.Drawing.Point(150, 28);
            this.NUD_gcs_distance_around.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.NUD_gcs_distance_around.Name = "NUD_gcs_distance_around";
            this.NUD_gcs_distance_around.Size = new System.Drawing.Size(120, 22);
            this.NUD_gcs_distance_around.TabIndex = 3;
            this.NUD_gcs_distance_around.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NUD_gcs_distance_around.ValueChanged += new System.EventHandler(this.NUD_gcs_distance_around_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Location = new System.Drawing.Point(295, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(131, 188);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "INS/GNSS pos diff";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.quickView1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.quickView2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.quickView3, 0, 2);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 18);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(119, 164);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // quickView1
            // 
            this.quickView1.desc = "North (m)";
            this.quickView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quickView1.Location = new System.Drawing.Point(3, 3);
            this.quickView1.Name = "quickView1";
            this.quickView1.number = 0D;
            this.quickView1.numberColor = System.Drawing.Color.Empty;
            this.quickView1.numberColorBackup = System.Drawing.Color.Empty;
            this.quickView1.numberformat = "0.00";
            this.quickView1.Size = new System.Drawing.Size(113, 47);
            this.quickView1.TabIndex = 0;
            this.quickView1.Text = "quickView1";
            // 
            // quickView2
            // 
            this.quickView2.desc = "East (m)";
            this.quickView2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quickView2.Location = new System.Drawing.Point(3, 57);
            this.quickView2.Name = "quickView2";
            this.quickView2.number = 0D;
            this.quickView2.numberColor = System.Drawing.Color.Empty;
            this.quickView2.numberColorBackup = System.Drawing.Color.Empty;
            this.quickView2.numberformat = "0.00";
            this.quickView2.Size = new System.Drawing.Size(113, 47);
            this.quickView2.TabIndex = 1;
            this.quickView2.Text = "quickView2";
            // 
            // quickView3
            // 
            this.quickView3.desc = "Down (m)";
            this.quickView3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quickView3.Location = new System.Drawing.Point(3, 111);
            this.quickView3.Name = "quickView3";
            this.quickView3.number = 0D;
            this.quickView3.numberColor = System.Drawing.Color.Empty;
            this.quickView3.numberColorBackup = System.Drawing.Color.Empty;
            this.quickView3.numberformat = "0.00";
            this.quickView3.Size = new System.Drawing.Size(113, 50);
            this.quickView3.TabIndex = 2;
            this.quickView3.Text = "quickView3";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tableLayoutPanel4);
            this.groupBox2.Location = new System.Drawing.Point(432, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(127, 188);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "INS pos accuracy";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.quickView4, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.quickView5, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.quickView6, 0, 2);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(6, 18);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(115, 164);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // quickView4
            // 
            this.quickView4.desc = "North (m)";
            this.quickView4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quickView4.Location = new System.Drawing.Point(3, 3);
            this.quickView4.Name = "quickView4";
            this.quickView4.number = 0D;
            this.quickView4.numberColor = System.Drawing.Color.Empty;
            this.quickView4.numberColorBackup = System.Drawing.Color.Empty;
            this.quickView4.numberformat = "0.00";
            this.quickView4.Size = new System.Drawing.Size(109, 47);
            this.quickView4.TabIndex = 0;
            this.quickView4.Text = "quickView4";
            // 
            // quickView5
            // 
            this.quickView5.desc = "East (m)";
            this.quickView5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quickView5.Location = new System.Drawing.Point(3, 57);
            this.quickView5.Name = "quickView5";
            this.quickView5.number = 0D;
            this.quickView5.numberColor = System.Drawing.Color.Empty;
            this.quickView5.numberColorBackup = System.Drawing.Color.Empty;
            this.quickView5.numberformat = "0.00";
            this.quickView5.Size = new System.Drawing.Size(109, 47);
            this.quickView5.TabIndex = 1;
            this.quickView5.Text = "quickView5";
            // 
            // quickView6
            // 
            this.quickView6.desc = "Down (m)";
            this.quickView6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quickView6.Location = new System.Drawing.Point(3, 111);
            this.quickView6.Name = "quickView6";
            this.quickView6.number = 0D;
            this.quickView6.numberColor = System.Drawing.Color.Empty;
            this.quickView6.numberColorBackup = System.Drawing.Color.Empty;
            this.quickView6.numberformat = "0.00";
            this.quickView6.Size = new System.Drawing.Size(109, 50);
            this.quickView6.TabIndex = 2;
            this.quickView6.Text = "quickView6";
            // 
            // EAHRSControl
            // 
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "EAHRSControl";
            this.Size = new System.Drawing.Size(565, 252);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.EAHRSButtonsTableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUD_gcs_distance_around)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void BUT_externalAHRS_gnss_enable_Click(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, MAVLink.MAV_CMD.EXTERNAL_AHRS_ENABLE_GNSS, 0, 0, 0, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.ToString(), Strings.ERROR);
            }
        }

        private void BUT_externalAHRS_gnss_disable_Click(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, MAVLink.MAV_CMD.EXTERNAL_AHRS_DISABLE_GNSS, 0, 0, 0, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.ToString(), Strings.ERROR);
            }
        }

        private void BUT_externalAHRS_vg3dclb_flight_start_Click(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, MAVLink.MAV_CMD.EXTERNAL_AHRS_START_VG3D_CALIBRATION_IN_FLIGHT, 0, 0, 0, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.ToString(), Strings.ERROR);
            }
        }

        private void BUT_externalAHRS_vg3dclb_flight_stop_Click(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, MAVLink.MAV_CMD.EXTERNAL_AHRS_STOP_VG3D_CALIBRATION_IN_FLIGHT, 0, 0, 0, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.ToString(), Strings.ERROR);
            }
        }

        private void BUT_externalAHRS_start_Click(object sender, EventArgs e)
        {
            try
            {
                MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, MAVLink.MAV_CMD.EXTERNAL_AHRS_START_UDD, 0, 0, 0, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.ToString(), Strings.ERROR);
            }
        }

        private void BUT_externalAHRS_stop_Click(object sender, EventArgs e)
        {
            if (CustomMessageBox.Show("Attention! External AHRS will be stopped.\nAre you Sure?",
                    "Are you sure?", CustomMessageBox.MessageBoxButtons.OKCancel) !=
                    CustomMessageBox.DialogResult.OK)
            {
                return;
            }
            try
            {
                MainV2.comPort.doCommandInt(MainV2.comPort.MAV.sysid, MainV2.comPort.MAV.compid, MAVLink.MAV_CMD.EXTERNAL_AHRS_STOP, 0, 0, 0, 0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(Strings.CommandFailed + ex.ToString(), Strings.ERROR);
            }
        }

        private void BUT_externalAHRS_aiding_data_Click(object sender, EventArgs e)
        {
            ThemeManager.ApplyThemeTo(aidingDataDialog);
            aidingDataDialog.Show();
        }

        private void CB_gnss_position_CheckedChanged(object sender, EventArgs e)
        {
            MainV2.comPort.MAV.cs.show_gps_raw_location = CB_gnss_position.Checked;
        }

        private void CB_ins_pos_estimation_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CB_gcs_distance_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void NUD_gcs_distance_around_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}

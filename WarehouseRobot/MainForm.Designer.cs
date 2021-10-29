
namespace WarehouseRobot
{
    partial class MainForm
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
            this.beginSimulateButton = new System.Windows.Forms.Button();
            this.simulatePanel = new System.Windows.Forms.Panel();
            this.drawButton = new System.Windows.Forms.Button();
            this.stopSimulateButton = new System.Windows.Forms.Button();
            this.rowCountUpDown = new System.Windows.Forms.NumericUpDown();
            this.colCountUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.addTaskButton = new System.Windows.Forms.Button();
            this.queuingTaskCountLabel = new System.Windows.Forms.Label();
            this.runningTaskCountLabel = new System.Windows.Forms.Label();
            this.allRobotCountLabel = new System.Windows.Forms.Label();
            this.setBeginButton = new System.Windows.Forms.Button();
            this.setObstacleButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.rowCountUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colCountUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // beginSimulateButton
            // 
            this.beginSimulateButton.Enabled = false;
            this.beginSimulateButton.Location = new System.Drawing.Point(63, 585);
            this.beginSimulateButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.beginSimulateButton.Name = "beginSimulateButton";
            this.beginSimulateButton.Size = new System.Drawing.Size(73, 25);
            this.beginSimulateButton.TabIndex = 0;
            this.beginSimulateButton.Text = "开始计算";
            this.beginSimulateButton.UseVisualStyleBackColor = true;
            this.beginSimulateButton.Click += new System.EventHandler(this.BeginSimulateButton_Click);
            // 
            // simulatePanel
            // 
            this.simulatePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.simulatePanel.BackColor = System.Drawing.SystemColors.Control;
            this.simulatePanel.Location = new System.Drawing.Point(9, 12);
            this.simulatePanel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.simulatePanel.Name = "simulatePanel";
            this.simulatePanel.Size = new System.Drawing.Size(1038, 644);
            this.simulatePanel.TabIndex = 1;
            this.simulatePanel.SizeChanged += new System.EventHandler(this.SimulatePanel_SizeChanged);
            this.simulatePanel.Click += new System.EventHandler(this.SimulatePanel_Click);
            // 
            // drawButton
            // 
            this.drawButton.Location = new System.Drawing.Point(53, 299);
            this.drawButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.drawButton.Name = "drawButton";
            this.drawButton.Size = new System.Drawing.Size(87, 25);
            this.drawButton.TabIndex = 2;
            this.drawButton.Text = "绘制仓库";
            this.drawButton.UseVisualStyleBackColor = true;
            this.drawButton.Click += new System.EventHandler(this.DrawButton_Click);
            // 
            // stopSimulateButton
            // 
            this.stopSimulateButton.Location = new System.Drawing.Point(63, 555);
            this.stopSimulateButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.stopSimulateButton.Name = "stopSimulateButton";
            this.stopSimulateButton.Size = new System.Drawing.Size(73, 25);
            this.stopSimulateButton.TabIndex = 3;
            this.stopSimulateButton.Text = "停止计算";
            this.stopSimulateButton.UseVisualStyleBackColor = true;
            this.stopSimulateButton.Click += new System.EventHandler(this.StopSimulateButton_Click);
            // 
            // rowCountUpDown
            // 
            this.rowCountUpDown.Location = new System.Drawing.Point(87, 242);
            this.rowCountUpDown.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rowCountUpDown.Name = "rowCountUpDown";
            this.rowCountUpDown.Size = new System.Drawing.Size(53, 23);
            this.rowCountUpDown.TabIndex = 4;
            this.rowCountUpDown.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // colCountUpDown
            // 
            this.colCountUpDown.Location = new System.Drawing.Point(87, 271);
            this.colCountUpDown.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.colCountUpDown.Name = "colCountUpDown";
            this.colCountUpDown.Size = new System.Drawing.Size(53, 23);
            this.colCountUpDown.TabIndex = 5;
            this.colCountUpDown.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 244);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "行：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 272);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "列：";
            // 
            // addTaskButton
            // 
            this.addTaskButton.Enabled = false;
            this.addTaskButton.Location = new System.Drawing.Point(63, 517);
            this.addTaskButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.addTaskButton.Name = "addTaskButton";
            this.addTaskButton.Size = new System.Drawing.Size(73, 25);
            this.addTaskButton.TabIndex = 8;
            this.addTaskButton.Text = "增加任务";
            this.addTaskButton.UseVisualStyleBackColor = true;
            this.addTaskButton.Click += new System.EventHandler(this.AddTaskButton_Click);
            // 
            // queuingTaskCountLabel
            // 
            this.queuingTaskCountLabel.AutoSize = true;
            this.queuingTaskCountLabel.Location = new System.Drawing.Point(5, 55);
            this.queuingTaskCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.queuingTaskCountLabel.Name = "queuingTaskCountLabel";
            this.queuingTaskCountLabel.Size = new System.Drawing.Size(104, 17);
            this.queuingTaskCountLabel.TabIndex = 9;
            this.queuingTaskCountLabel.Text = "排队任务数：待定";
            // 
            // runningTaskCountLabel
            // 
            this.runningTaskCountLabel.AutoSize = true;
            this.runningTaskCountLabel.Location = new System.Drawing.Point(5, 83);
            this.runningTaskCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.runningTaskCountLabel.Name = "runningTaskCountLabel";
            this.runningTaskCountLabel.Size = new System.Drawing.Size(104, 17);
            this.runningTaskCountLabel.TabIndex = 10;
            this.runningTaskCountLabel.Text = "执行任务数：待定";
            // 
            // allRobotCountLabel
            // 
            this.allRobotCountLabel.AutoSize = true;
            this.allRobotCountLabel.Location = new System.Drawing.Point(5, 26);
            this.allRobotCountLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.allRobotCountLabel.Name = "allRobotCountLabel";
            this.allRobotCountLabel.Size = new System.Drawing.Size(116, 17);
            this.allRobotCountLabel.TabIndex = 11;
            this.allRobotCountLabel.Text = "总共机器人数：待定";
            // 
            // setBeginButton
            // 
            this.setBeginButton.Enabled = false;
            this.setBeginButton.Location = new System.Drawing.Point(7, 126);
            this.setBeginButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.setBeginButton.Name = "setBeginButton";
            this.setBeginButton.Size = new System.Drawing.Size(188, 25);
            this.setBeginButton.TabIndex = 12;
            this.setBeginButton.Text = "设置出发点";
            this.setBeginButton.UseVisualStyleBackColor = true;
            this.setBeginButton.Click += new System.EventHandler(this.SetBeginButton_Click);
            // 
            // setObstacleButton
            // 
            this.setObstacleButton.Enabled = false;
            this.setObstacleButton.Location = new System.Drawing.Point(7, 157);
            this.setObstacleButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.setObstacleButton.Name = "setObstacleButton";
            this.setObstacleButton.Size = new System.Drawing.Size(188, 25);
            this.setObstacleButton.TabIndex = 13;
            this.setObstacleButton.Text = "设置障碍物";
            this.setObstacleButton.UseVisualStyleBackColor = true;
            this.setObstacleButton.Click += new System.EventHandler(this.SetObstacleButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.queuingTaskCountLabel);
            this.groupBox1.Controls.Add(this.setObstacleButton);
            this.groupBox1.Controls.Add(this.runningTaskCountLabel);
            this.groupBox1.Controls.Add(this.drawButton);
            this.groupBox1.Controls.Add(this.allRobotCountLabel);
            this.groupBox1.Controls.Add(this.setBeginButton);
            this.groupBox1.Controls.Add(this.colCountUpDown);
            this.groupBox1.Controls.Add(this.stopSimulateButton);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.beginSimulateButton);
            this.groupBox1.Controls.Add(this.rowCountUpDown);
            this.groupBox1.Controls.Add(this.addTaskButton);
            this.groupBox1.Location = new System.Drawing.Point(1052, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 644);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 659);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1264, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.simulatePanel);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.rowCountUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colCountUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button beginSimulateButton;
        private System.Windows.Forms.Panel simulatePanel;
        private System.Windows.Forms.Button drawButton;
        private System.Windows.Forms.Button stopSimulateButton;
        private System.Windows.Forms.NumericUpDown rowCountUpDown;
        private System.Windows.Forms.NumericUpDown colCountUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button addTaskButton;
        private System.Windows.Forms.Label queuingTaskCountLabel;
        private System.Windows.Forms.Label runningTaskCountLabel;
        private System.Windows.Forms.Label allRobotCountLabel;
        private System.Windows.Forms.Button setBeginButton;
        private System.Windows.Forms.Button setObstacleButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    }
}
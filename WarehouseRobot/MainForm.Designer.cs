
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
            ((System.ComponentModel.ISupportInitialize)(this.rowCountUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colCountUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // beginSimulateButton
            // 
            this.beginSimulateButton.Enabled = false;
            this.beginSimulateButton.Location = new System.Drawing.Point(1156, 632);
            this.beginSimulateButton.Name = "beginSimulateButton";
            this.beginSimulateButton.Size = new System.Drawing.Size(94, 29);
            this.beginSimulateButton.TabIndex = 0;
            this.beginSimulateButton.Text = "开始计算";
            this.beginSimulateButton.UseVisualStyleBackColor = true;
            this.beginSimulateButton.Click += new System.EventHandler(this.BeginSimulateButton_Click);
            // 
            // simulatePanel
            // 
            this.simulatePanel.Location = new System.Drawing.Point(12, 30);
            this.simulatePanel.Name = "simulatePanel";
            this.simulatePanel.Size = new System.Drawing.Size(1088, 631);
            this.simulatePanel.TabIndex = 1;
            this.simulatePanel.Click += new System.EventHandler(this.SimulatePanel_Click);
            // 
            // drawButton
            // 
            this.drawButton.Location = new System.Drawing.Point(1156, 423);
            this.drawButton.Name = "drawButton";
            this.drawButton.Size = new System.Drawing.Size(94, 29);
            this.drawButton.TabIndex = 2;
            this.drawButton.Text = "绘制仓库";
            this.drawButton.UseVisualStyleBackColor = true;
            this.drawButton.Click += new System.EventHandler(this.DrawButton_Click);
            // 
            // stopSimulateButton
            // 
            this.stopSimulateButton.Location = new System.Drawing.Point(1156, 597);
            this.stopSimulateButton.Name = "stopSimulateButton";
            this.stopSimulateButton.Size = new System.Drawing.Size(94, 29);
            this.stopSimulateButton.TabIndex = 3;
            this.stopSimulateButton.Text = "停止计算";
            this.stopSimulateButton.UseVisualStyleBackColor = true;
            this.stopSimulateButton.Click += new System.EventHandler(this.StopSimulateButton_Click);
            // 
            // rowCountUpDown
            // 
            this.rowCountUpDown.Location = new System.Drawing.Point(1200, 357);
            this.rowCountUpDown.Name = "rowCountUpDown";
            this.rowCountUpDown.Size = new System.Drawing.Size(50, 27);
            this.rowCountUpDown.TabIndex = 4;
            this.rowCountUpDown.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // colCountUpDown
            // 
            this.colCountUpDown.Location = new System.Drawing.Point(1200, 390);
            this.colCountUpDown.Name = "colCountUpDown";
            this.colCountUpDown.Size = new System.Drawing.Size(50, 27);
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
            this.label1.Location = new System.Drawing.Point(1156, 359);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "行：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1156, 392);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "列：";
            // 
            // addTaskButton
            // 
            this.addTaskButton.Enabled = false;
            this.addTaskButton.Location = new System.Drawing.Point(1156, 552);
            this.addTaskButton.Name = "addTaskButton";
            this.addTaskButton.Size = new System.Drawing.Size(94, 29);
            this.addTaskButton.TabIndex = 8;
            this.addTaskButton.Text = "增加任务";
            this.addTaskButton.UseVisualStyleBackColor = true;
            this.addTaskButton.Click += new System.EventHandler(this.AddTaskButton_Click);
            // 
            // queuingTaskCountLabel
            // 
            this.queuingTaskCountLabel.AutoSize = true;
            this.queuingTaskCountLabel.Location = new System.Drawing.Point(1106, 65);
            this.queuingTaskCountLabel.Name = "queuingTaskCountLabel";
            this.queuingTaskCountLabel.Size = new System.Drawing.Size(129, 20);
            this.queuingTaskCountLabel.TabIndex = 9;
            this.queuingTaskCountLabel.Text = "排队任务数：待定";
            // 
            // runningTaskCountLabel
            // 
            this.runningTaskCountLabel.AutoSize = true;
            this.runningTaskCountLabel.Location = new System.Drawing.Point(1106, 98);
            this.runningTaskCountLabel.Name = "runningTaskCountLabel";
            this.runningTaskCountLabel.Size = new System.Drawing.Size(129, 20);
            this.runningTaskCountLabel.TabIndex = 10;
            this.runningTaskCountLabel.Text = "执行任务数：待定";
            // 
            // allRobotCountLabel
            // 
            this.allRobotCountLabel.AutoSize = true;
            this.allRobotCountLabel.Location = new System.Drawing.Point(1106, 30);
            this.allRobotCountLabel.Name = "allRobotCountLabel";
            this.allRobotCountLabel.Size = new System.Drawing.Size(144, 20);
            this.allRobotCountLabel.TabIndex = 11;
            this.allRobotCountLabel.Text = "总共机器人数：待定";
            // 
            // setBeginButton
            // 
            this.setBeginButton.Enabled = false;
            this.setBeginButton.Location = new System.Drawing.Point(1132, 187);
            this.setBeginButton.Name = "setBeginButton";
            this.setBeginButton.Size = new System.Drawing.Size(94, 29);
            this.setBeginButton.TabIndex = 12;
            this.setBeginButton.Text = "设置出发点";
            this.setBeginButton.UseVisualStyleBackColor = true;
            // 
            // setObstacleButton
            // 
            this.setObstacleButton.Enabled = false;
            this.setObstacleButton.Location = new System.Drawing.Point(1132, 239);
            this.setObstacleButton.Name = "setObstacleButton";
            this.setObstacleButton.Size = new System.Drawing.Size(94, 29);
            this.setObstacleButton.TabIndex = 13;
            this.setObstacleButton.Text = "设置障碍物";
            this.setObstacleButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.setObstacleButton);
            this.Controls.Add(this.setBeginButton);
            this.Controls.Add(this.allRobotCountLabel);
            this.Controls.Add(this.runningTaskCountLabel);
            this.Controls.Add(this.queuingTaskCountLabel);
            this.Controls.Add(this.addTaskButton);
            this.Controls.Add(this.rowCountUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.colCountUpDown);
            this.Controls.Add(this.stopSimulateButton);
            this.Controls.Add(this.drawButton);
            this.Controls.Add(this.simulatePanel);
            this.Controls.Add(this.beginSimulateButton);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.rowCountUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colCountUpDown)).EndInit();
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
    }
}
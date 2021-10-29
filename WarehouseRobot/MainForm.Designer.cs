
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
            this.SuspendLayout();
            // 
            // beginSimulateButton
            // 
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
            this.simulatePanel.Size = new System.Drawing.Size(1138, 631);
            this.simulatePanel.TabIndex = 1;
            // 
            // drawButton
            // 
            this.drawButton.Location = new System.Drawing.Point(1156, 597);
            this.drawButton.Name = "drawButton";
            this.drawButton.Size = new System.Drawing.Size(94, 29);
            this.drawButton.TabIndex = 2;
            this.drawButton.Text = "绘制仓库";
            this.drawButton.UseVisualStyleBackColor = true;
            this.drawButton.Click += new System.EventHandler(this.DrawButton_Click);
            // 
            // stopSimulateButton
            // 
            this.stopSimulateButton.Location = new System.Drawing.Point(1156, 562);
            this.stopSimulateButton.Name = "stopSimulateButton";
            this.stopSimulateButton.Size = new System.Drawing.Size(94, 29);
            this.stopSimulateButton.TabIndex = 3;
            this.stopSimulateButton.Text = "停止计算";
            this.stopSimulateButton.UseVisualStyleBackColor = true;
            this.stopSimulateButton.Click += new System.EventHandler(this.StopSimulateButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.stopSimulateButton);
            this.Controls.Add(this.drawButton);
            this.Controls.Add(this.simulatePanel);
            this.Controls.Add(this.beginSimulateButton);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button beginSimulateButton;
        private System.Windows.Forms.Panel simulatePanel;
        private System.Windows.Forms.Button drawButton;
        private System.Windows.Forms.Button stopSimulateButton;
    }
}
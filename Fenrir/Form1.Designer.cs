namespace Fenrir
{
    partial class Fenrir
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fenrir));
            this.ProxyStartButton = new System.Windows.Forms.Button();
            this.portNumber = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.portNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // ProxyStartButton
            // 
            this.ProxyStartButton.Location = new System.Drawing.Point(16, 71);
            this.ProxyStartButton.Margin = new System.Windows.Forms.Padding(4);
            this.ProxyStartButton.Name = "ProxyStartButton";
            this.ProxyStartButton.Size = new System.Drawing.Size(169, 28);
            this.ProxyStartButton.TabIndex = 0;
            this.ProxyStartButton.Text = "Start Forwarder";
            this.ProxyStartButton.UseVisualStyleBackColor = true;
            this.ProxyStartButton.Click += new System.EventHandler(this.ProxyStartButton_Click);
            // 
            // portNumber
            // 
            this.portNumber.Location = new System.Drawing.Point(25, 25);
            this.portNumber.Margin = new System.Windows.Forms.Padding(4);
            this.portNumber.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.portNumber.Minimum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.portNumber.Name = "portNumber";
            this.portNumber.Size = new System.Drawing.Size(160, 22);
            this.portNumber.TabIndex = 1;
            this.portNumber.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Local Port";
            // 
            // Fenrir
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(214, 150);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.portNumber);
            this.Controls.Add(this.ProxyStartButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Fenrir";
            this.Text = "Fenrir";
            ((System.ComponentModel.ISupportInitialize)(this.portNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ProxyStartButton;
        public System.Windows.Forms.NumericUpDown portNumber;
        private System.Windows.Forms.Label label1;
    }
}


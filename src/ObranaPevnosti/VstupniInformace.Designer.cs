namespace ObranaPevnosti
{
    partial class VstupniInformace
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
            this.JmenoUtocnika = new System.Windows.Forms.TextBox();
            this.JmenoObrance = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.utokComboBox = new System.Windows.Forms.ComboBox();
            this.obranaComboBox = new System.Windows.Forms.ComboBox();
            this.VstupniInformaceButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // JmenoUtocnika
            // 
            this.JmenoUtocnika.Location = new System.Drawing.Point(56, 12);
            this.JmenoUtocnika.Name = "JmenoUtocnika";
            this.JmenoUtocnika.Size = new System.Drawing.Size(100, 20);
            this.JmenoUtocnika.TabIndex = 0;
            this.JmenoUtocnika.Text = "Tomáš";
            // 
            // JmenoObrance
            // 
            this.JmenoObrance.Location = new System.Drawing.Point(56, 38);
            this.JmenoObrance.Name = "JmenoObrance";
            this.JmenoObrance.Size = new System.Drawing.Size(100, 20);
            this.JmenoObrance.TabIndex = 1;
            this.JmenoObrance.Text = "Jana";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Útok:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Obrana:";
            // 
            // utokComboBox
            // 
            this.utokComboBox.FormattingEnabled = true;
            this.utokComboBox.Items.AddRange(new object[] {
            "Lidský Hráč",
            "Počítačový hráč"});
            this.utokComboBox.Location = new System.Drawing.Point(162, 11);
            this.utokComboBox.Name = "utokComboBox";
            this.utokComboBox.Size = new System.Drawing.Size(121, 21);
            this.utokComboBox.TabIndex = 4;
            // 
            // obranaComboBox
            // 
            this.obranaComboBox.FormattingEnabled = true;
            this.obranaComboBox.Items.AddRange(new object[] {
            "Lidský Hráč",
            "Počítačový hráč"});
            this.obranaComboBox.Location = new System.Drawing.Point(162, 38);
            this.obranaComboBox.Name = "obranaComboBox";
            this.obranaComboBox.Size = new System.Drawing.Size(121, 21);
            this.obranaComboBox.TabIndex = 5;
            // 
            // VstupniInformaceButton
            // 
            this.VstupniInformaceButton.Location = new System.Drawing.Point(208, 65);
            this.VstupniInformaceButton.Name = "VstupniInformaceButton";
            this.VstupniInformaceButton.Size = new System.Drawing.Size(75, 23);
            this.VstupniInformaceButton.TabIndex = 6;
            this.VstupniInformaceButton.Text = "&Začít hru";
            this.VstupniInformaceButton.UseVisualStyleBackColor = true;
            this.VstupniInformaceButton.Click += new System.EventHandler(this.VstupniInformaceButton_Click);
            // 
            // VstupniInformace
            // 
            this.AcceptButton = this.VstupniInformaceButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 92);
            this.Controls.Add(this.VstupniInformaceButton);
            this.Controls.Add(this.obranaComboBox);
            this.Controls.Add(this.utokComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.JmenoObrance);
            this.Controls.Add(this.JmenoUtocnika);
            this.Name = "VstupniInformace";
            this.Text = "Nová hra";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox JmenoUtocnika;
        private System.Windows.Forms.TextBox JmenoObrance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox utokComboBox;
        private System.Windows.Forms.ComboBox obranaComboBox;
        private System.Windows.Forms.Button VstupniInformaceButton;
    }
}
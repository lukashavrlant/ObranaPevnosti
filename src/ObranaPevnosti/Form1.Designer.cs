namespace ObranaPevnosti
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.obranaPevnostiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nováHraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartHryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uložitHruToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nahrátHruToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.konecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.obtížnostToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.snadnáToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.normálníToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.těžkáToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.možnostiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zpětToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vpředToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nejlepšíTahToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prohoďHráčeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.přehrátToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Platno = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.NaRadeLabel = new System.Windows.Forms.Label();
            this.PocitaciHraj = new System.Windows.Forms.Button();
            this.Historie = new System.Windows.Forms.ListBox();
            this.Pausa = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.Platno)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.obranaPevnostiToolStripMenuItem,
            this.obtížnostToolStripMenuItem,
            this.možnostiToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(744, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // obranaPevnostiToolStripMenuItem
            // 
            this.obranaPevnostiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nováHraToolStripMenuItem,
            this.restartHryToolStripMenuItem,
            this.uložitHruToolStripMenuItem,
            this.nahrátHruToolStripMenuItem,
            this.konecToolStripMenuItem});
            this.obranaPevnostiToolStripMenuItem.Name = "obranaPevnostiToolStripMenuItem";
            this.obranaPevnostiToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.obranaPevnostiToolStripMenuItem.Text = "&Hra";
            // 
            // nováHraToolStripMenuItem
            // 
            this.nováHraToolStripMenuItem.Name = "nováHraToolStripMenuItem";
            this.nováHraToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.nováHraToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.nováHraToolStripMenuItem.Text = "&Nová hra";
            this.nováHraToolStripMenuItem.Click += new System.EventHandler(this.nováHraToolStripMenuItem_Click);
            // 
            // restartHryToolStripMenuItem
            // 
            this.restartHryToolStripMenuItem.Enabled = false;
            this.restartHryToolStripMenuItem.Name = "restartHryToolStripMenuItem";
            this.restartHryToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.restartHryToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.restartHryToolStripMenuItem.Text = "&Restart hry";
            this.restartHryToolStripMenuItem.Click += new System.EventHandler(this.restartHryToolStripMenuItem_Click);
            // 
            // uložitHruToolStripMenuItem
            // 
            this.uložitHruToolStripMenuItem.Enabled = false;
            this.uložitHruToolStripMenuItem.Name = "uložitHruToolStripMenuItem";
            this.uložitHruToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.uložitHruToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.uložitHruToolStripMenuItem.Text = "&Uložit hru";
            this.uložitHruToolStripMenuItem.Click += new System.EventHandler(this.uložitHruToolStripMenuItem_Click);
            // 
            // nahrátHruToolStripMenuItem
            // 
            this.nahrátHruToolStripMenuItem.Name = "nahrátHruToolStripMenuItem";
            this.nahrátHruToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.nahrátHruToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.nahrátHruToolStripMenuItem.Text = "Na&hrát hru";
            this.nahrátHruToolStripMenuItem.Click += new System.EventHandler(this.nahrátHruToolStripMenuItem_Click);
            // 
            // konecToolStripMenuItem
            // 
            this.konecToolStripMenuItem.Name = "konecToolStripMenuItem";
            this.konecToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.konecToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.konecToolStripMenuItem.Text = "&Konec";
            this.konecToolStripMenuItem.Click += new System.EventHandler(this.konecToolStripMenuItem_Click);
            // 
            // obtížnostToolStripMenuItem
            // 
            this.obtížnostToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.snadnáToolStripMenuItem,
            this.normálníToolStripMenuItem,
            this.těžkáToolStripMenuItem});
            this.obtížnostToolStripMenuItem.Name = "obtížnostToolStripMenuItem";
            this.obtížnostToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.obtížnostToolStripMenuItem.Text = "&Obtížnost";
            // 
            // snadnáToolStripMenuItem
            // 
            this.snadnáToolStripMenuItem.Name = "snadnáToolStripMenuItem";
            this.snadnáToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.snadnáToolStripMenuItem.Text = "&Snadná";
            this.snadnáToolStripMenuItem.Click += new System.EventHandler(this.snadnáToolStripMenuItem_Click);
            // 
            // normálníToolStripMenuItem
            // 
            this.normálníToolStripMenuItem.Checked = true;
            this.normálníToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.normálníToolStripMenuItem.Name = "normálníToolStripMenuItem";
            this.normálníToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.normálníToolStripMenuItem.Text = "&Normální";
            this.normálníToolStripMenuItem.Click += new System.EventHandler(this.normálníToolStripMenuItem_Click);
            // 
            // těžkáToolStripMenuItem
            // 
            this.těžkáToolStripMenuItem.Name = "těžkáToolStripMenuItem";
            this.těžkáToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.těžkáToolStripMenuItem.Text = "&Těžká";
            this.těžkáToolStripMenuItem.Click += new System.EventHandler(this.těžkáToolStripMenuItem_Click);
            // 
            // možnostiToolStripMenuItem
            // 
            this.možnostiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zpětToolStripMenuItem,
            this.vpředToolStripMenuItem,
            this.nejlepšíTahToolStripMenuItem,
            this.prohoďHráčeToolStripMenuItem,
            this.přehrátToolStripMenuItem,
            this.refreshToolStripMenuItem});
            this.možnostiToolStripMenuItem.Enabled = false;
            this.možnostiToolStripMenuItem.Name = "možnostiToolStripMenuItem";
            this.možnostiToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.možnostiToolStripMenuItem.Text = "&Možnosti";
            // 
            // zpětToolStripMenuItem
            // 
            this.zpětToolStripMenuItem.Enabled = false;
            this.zpětToolStripMenuItem.Name = "zpětToolStripMenuItem";
            this.zpětToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.zpětToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.zpětToolStripMenuItem.Text = "&Zpět";
            this.zpětToolStripMenuItem.Click += new System.EventHandler(this.zpětToolStripMenuItem_Click);
            // 
            // vpředToolStripMenuItem
            // 
            this.vpředToolStripMenuItem.Enabled = false;
            this.vpředToolStripMenuItem.Name = "vpředToolStripMenuItem";
            this.vpředToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.vpředToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.vpředToolStripMenuItem.Text = "&Vpřed";
            this.vpředToolStripMenuItem.Click += new System.EventHandler(this.vpředToolStripMenuItem_Click);
            // 
            // nejlepšíTahToolStripMenuItem
            // 
            this.nejlepšíTahToolStripMenuItem.Enabled = false;
            this.nejlepšíTahToolStripMenuItem.Name = "nejlepšíTahToolStripMenuItem";
            this.nejlepšíTahToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.nejlepšíTahToolStripMenuItem.Text = "&Nejlepší tah";
            this.nejlepšíTahToolStripMenuItem.Click += new System.EventHandler(this.nejlepšíTahToolStripMenuItem_Click);
            // 
            // prohoďHráčeToolStripMenuItem
            // 
            this.prohoďHráčeToolStripMenuItem.Enabled = false;
            this.prohoďHráčeToolStripMenuItem.Name = "prohoďHráčeToolStripMenuItem";
            this.prohoďHráčeToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.prohoďHráčeToolStripMenuItem.Text = "&Prohoď hráče";
            this.prohoďHráčeToolStripMenuItem.Click += new System.EventHandler(this.prohoďHráčeToolStripMenuItem_Click);
            // 
            // přehrátToolStripMenuItem
            // 
            this.přehrátToolStripMenuItem.Enabled = false;
            this.přehrátToolStripMenuItem.Name = "přehrátToolStripMenuItem";
            this.přehrátToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.přehrátToolStripMenuItem.Text = "Přehrá&t hru";
            this.přehrátToolStripMenuItem.Click += new System.EventHandler(this.přehrátToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // Platno
            // 
            this.Platno.BackColor = System.Drawing.Color.Silver;
            this.Platno.Location = new System.Drawing.Point(0, 27);
            this.Platno.Name = "Platno";
            this.Platno.Size = new System.Drawing.Size(535, 535);
            this.Platno.TabIndex = 1;
            this.Platno.TabStop = false;
            this.Platno.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Platno_MouseMove);
            this.Platno.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Platno_MouseClick);
            this.Platno.Paint += new System.Windows.Forms.PaintEventHandler(this.Platno_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(537, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Na řadě:";
            // 
            // NaRadeLabel
            // 
            this.NaRadeLabel.AutoSize = true;
            this.NaRadeLabel.Location = new System.Drawing.Point(592, 27);
            this.NaRadeLabel.Name = "NaRadeLabel";
            this.NaRadeLabel.Size = new System.Drawing.Size(0, 13);
            this.NaRadeLabel.TabIndex = 3;
            // 
            // PocitaciHraj
            // 
            this.PocitaciHraj.Enabled = false;
            this.PocitaciHraj.Location = new System.Drawing.Point(540, 472);
            this.PocitaciHraj.Name = "PocitaciHraj";
            this.PocitaciHraj.Size = new System.Drawing.Size(114, 23);
            this.PocitaciHraj.TabIndex = 4;
            this.PocitaciHraj.Text = "Počítači hraj!";
            this.PocitaciHraj.UseVisualStyleBackColor = true;
            this.PocitaciHraj.Click += new System.EventHandler(this.PocitaciHraj_Click);
            // 
            // Historie
            // 
            this.Historie.FormattingEnabled = true;
            this.Historie.Location = new System.Drawing.Point(540, 43);
            this.Historie.Name = "Historie";
            this.Historie.Size = new System.Drawing.Size(190, 394);
            this.Historie.TabIndex = 5;
            this.Historie.SelectedIndexChanged += new System.EventHandler(this.Historie_SelectedIndexChanged);
            // 
            // Pausa
            // 
            this.Pausa.Enabled = false;
            this.Pausa.Location = new System.Drawing.Point(540, 443);
            this.Pausa.Name = "Pausa";
            this.Pausa.Size = new System.Drawing.Size(114, 23);
            this.Pausa.TabIndex = 6;
            this.Pausa.Text = "Zastav Přehrávání";
            this.Pausa.UseVisualStyleBackColor = true;
            this.Pausa.Click += new System.EventHandler(this.Pausa_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(744, 584);
            this.Controls.Add(this.Pausa);
            this.Controls.Add(this.Historie);
            this.Controls.Add(this.PocitaciHraj);
            this.Controls.Add(this.NaRadeLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Platno);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Obrana Pevnosti";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.Platno)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem obranaPevnostiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nováHraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uložitHruToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nahrátHruToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem obtížnostToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem snadnáToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem normálníToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem těžkáToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem konecToolStripMenuItem;
        private System.Windows.Forms.PictureBox Platno;
        private System.Windows.Forms.ToolStripMenuItem restartHryToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label NaRadeLabel;
        private System.Windows.Forms.ToolStripMenuItem možnostiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zpětToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vpředToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nejlepšíTahToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.Button PocitaciHraj;
        private System.Windows.Forms.ToolStripMenuItem prohoďHráčeToolStripMenuItem;
        private System.Windows.Forms.ListBox Historie;
        private System.Windows.Forms.ToolStripMenuItem přehrátToolStripMenuItem;
        private System.Windows.Forms.Button Pausa;

    }
}


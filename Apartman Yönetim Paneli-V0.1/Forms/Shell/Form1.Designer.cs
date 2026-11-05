namespace Apartman_Yönetim_Paneli_V0._1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tableAna = new TableLayoutPanel();
            pnlSol = new Panel();
            btnCikis = new Button();
            btnPersonel = new Button();
            lblRol = new Label();
            lblKullanici = new Label();
            lblSolAlt = new Label();
            lblSolBaslik = new Label();
            lblSolUst = new Label();
            pnlSag = new Panel();
            tableIcerik = new TableLayoutPanel();
            lblBaslik = new Label();
            lblAltBaslik = new Label();
            pnlKart = new Panel();
            lblKartIcerik = new Label();
            tableAna.SuspendLayout();
            pnlSol.SuspendLayout();
            pnlSag.SuspendLayout();
            tableIcerik.SuspendLayout();
            pnlKart.SuspendLayout();
            SuspendLayout();
            // 
            // tableAna
            // 
            tableAna.BackColor = Color.FromArgb(31, 34, 43);
            tableAna.ColumnCount = 2;
            tableAna.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26F));
            tableAna.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 74F));
            tableAna.Controls.Add(pnlSol, 0, 0);
            tableAna.Controls.Add(pnlSag, 1, 0);
            tableAna.Dock = DockStyle.Fill;
            tableAna.Location = new Point(0, 0);
            tableAna.Name = "tableAna";
            tableAna.RowCount = 1;
            tableAna.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableAna.Size = new Size(1100, 700);
            tableAna.TabIndex = 0;
            // 
            // pnlSol
            // 
            pnlSol.BackColor = Color.FromArgb(42, 46, 61);
            pnlSol.Controls.Add(btnCikis);
            pnlSol.Controls.Add(btnPersonel);
            pnlSol.Controls.Add(lblRol);
            pnlSol.Controls.Add(lblKullanici);
            pnlSol.Controls.Add(lblSolAlt);
            pnlSol.Controls.Add(lblSolBaslik);
            pnlSol.Controls.Add(lblSolUst);
            pnlSol.Dock = DockStyle.Fill;
            pnlSol.Location = new Point(0, 0);
            pnlSol.Margin = new Padding(0);
            pnlSol.Name = "pnlSol";
            pnlSol.Padding = new Padding(28);
            pnlSol.Size = new Size(286, 700);
            pnlSol.TabIndex = 0;
            // 
            // lblSolUst
            // 
            lblSolUst.AutoSize = true;
            lblSolUst.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSolUst.ForeColor = Color.FromArgb(197, 225, 29);
            lblSolUst.Location = new Point(28, 40);
            lblSolUst.Name = "lblSolUst";
            lblSolUst.Size = new Size(140, 15);
            lblSolUst.TabIndex = 0;
            lblSolUst.Text = "APARTMAN YÖNETİMİ";
            // 
            // lblSolBaslik
            // 
            lblSolBaslik.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblSolBaslik.ForeColor = Color.FromArgb(236, 237, 232);
            lblSolBaslik.Location = new Point(28, 64);
            lblSolBaslik.Name = "lblSolBaslik";
            lblSolBaslik.Size = new Size(220, 80);
            lblSolBaslik.TabIndex = 1;
            lblSolBaslik.Text = "Kontrol Paneli";
            // 
            // lblSolAlt
            // 
            lblSolAlt.Font = new Font("Segoe UI", 9.5F);
            lblSolAlt.ForeColor = Color.FromArgb(154, 160, 176);
            lblSolAlt.Location = new Point(28, 150);
            lblSolAlt.Name = "lblSolAlt";
            lblSolAlt.Size = new Size(220, 90);
            lblSolAlt.TabIndex = 2;
            lblSolAlt.Text = "Rolünüze göre işlemlerinizi bu ekrandan yöneteceksiniz.";
            // 
            // lblKullanici
            // 
            lblKullanici.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblKullanici.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblKullanici.ForeColor = Color.FromArgb(236, 237, 232);
            lblKullanici.Location = new Point(28, 500);
            lblKullanici.Name = "lblKullanici";
            lblKullanici.Size = new Size(220, 28);
            lblKullanici.TabIndex = 3;
            lblKullanici.Text = "Kullanıcı";
            // 
            // lblRol
            // 
            lblRol.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblRol.Font = new Font("Segoe UI", 9F);
            lblRol.ForeColor = Color.FromArgb(197, 225, 29);
            lblRol.Location = new Point(28, 528);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(220, 28);
            lblRol.TabIndex = 4;
            lblRol.Text = "Rol";
            // 
            // btnPersonel
            // 
            btnPersonel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnPersonel.BackColor = Color.FromArgb(197, 225, 29);
            btnPersonel.Cursor = Cursors.Hand;
            btnPersonel.FlatAppearance.BorderSize = 0;
            btnPersonel.FlatStyle = FlatStyle.Flat;
            btnPersonel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnPersonel.ForeColor = Color.FromArgb(31, 34, 43);
            btnPersonel.Location = new Point(28, 572);
            btnPersonel.Name = "btnPersonel";
            btnPersonel.Size = new Size(220, 42);
            btnPersonel.TabIndex = 5;
            btnPersonel.Text = "Personel Ata";
            btnPersonel.UseVisualStyleBackColor = false;
            btnPersonel.Visible = false;
            btnPersonel.Click += btnPersonel_Click;
            // 
            // btnCikis
            // 
            btnCikis.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnCikis.BackColor = Color.FromArgb(50, 55, 72);
            btnCikis.Cursor = Cursors.Hand;
            btnCikis.FlatAppearance.BorderColor = Color.FromArgb(61, 66, 84);
            btnCikis.FlatStyle = FlatStyle.Flat;
            btnCikis.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnCikis.ForeColor = Color.FromArgb(236, 237, 232);
            btnCikis.Location = new Point(28, 630);
            btnCikis.Name = "btnCikis";
            btnCikis.Size = new Size(220, 40);
            btnCikis.TabIndex = 6;
            btnCikis.Text = "Çıkış Yap";
            btnCikis.UseVisualStyleBackColor = false;
            btnCikis.Click += btnCikis_Click;
            // 
            // pnlSag
            // 
            pnlSag.BackColor = Color.FromArgb(31, 34, 43);
            pnlSag.Controls.Add(tableIcerik);
            pnlSag.Dock = DockStyle.Fill;
            pnlSag.Location = new Point(286, 0);
            pnlSag.Margin = new Padding(0);
            pnlSag.Name = "pnlSag";
            pnlSag.Padding = new Padding(32);
            pnlSag.Size = new Size(814, 700);
            pnlSag.TabIndex = 1;
            // 
            // tableIcerik
            // 
            tableIcerik.ColumnCount = 1;
            tableIcerik.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableIcerik.Controls.Add(lblBaslik, 0, 0);
            tableIcerik.Controls.Add(lblAltBaslik, 0, 1);
            tableIcerik.Controls.Add(pnlKart, 0, 2);
            tableIcerik.Dock = DockStyle.Fill;
            tableIcerik.Location = new Point(32, 32);
            tableIcerik.Name = "tableIcerik";
            tableIcerik.RowCount = 4;
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Absolute, 180F));
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableIcerik.Size = new Size(750, 636);
            tableIcerik.TabIndex = 0;
            // 
            // lblBaslik
            // 
            lblBaslik.AutoSize = true;
            lblBaslik.Dock = DockStyle.Bottom;
            lblBaslik.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            lblBaslik.ForeColor = Color.FromArgb(236, 237, 232);
            lblBaslik.Name = "lblBaslik";
            lblBaslik.TabIndex = 0;
            lblBaslik.Text = "Hoş geldiniz";
            // 
            // lblAltBaslik
            // 
            lblAltBaslik.AutoSize = true;
            lblAltBaslik.Dock = DockStyle.Top;
            lblAltBaslik.Font = new Font("Segoe UI", 10F);
            lblAltBaslik.ForeColor = Color.FromArgb(154, 160, 176);
            lblAltBaslik.Name = "lblAltBaslik";
            lblAltBaslik.TabIndex = 1;
            lblAltBaslik.Text = "Oturumunuz açıldı.";
            // 
            // pnlKart
            // 
            pnlKart.BackColor = Color.FromArgb(50, 55, 72);
            pnlKart.Controls.Add(lblKartIcerik);
            pnlKart.Dock = DockStyle.Fill;
            pnlKart.Location = new Point(0, 87);
            pnlKart.Margin = new Padding(0, 3, 0, 3);
            pnlKart.Name = "pnlKart";
            pnlKart.Padding = new Padding(24);
            pnlKart.Size = new Size(750, 174);
            pnlKart.TabIndex = 2;
            // 
            // lblKartIcerik
            // 
            lblKartIcerik.Dock = DockStyle.Fill;
            lblKartIcerik.Font = new Font("Segoe UI", 11F);
            lblKartIcerik.ForeColor = Color.FromArgb(236, 237, 232);
            lblKartIcerik.Name = "lblKartIcerik";
            lblKartIcerik.TabIndex = 0;
            lblKartIcerik.Text = "Apartman işlemleri bu panel üzerinden yönetilecek.";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(31, 34, 43);
            ClientSize = new Size(1100, 700);
            Controls.Add(tableAna);
            MinimumSize = new Size(920, 580);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Apartman Yönetim Paneli";
            Load += Form1_Load;
            tableAna.ResumeLayout(false);
            pnlSol.ResumeLayout(false);
            pnlSol.PerformLayout();
            pnlSag.ResumeLayout(false);
            tableIcerik.ResumeLayout(false);
            tableIcerik.PerformLayout();
            pnlKart.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableAna;
        private Panel pnlSol;
        private Button btnCikis;
        private Button btnPersonel;
        private Label lblRol;
        private Label lblKullanici;
        private Label lblSolAlt;
        private Label lblSolBaslik;
        private Label lblSolUst;
        private Panel pnlSag;
        private TableLayoutPanel tableIcerik;
        private Label lblBaslik;
        private Label lblAltBaslik;
        private Panel pnlKart;
        private Label lblKartIcerik;
    }
}

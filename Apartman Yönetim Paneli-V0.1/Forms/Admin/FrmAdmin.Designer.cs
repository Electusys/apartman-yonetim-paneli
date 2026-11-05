namespace Apartman_Yönetim_Paneli_V0._1
{
    partial class FrmAdmin
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
            lblKullanici = new Label();
            lblSolAlt = new Label();
            lblSolBaslik = new Label();
            lblSolUst = new Label();
            pnlSag = new Panel();
            tableIcerik = new TableLayoutPanel();
            lblBaslik = new Label();
            lblAltBaslik = new Label();
            pnlKart = new Panel();
            tableForm = new TableLayoutPanel();
            tableAdSoyad = new TableLayoutPanel();
            lblAd = new Label();
            txtAd = new TextBox();
            lblSoyad = new Label();
            txtSoyad = new TextBox();
            lblKullaniciAdi = new Label();
            txtKullaniciAdi = new TextBox();
            tableSifre = new TableLayoutPanel();
            lblSifre = new Label();
            txtSifre = new TextBox();
            lblSifreTekrar = new Label();
            txtSifreTekrar = new TextBox();
            btnYoneticiOlustur = new Button();
            lblListeBaslik = new Label();
            dgvYoneticiler = new DataGridView();
            tableAna.SuspendLayout();
            pnlSol.SuspendLayout();
            pnlSag.SuspendLayout();
            tableIcerik.SuspendLayout();
            pnlKart.SuspendLayout();
            tableForm.SuspendLayout();
            tableAdSoyad.SuspendLayout();
            tableSifre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvYoneticiler).BeginInit();
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
            btnCikis.TabIndex = 4;
            btnCikis.Text = "Çıkış Yap";
            btnCikis.UseVisualStyleBackColor = false;
            btnCikis.Click += btnCikis_Click;
            // 
            // lblKullanici
            // 
            lblKullanici.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblKullanici.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblKullanici.ForeColor = Color.FromArgb(236, 237, 232);
            lblKullanici.Location = new Point(28, 580);
            lblKullanici.Name = "lblKullanici";
            lblKullanici.Size = new Size(220, 40);
            lblKullanici.TabIndex = 3;
            lblKullanici.Text = "Admin";
            // 
            // lblSolAlt
            // 
            lblSolAlt.Font = new Font("Segoe UI", 9.5F);
            lblSolAlt.ForeColor = Color.FromArgb(154, 160, 176);
            lblSolAlt.Location = new Point(28, 150);
            lblSolAlt.Name = "lblSolAlt";
            lblSolAlt.Size = new Size(220, 90);
            lblSolAlt.TabIndex = 2;
            lblSolAlt.Text = "Yönetici rolünü yalnızca Admin atar. Muhasebeci ve teknisyen ataması yönetici panelinden yapılır.";
            // 
            // lblSolBaslik
            // 
            lblSolBaslik.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblSolBaslik.ForeColor = Color.FromArgb(236, 237, 232);
            lblSolBaslik.Location = new Point(28, 64);
            lblSolBaslik.Name = "lblSolBaslik";
            lblSolBaslik.Size = new Size(220, 80);
            lblSolBaslik.TabIndex = 1;
            lblSolBaslik.Text = "Admin Paneli";
            // 
            // lblSolUst
            // 
            lblSolUst.AutoSize = true;
            lblSolUst.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSolUst.ForeColor = Color.FromArgb(197, 225, 29);
            lblSolUst.Location = new Point(28, 40);
            lblSolUst.Name = "lblSolUst";
            lblSolUst.Size = new Size(93, 15);
            lblSolUst.TabIndex = 0;
            lblSolUst.Text = "SİSTEM ADMIN";
            // 
            // pnlSag
            // 
            pnlSag.BackColor = Color.FromArgb(31, 34, 43);
            pnlSag.Controls.Add(tableIcerik);
            pnlSag.Dock = DockStyle.Fill;
            pnlSag.Location = new Point(286, 0);
            pnlSag.Margin = new Padding(0);
            pnlSag.Name = "pnlSag";
            pnlSag.Padding = new Padding(28);
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
            tableIcerik.Controls.Add(lblListeBaslik, 0, 3);
            tableIcerik.Controls.Add(dgvYoneticiler, 0, 4);
            tableIcerik.Dock = DockStyle.Fill;
            tableIcerik.Location = new Point(28, 28);
            tableIcerik.Name = "tableIcerik";
            tableIcerik.RowCount = 5;
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Absolute, 250F));
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableIcerik.Size = new Size(758, 644);
            tableIcerik.TabIndex = 0;
            // 
            // lblBaslik
            // 
            lblBaslik.AutoSize = true;
            lblBaslik.Dock = DockStyle.Bottom;
            lblBaslik.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblBaslik.ForeColor = Color.FromArgb(236, 237, 232);
            lblBaslik.Location = new Point(3, 5);
            lblBaslik.Name = "lblBaslik";
            lblBaslik.Size = new Size(752, 37);
            lblBaslik.TabIndex = 0;
            lblBaslik.Text = "Apartman Yöneticisi Oluştur";
            // 
            // lblAltBaslik
            // 
            lblAltBaslik.AutoSize = true;
            lblAltBaslik.Dock = DockStyle.Top;
            lblAltBaslik.Font = new Font("Segoe UI", 10F);
            lblAltBaslik.ForeColor = Color.FromArgb(154, 160, 176);
            lblAltBaslik.Location = new Point(3, 42);
            lblAltBaslik.Name = "lblAltBaslik";
            lblAltBaslik.Size = new Size(752, 19);
            lblAltBaslik.TabIndex = 1;
            lblAltBaslik.Text = "Yeni yönetici hesabı açın. Bu hesap sakin kayıtlarından bağımsızdır.";
            // 
            // pnlKart
            // 
            pnlKart.BackColor = Color.FromArgb(50, 55, 72);
            pnlKart.Controls.Add(tableForm);
            pnlKart.Dock = DockStyle.Fill;
            pnlKart.Location = new Point(0, 73);
            pnlKart.Margin = new Padding(0, 3, 0, 3);
            pnlKart.Name = "pnlKart";
            pnlKart.Padding = new Padding(16);
            pnlKart.Size = new Size(758, 244);
            pnlKart.TabIndex = 2;
            // 
            // tableForm
            // 
            tableForm.ColumnCount = 1;
            tableForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableForm.Controls.Add(tableAdSoyad, 0, 0);
            tableForm.Controls.Add(lblKullaniciAdi, 0, 1);
            tableForm.Controls.Add(txtKullaniciAdi, 0, 2);
            tableForm.Controls.Add(tableSifre, 0, 3);
            tableForm.Controls.Add(btnYoneticiOlustur, 0, 4);
            tableForm.Dock = DockStyle.Fill;
            tableForm.Location = new Point(16, 16);
            tableForm.Name = "tableForm";
            tableForm.RowCount = 5;
            tableForm.RowStyles.Add(new RowStyle(SizeType.Percent, 34F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Percent, 34F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Percent, 32F));
            tableForm.Size = new Size(726, 212);
            tableForm.TabIndex = 0;
            // 
            // tableAdSoyad
            // 
            tableAdSoyad.ColumnCount = 2;
            tableAdSoyad.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableAdSoyad.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableAdSoyad.Controls.Add(lblAd, 0, 0);
            tableAdSoyad.Controls.Add(txtAd, 0, 1);
            tableAdSoyad.Controls.Add(lblSoyad, 1, 0);
            tableAdSoyad.Controls.Add(txtSoyad, 1, 1);
            tableAdSoyad.Dock = DockStyle.Fill;
            tableAdSoyad.Location = new Point(0, 0);
            tableAdSoyad.Margin = new Padding(0);
            tableAdSoyad.Name = "tableAdSoyad";
            tableAdSoyad.RowCount = 2;
            tableAdSoyad.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableAdSoyad.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableAdSoyad.Size = new Size(726, 53);
            tableAdSoyad.TabIndex = 0;
            // 
            // lblAd
            // 
            lblAd.AutoSize = true;
            lblAd.Dock = DockStyle.Fill;
            lblAd.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAd.ForeColor = Color.FromArgb(154, 160, 176);
            lblAd.Location = new Point(3, 0);
            lblAd.Name = "lblAd";
            lblAd.Size = new Size(357, 22);
            lblAd.TabIndex = 0;
            lblAd.Text = "Ad";
            lblAd.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtAd
            // 
            txtAd.BackColor = Color.FromArgb(42, 46, 61);
            txtAd.BorderStyle = BorderStyle.FixedSingle;
            txtAd.Dock = DockStyle.Fill;
            txtAd.Font = new Font("Segoe UI", 11F);
            txtAd.ForeColor = Color.FromArgb(236, 237, 232);
            txtAd.Location = new Point(0, 22);
            txtAd.Margin = new Padding(0, 0, 8, 0);
            txtAd.Name = "txtAd";
            txtAd.Size = new Size(355, 27);
            txtAd.TabIndex = 1;
            // 
            // lblSoyad
            // 
            lblSoyad.AutoSize = true;
            lblSoyad.Dock = DockStyle.Fill;
            lblSoyad.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSoyad.ForeColor = Color.FromArgb(154, 160, 176);
            lblSoyad.Location = new Point(366, 0);
            lblSoyad.Name = "lblSoyad";
            lblSoyad.Size = new Size(357, 22);
            lblSoyad.TabIndex = 2;
            lblSoyad.Text = "Soyad";
            lblSoyad.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtSoyad
            // 
            txtSoyad.BackColor = Color.FromArgb(42, 46, 61);
            txtSoyad.BorderStyle = BorderStyle.FixedSingle;
            txtSoyad.Dock = DockStyle.Fill;
            txtSoyad.Font = new Font("Segoe UI", 11F);
            txtSoyad.ForeColor = Color.FromArgb(236, 237, 232);
            txtSoyad.Location = new Point(371, 22);
            txtSoyad.Margin = new Padding(8, 0, 0, 0);
            txtSoyad.Name = "txtSoyad";
            txtSoyad.Size = new Size(355, 27);
            txtSoyad.TabIndex = 3;
            // 
            // lblKullaniciAdi
            // 
            lblKullaniciAdi.AutoSize = true;
            lblKullaniciAdi.Dock = DockStyle.Fill;
            lblKullaniciAdi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblKullaniciAdi.ForeColor = Color.FromArgb(154, 160, 176);
            lblKullaniciAdi.Location = new Point(3, 53);
            lblKullaniciAdi.Name = "lblKullaniciAdi";
            lblKullaniciAdi.Size = new Size(720, 22);
            lblKullaniciAdi.TabIndex = 1;
            lblKullaniciAdi.Text = "Kullanıcı Adı";
            lblKullaniciAdi.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtKullaniciAdi
            // 
            txtKullaniciAdi.BackColor = Color.FromArgb(42, 46, 61);
            txtKullaniciAdi.BorderStyle = BorderStyle.FixedSingle;
            txtKullaniciAdi.Dock = DockStyle.Fill;
            txtKullaniciAdi.Font = new Font("Segoe UI", 11F);
            txtKullaniciAdi.ForeColor = Color.FromArgb(236, 237, 232);
            txtKullaniciAdi.Location = new Point(3, 78);
            txtKullaniciAdi.Name = "txtKullaniciAdi";
            txtKullaniciAdi.Size = new Size(720, 27);
            txtKullaniciAdi.TabIndex = 2;
            // 
            // tableSifre
            // 
            tableSifre.ColumnCount = 2;
            tableSifre.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableSifre.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableSifre.Controls.Add(lblSifre, 0, 0);
            tableSifre.Controls.Add(txtSifre, 0, 1);
            tableSifre.Controls.Add(lblSifreTekrar, 1, 0);
            tableSifre.Controls.Add(txtSifreTekrar, 1, 1);
            tableSifre.Dock = DockStyle.Fill;
            tableSifre.Location = new Point(0, 109);
            tableSifre.Margin = new Padding(0);
            tableSifre.Name = "tableSifre";
            tableSifre.RowCount = 2;
            tableSifre.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableSifre.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableSifre.Size = new Size(726, 53);
            tableSifre.TabIndex = 3;
            // 
            // lblSifre
            // 
            lblSifre.AutoSize = true;
            lblSifre.Dock = DockStyle.Fill;
            lblSifre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSifre.ForeColor = Color.FromArgb(154, 160, 176);
            lblSifre.Location = new Point(3, 0);
            lblSifre.Name = "lblSifre";
            lblSifre.Size = new Size(357, 22);
            lblSifre.TabIndex = 0;
            lblSifre.Text = "Şifre";
            lblSifre.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtSifre
            // 
            txtSifre.BackColor = Color.FromArgb(42, 46, 61);
            txtSifre.BorderStyle = BorderStyle.FixedSingle;
            txtSifre.Dock = DockStyle.Fill;
            txtSifre.Font = new Font("Segoe UI", 11F);
            txtSifre.ForeColor = Color.FromArgb(236, 237, 232);
            txtSifre.Location = new Point(0, 22);
            txtSifre.Margin = new Padding(0, 0, 8, 0);
            txtSifre.Name = "txtSifre";
            txtSifre.Size = new Size(355, 27);
            txtSifre.TabIndex = 1;
            txtSifre.UseSystemPasswordChar = true;
            // 
            // lblSifreTekrar
            // 
            lblSifreTekrar.AutoSize = true;
            lblSifreTekrar.Dock = DockStyle.Fill;
            lblSifreTekrar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSifreTekrar.ForeColor = Color.FromArgb(154, 160, 176);
            lblSifreTekrar.Location = new Point(366, 0);
            lblSifreTekrar.Name = "lblSifreTekrar";
            lblSifreTekrar.Size = new Size(357, 22);
            lblSifreTekrar.TabIndex = 2;
            lblSifreTekrar.Text = "Şifre Tekrar";
            lblSifreTekrar.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtSifreTekrar
            // 
            txtSifreTekrar.BackColor = Color.FromArgb(42, 46, 61);
            txtSifreTekrar.BorderStyle = BorderStyle.FixedSingle;
            txtSifreTekrar.Dock = DockStyle.Fill;
            txtSifreTekrar.Font = new Font("Segoe UI", 11F);
            txtSifreTekrar.ForeColor = Color.FromArgb(236, 237, 232);
            txtSifreTekrar.Location = new Point(371, 22);
            txtSifreTekrar.Margin = new Padding(8, 0, 0, 0);
            txtSifreTekrar.Name = "txtSifreTekrar";
            txtSifreTekrar.Size = new Size(355, 27);
            txtSifreTekrar.TabIndex = 3;
            txtSifreTekrar.UseSystemPasswordChar = true;
            // 
            // btnYoneticiOlustur
            // 
            btnYoneticiOlustur.BackColor = Color.FromArgb(197, 225, 29);
            btnYoneticiOlustur.Cursor = Cursors.Hand;
            btnYoneticiOlustur.Dock = DockStyle.Fill;
            btnYoneticiOlustur.FlatAppearance.BorderSize = 0;
            btnYoneticiOlustur.FlatStyle = FlatStyle.Flat;
            btnYoneticiOlustur.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnYoneticiOlustur.ForeColor = Color.FromArgb(31, 34, 43);
            btnYoneticiOlustur.Location = new Point(0, 170);
            btnYoneticiOlustur.Margin = new Padding(0, 8, 0, 0);
            btnYoneticiOlustur.Name = "btnYoneticiOlustur";
            btnYoneticiOlustur.Size = new Size(726, 42);
            btnYoneticiOlustur.TabIndex = 4;
            btnYoneticiOlustur.Text = "Yönetici Oluştur";
            btnYoneticiOlustur.UseVisualStyleBackColor = false;
            btnYoneticiOlustur.Click += btnYoneticiOlustur_Click;
            // 
            // lblListeBaslik
            // 
            lblListeBaslik.AutoSize = true;
            lblListeBaslik.Dock = DockStyle.Fill;
            lblListeBaslik.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblListeBaslik.ForeColor = Color.FromArgb(236, 237, 232);
            lblListeBaslik.Location = new Point(3, 320);
            lblListeBaslik.Name = "lblListeBaslik";
            lblListeBaslik.Size = new Size(752, 36);
            lblListeBaslik.TabIndex = 3;
            lblListeBaslik.Text = "Mevcut apartman yöneticileri";
            lblListeBaslik.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvYoneticiler
            // 
            dgvYoneticiler.BackgroundColor = Color.FromArgb(50, 55, 72);
            dgvYoneticiler.BorderStyle = BorderStyle.None;
            dgvYoneticiler.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvYoneticiler.Dock = DockStyle.Fill;
            dgvYoneticiler.Location = new Point(3, 359);
            dgvYoneticiler.Name = "dgvYoneticiler";
            dgvYoneticiler.RowHeadersVisible = false;
            dgvYoneticiler.Size = new Size(752, 282);
            dgvYoneticiler.TabIndex = 4;
            // 
            // FrmAdmin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(31, 34, 43);
            ClientSize = new Size(1100, 700);
            Controls.Add(tableAna);
            MinimumSize = new Size(980, 640);
            Name = "FrmAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Admin Paneli - Apartman Yönetim";
            Load += FrmAdmin_Load;
            tableAna.ResumeLayout(false);
            pnlSol.ResumeLayout(false);
            pnlSol.PerformLayout();
            pnlSag.ResumeLayout(false);
            tableIcerik.ResumeLayout(false);
            tableIcerik.PerformLayout();
            pnlKart.ResumeLayout(false);
            tableForm.ResumeLayout(false);
            tableForm.PerformLayout();
            tableAdSoyad.ResumeLayout(false);
            tableAdSoyad.PerformLayout();
            tableSifre.ResumeLayout(false);
            tableSifre.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvYoneticiler).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableAna;
        private Panel pnlSol;
        private Button btnCikis;
        private Label lblKullanici;
        private Label lblSolAlt;
        private Label lblSolBaslik;
        private Label lblSolUst;
        private Panel pnlSag;
        private TableLayoutPanel tableIcerik;
        private Label lblBaslik;
        private Label lblAltBaslik;
        private Panel pnlKart;
        private TableLayoutPanel tableForm;
        private TableLayoutPanel tableAdSoyad;
        private Label lblAd;
        private TextBox txtAd;
        private Label lblSoyad;
        private TextBox txtSoyad;
        private Label lblKullaniciAdi;
        private TextBox txtKullaniciAdi;
        private TableLayoutPanel tableSifre;
        private Label lblSifre;
        private TextBox txtSifre;
        private Label lblSifreTekrar;
        private TextBox txtSifreTekrar;
        private Button btnYoneticiOlustur;
        private Label lblListeBaslik;
        private DataGridView dgvYoneticiler;
    }
}

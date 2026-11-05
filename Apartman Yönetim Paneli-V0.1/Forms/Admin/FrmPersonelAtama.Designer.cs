namespace Apartman_Yönetim_Paneli_V0._1
{
    partial class FrmPersonelAtama
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
            btnGeri = new Button();
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
            lblRol = new Label();
            cmbRol = new ComboBox();
            btnPersonelOlustur = new Button();
            lblListeBaslik = new Label();
            dgvPersonel = new DataGridView();
            tableAna.SuspendLayout();
            pnlSol.SuspendLayout();
            pnlSag.SuspendLayout();
            tableIcerik.SuspendLayout();
            pnlKart.SuspendLayout();
            tableForm.SuspendLayout();
            tableAdSoyad.SuspendLayout();
            tableSifre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPersonel).BeginInit();
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
            tableAna.Size = new Size(1100, 720);
            tableAna.TabIndex = 0;
            // 
            // pnlSol
            // 
            pnlSol.BackColor = Color.FromArgb(42, 46, 61);
            pnlSol.Controls.Add(btnGeri);
            pnlSol.Controls.Add(lblKullanici);
            pnlSol.Controls.Add(lblSolAlt);
            pnlSol.Controls.Add(lblSolBaslik);
            pnlSol.Controls.Add(lblSolUst);
            pnlSol.Dock = DockStyle.Fill;
            pnlSol.Location = new Point(0, 0);
            pnlSol.Margin = new Padding(0);
            pnlSol.Name = "pnlSol";
            pnlSol.Padding = new Padding(28);
            pnlSol.Size = new Size(286, 720);
            pnlSol.TabIndex = 0;
            // 
            // btnGeri
            // 
            btnGeri.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnGeri.BackColor = Color.FromArgb(50, 55, 72);
            btnGeri.Cursor = Cursors.Hand;
            btnGeri.FlatAppearance.BorderColor = Color.FromArgb(61, 66, 84);
            btnGeri.FlatStyle = FlatStyle.Flat;
            btnGeri.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnGeri.ForeColor = Color.FromArgb(236, 237, 232);
            btnGeri.Location = new Point(28, 650);
            btnGeri.Name = "btnGeri";
            btnGeri.Size = new Size(220, 40);
            btnGeri.TabIndex = 4;
            btnGeri.Text = "Panele Dön";
            btnGeri.UseVisualStyleBackColor = false;
            btnGeri.Click += btnGeri_Click;
            // 
            // lblKullanici
            // 
            lblKullanici.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblKullanici.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblKullanici.ForeColor = Color.FromArgb(236, 237, 232);
            lblKullanici.Location = new Point(28, 600);
            lblKullanici.Name = "lblKullanici";
            lblKullanici.Size = new Size(220, 40);
            lblKullanici.TabIndex = 3;
            lblKullanici.Text = "Yönetici";
            // 
            // lblSolAlt
            // 
            lblSolAlt.Font = new Font("Segoe UI", 9.5F);
            lblSolAlt.ForeColor = Color.FromArgb(154, 160, 176);
            lblSolAlt.Location = new Point(28, 160);
            lblSolAlt.Name = "lblSolAlt";
            lblSolAlt.Size = new Size(220, 110);
            lblSolAlt.TabIndex = 2;
            lblSolAlt.Text = "Muhasebeci ve teknisyen hesapları yalnızca apartman yöneticisi tarafından açılır.";
            // 
            // lblSolBaslik
            // 
            lblSolBaslik.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblSolBaslik.ForeColor = Color.FromArgb(236, 237, 232);
            lblSolBaslik.Location = new Point(28, 64);
            lblSolBaslik.Name = "lblSolBaslik";
            lblSolBaslik.Size = new Size(220, 90);
            lblSolBaslik.TabIndex = 1;
            lblSolBaslik.Text = "Personel Atama";
            // 
            // lblSolUst
            // 
            lblSolUst.AutoSize = true;
            lblSolUst.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSolUst.ForeColor = Color.FromArgb(197, 225, 29);
            lblSolUst.Location = new Point(28, 40);
            lblSolUst.Name = "lblSolUst";
            lblSolUst.Size = new Size(60, 15);
            lblSolUst.TabIndex = 0;
            lblSolUst.Text = "YÖNETİCİ";
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
            pnlSag.Size = new Size(814, 720);
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
            tableIcerik.Controls.Add(dgvPersonel, 0, 4);
            tableIcerik.Dock = DockStyle.Fill;
            tableIcerik.Location = new Point(28, 28);
            tableIcerik.Name = "tableIcerik";
            tableIcerik.RowCount = 5;
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Absolute, 300F));
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableIcerik.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableIcerik.Size = new Size(758, 664);
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
            lblBaslik.Text = "Muhasebeci / Teknisyen Oluştur";
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
            lblAltBaslik.Text = "Rol olarak yalnızca Muhasebeci veya Teknisyen seçilebilir.";
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
            pnlKart.Size = new Size(758, 294);
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
            tableForm.Controls.Add(lblRol, 0, 4);
            tableForm.Controls.Add(cmbRol, 0, 5);
            tableForm.Controls.Add(btnPersonelOlustur, 0, 6);
            tableForm.Dock = DockStyle.Fill;
            tableForm.Location = new Point(16, 16);
            tableForm.Name = "tableForm";
            tableForm.RowCount = 7;
            tableForm.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Percent, 44F));
            tableForm.Size = new Size(726, 262);
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
            tableAdSoyad.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableAdSoyad.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableAdSoyad.Size = new Size(726, 44);
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
            lblAd.Size = new Size(357, 20);
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
            txtAd.Location = new Point(0, 20);
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
            lblSoyad.Size = new Size(357, 20);
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
            txtSoyad.Location = new Point(371, 20);
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
            lblKullaniciAdi.Location = new Point(3, 44);
            lblKullaniciAdi.Name = "lblKullaniciAdi";
            lblKullaniciAdi.Size = new Size(720, 20);
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
            txtKullaniciAdi.Location = new Point(3, 67);
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
            tableSifre.Location = new Point(0, 96);
            tableSifre.Margin = new Padding(0);
            tableSifre.Name = "tableSifre";
            tableSifre.RowCount = 2;
            tableSifre.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableSifre.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableSifre.Size = new Size(726, 44);
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
            lblSifre.Size = new Size(357, 20);
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
            txtSifre.Location = new Point(0, 20);
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
            lblSifreTekrar.Size = new Size(357, 20);
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
            txtSifreTekrar.Location = new Point(371, 20);
            txtSifreTekrar.Margin = new Padding(8, 0, 0, 0);
            txtSifreTekrar.Name = "txtSifreTekrar";
            txtSifreTekrar.Size = new Size(355, 27);
            txtSifreTekrar.TabIndex = 3;
            txtSifreTekrar.UseSystemPasswordChar = true;
            // 
            // lblRol
            // 
            lblRol.AutoSize = true;
            lblRol.Dock = DockStyle.Fill;
            lblRol.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRol.ForeColor = Color.FromArgb(154, 160, 176);
            lblRol.Location = new Point(3, 140);
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(720, 20);
            lblRol.TabIndex = 4;
            lblRol.Text = "Rol";
            lblRol.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cmbRol
            // 
            cmbRol.BackColor = Color.FromArgb(42, 46, 61);
            cmbRol.Dock = DockStyle.Fill;
            cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRol.FlatStyle = FlatStyle.Flat;
            cmbRol.Font = new Font("Segoe UI", 11F);
            cmbRol.ForeColor = Color.FromArgb(236, 237, 232);
            cmbRol.FormattingEnabled = true;
            cmbRol.Location = new Point(3, 163);
            cmbRol.Name = "cmbRol";
            cmbRol.Size = new Size(720, 28);
            cmbRol.TabIndex = 5;
            // 
            // btnPersonelOlustur
            // 
            btnPersonelOlustur.BackColor = Color.FromArgb(197, 225, 29);
            btnPersonelOlustur.Cursor = Cursors.Hand;
            btnPersonelOlustur.Dock = DockStyle.Fill;
            btnPersonelOlustur.FlatAppearance.BorderSize = 0;
            btnPersonelOlustur.FlatStyle = FlatStyle.Flat;
            btnPersonelOlustur.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnPersonelOlustur.ForeColor = Color.FromArgb(31, 34, 43);
            btnPersonelOlustur.Location = new Point(0, 200);
            btnPersonelOlustur.Margin = new Padding(0, 8, 0, 0);
            btnPersonelOlustur.Name = "btnPersonelOlustur";
            btnPersonelOlustur.Size = new Size(726, 62);
            btnPersonelOlustur.TabIndex = 6;
            btnPersonelOlustur.Text = "Personel Oluştur";
            btnPersonelOlustur.UseVisualStyleBackColor = false;
            btnPersonelOlustur.Click += btnPersonelOlustur_Click;
            // 
            // lblListeBaslik
            // 
            lblListeBaslik.AutoSize = true;
            lblListeBaslik.Dock = DockStyle.Fill;
            lblListeBaslik.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            lblListeBaslik.ForeColor = Color.FromArgb(236, 237, 232);
            lblListeBaslik.Location = new Point(3, 370);
            lblListeBaslik.Name = "lblListeBaslik";
            lblListeBaslik.Size = new Size(752, 36);
            lblListeBaslik.TabIndex = 3;
            lblListeBaslik.Text = "Mevcut muhasebeci ve teknisyenler";
            lblListeBaslik.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvPersonel
            // 
            dgvPersonel.BackgroundColor = Color.FromArgb(50, 55, 72);
            dgvPersonel.BorderStyle = BorderStyle.None;
            dgvPersonel.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPersonel.Dock = DockStyle.Fill;
            dgvPersonel.Location = new Point(3, 409);
            dgvPersonel.Name = "dgvPersonel";
            dgvPersonel.RowHeadersVisible = false;
            dgvPersonel.Size = new Size(752, 252);
            dgvPersonel.TabIndex = 4;
            // 
            // FrmPersonelAtama
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(31, 34, 43);
            ClientSize = new Size(1100, 720);
            Controls.Add(tableAna);
            MinimumSize = new Size(980, 640);
            Name = "FrmPersonelAtama";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Personel Atama - Apartman Yönetim";
            Load += FrmPersonelAtama_Load;
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
            ((System.ComponentModel.ISupportInitialize)dgvPersonel).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableAna;
        private Panel pnlSol;
        private Button btnGeri;
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
        private Label lblRol;
        private ComboBox cmbRol;
        private Button btnPersonelOlustur;
        private Label lblListeBaslik;
        private DataGridView dgvPersonel;
    }
}

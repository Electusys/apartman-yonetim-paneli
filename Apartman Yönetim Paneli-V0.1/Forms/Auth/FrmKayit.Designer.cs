namespace Apartman_Yönetim_Paneli_V0._1
{
    partial class FrmKayit
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
            lblSolAlt = new Label();
            lblSolBaslik = new Label();
            lblSolUst = new Label();
            pnlSag = new Panel();
            tableForm = new TableLayoutPanel();
            lblBaslik = new Label();
            lblAltBaslik = new Label();
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
            lblRolBilgi = new Label();
            btnKayitOl = new Button();
            btnGirisDon = new Button();
            tableAna.SuspendLayout();
            pnlSol.SuspendLayout();
            pnlSag.SuspendLayout();
            tableForm.SuspendLayout();
            tableAdSoyad.SuspendLayout();
            tableSifre.SuspendLayout();
            SuspendLayout();
            // 
            // tableAna
            // 
            tableAna.BackColor = Color.FromArgb(31, 34, 43);
            tableAna.ColumnCount = 2;
            tableAna.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36F));
            tableAna.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64F));
            tableAna.Controls.Add(pnlSol, 0, 0);
            tableAna.Controls.Add(pnlSag, 1, 0);
            tableAna.Dock = DockStyle.Fill;
            tableAna.Location = new Point(0, 0);
            tableAna.Name = "tableAna";
            tableAna.RowCount = 1;
            tableAna.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableAna.Size = new Size(980, 640);
            tableAna.TabIndex = 0;
            // 
            // pnlSol
            // 
            pnlSol.BackColor = Color.FromArgb(42, 46, 61);
            pnlSol.Controls.Add(lblSolAlt);
            pnlSol.Controls.Add(lblSolBaslik);
            pnlSol.Controls.Add(lblSolUst);
            pnlSol.Dock = DockStyle.Fill;
            pnlSol.Location = new Point(0, 0);
            pnlSol.Margin = new Padding(0);
            pnlSol.Name = "pnlSol";
            pnlSol.Padding = new Padding(36);
            pnlSol.Size = new Size(352, 640);
            pnlSol.TabIndex = 0;
            // 
            // lblSolUst
            // 
            lblSolUst.AutoSize = true;
            lblSolUst.Font = new Font("Segoe UI", 10F);
            lblSolUst.ForeColor = Color.FromArgb(197, 225, 29);
            lblSolUst.Location = new Point(36, 180);
            lblSolUst.Name = "lblSolUst";
            lblSolUst.Size = new Size(180, 19);
            lblSolUst.TabIndex = 0;
            lblSolUst.Text = "YENİ SAKİN HESABI";
            // 
            // lblSolBaslik
            // 
            lblSolBaslik.Font = new Font("Segoe UI Semibold", 28F, FontStyle.Bold);
            lblSolBaslik.ForeColor = Color.FromArgb(236, 237, 232);
            lblSolBaslik.Location = new Point(34, 210);
            lblSolBaslik.Name = "lblSolBaslik";
            lblSolBaslik.Size = new Size(270, 90);
            lblSolBaslik.TabIndex = 1;
            lblSolBaslik.Text = "Kayıt Ol";
            // 
            // lblSolAlt
            // 
            lblSolAlt.Font = new Font("Segoe UI", 10F);
            lblSolAlt.ForeColor = Color.FromArgb(154, 160, 176);
            lblSolAlt.Location = new Point(36, 310);
            lblSolAlt.Name = "lblSolAlt";
            lblSolAlt.Size = new Size(260, 110);
            lblSolAlt.TabIndex = 2;
            lblSolAlt.Text = "Bu ekrandan yalnızca apartman sakini hesabı oluşturulur. Yönetici hesapları Admin panelinden açılır.";
            // 
            // pnlSag
            // 
            pnlSag.BackColor = Color.FromArgb(31, 34, 43);
            pnlSag.Controls.Add(tableForm);
            pnlSag.Dock = DockStyle.Fill;
            pnlSag.Location = new Point(352, 0);
            pnlSag.Margin = new Padding(0);
            pnlSag.Name = "pnlSag";
            pnlSag.Padding = new Padding(40);
            pnlSag.Size = new Size(628, 640);
            pnlSag.TabIndex = 1;
            // 
            // tableForm
            // 
            tableForm.ColumnCount = 1;
            tableForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableForm.Controls.Add(lblBaslik, 0, 0);
            tableForm.Controls.Add(lblAltBaslik, 0, 1);
            tableForm.Controls.Add(tableAdSoyad, 0, 2);
            tableForm.Controls.Add(lblKullaniciAdi, 0, 3);
            tableForm.Controls.Add(txtKullaniciAdi, 0, 4);
            tableForm.Controls.Add(tableSifre, 0, 5);
            tableForm.Controls.Add(lblRolBilgi, 0, 6);
            tableForm.Controls.Add(btnKayitOl, 0, 7);
            tableForm.Controls.Add(btnGirisDon, 0, 8);
            tableForm.Dock = DockStyle.Fill;
            tableForm.Location = new Point(40, 40);
            tableForm.Name = "tableForm";
            tableForm.RowCount = 10;
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 86F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 86F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableForm.Size = new Size(548, 560);
            tableForm.TabIndex = 0;
            // 
            // lblBaslik
            // 
            lblBaslik.AutoSize = true;
            lblBaslik.Dock = DockStyle.Bottom;
            lblBaslik.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblBaslik.ForeColor = Color.FromArgb(236, 237, 232);
            lblBaslik.Name = "lblBaslik";
            lblBaslik.TabIndex = 0;
            lblBaslik.Text = "Sakin Hesabı Oluştur";
            // 
            // lblAltBaslik
            // 
            lblAltBaslik.AutoSize = true;
            lblAltBaslik.Dock = DockStyle.Top;
            lblAltBaslik.Font = new Font("Segoe UI", 10F);
            lblAltBaslik.ForeColor = Color.FromArgb(154, 160, 176);
            lblAltBaslik.Name = "lblAltBaslik";
            lblAltBaslik.Padding = new Padding(0, 6, 0, 0);
            lblAltBaslik.TabIndex = 1;
            lblAltBaslik.Text = "Rol seçimi yoktur; hesabınız apartman sakini olarak açılır.";
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
            tableAdSoyad.Location = new Point(0, 74);
            tableAdSoyad.Margin = new Padding(0);
            tableAdSoyad.Name = "tableAdSoyad";
            tableAdSoyad.RowCount = 2;
            tableAdSoyad.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tableAdSoyad.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableAdSoyad.Size = new Size(548, 86);
            tableAdSoyad.TabIndex = 2;
            // 
            // lblAd
            // 
            lblAd.AutoSize = true;
            lblAd.Dock = DockStyle.Bottom;
            lblAd.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblAd.ForeColor = Color.FromArgb(154, 160, 176);
            lblAd.Name = "lblAd";
            lblAd.TabIndex = 0;
            lblAd.Text = "Ad";
            // 
            // txtAd
            // 
            txtAd.BackColor = Color.FromArgb(42, 46, 61);
            txtAd.BorderStyle = BorderStyle.FixedSingle;
            txtAd.Dock = DockStyle.Top;
            txtAd.Font = new Font("Segoe UI", 11F);
            txtAd.ForeColor = Color.FromArgb(236, 237, 232);
            txtAd.Margin = new Padding(0, 0, 8, 0);
            txtAd.Name = "txtAd";
            txtAd.TabIndex = 1;
            // 
            // lblSoyad
            // 
            lblSoyad.AutoSize = true;
            lblSoyad.Dock = DockStyle.Bottom;
            lblSoyad.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSoyad.ForeColor = Color.FromArgb(154, 160, 176);
            lblSoyad.Name = "lblSoyad";
            lblSoyad.TabIndex = 2;
            lblSoyad.Text = "Soyad";
            // 
            // txtSoyad
            // 
            txtSoyad.BackColor = Color.FromArgb(42, 46, 61);
            txtSoyad.BorderStyle = BorderStyle.FixedSingle;
            txtSoyad.Dock = DockStyle.Top;
            txtSoyad.Font = new Font("Segoe UI", 11F);
            txtSoyad.ForeColor = Color.FromArgb(236, 237, 232);
            txtSoyad.Margin = new Padding(8, 0, 0, 0);
            txtSoyad.Name = "txtSoyad";
            txtSoyad.TabIndex = 3;
            // 
            // lblKullaniciAdi
            // 
            lblKullaniciAdi.AutoSize = true;
            lblKullaniciAdi.Dock = DockStyle.Bottom;
            lblKullaniciAdi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblKullaniciAdi.ForeColor = Color.FromArgb(154, 160, 176);
            lblKullaniciAdi.Name = "lblKullaniciAdi";
            lblKullaniciAdi.TabIndex = 4;
            lblKullaniciAdi.Text = "Kullanıcı Adı";
            // 
            // txtKullaniciAdi
            // 
            txtKullaniciAdi.BackColor = Color.FromArgb(42, 46, 61);
            txtKullaniciAdi.BorderStyle = BorderStyle.FixedSingle;
            txtKullaniciAdi.Dock = DockStyle.Top;
            txtKullaniciAdi.Font = new Font("Segoe UI", 11F);
            txtKullaniciAdi.ForeColor = Color.FromArgb(236, 237, 232);
            txtKullaniciAdi.Name = "txtKullaniciAdi";
            txtKullaniciAdi.TabIndex = 5;
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
            tableSifre.Location = new Point(0, 226);
            tableSifre.Margin = new Padding(0);
            tableSifre.Name = "tableSifre";
            tableSifre.RowCount = 2;
            tableSifre.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tableSifre.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableSifre.Size = new Size(548, 86);
            tableSifre.TabIndex = 6;
            // 
            // lblSifre
            // 
            lblSifre.AutoSize = true;
            lblSifre.Dock = DockStyle.Bottom;
            lblSifre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSifre.ForeColor = Color.FromArgb(154, 160, 176);
            lblSifre.Name = "lblSifre";
            lblSifre.TabIndex = 0;
            lblSifre.Text = "Şifre";
            // 
            // txtSifre
            // 
            txtSifre.BackColor = Color.FromArgb(42, 46, 61);
            txtSifre.BorderStyle = BorderStyle.FixedSingle;
            txtSifre.Dock = DockStyle.Top;
            txtSifre.Font = new Font("Segoe UI", 11F);
            txtSifre.ForeColor = Color.FromArgb(236, 237, 232);
            txtSifre.Margin = new Padding(0, 0, 8, 0);
            txtSifre.Name = "txtSifre";
            txtSifre.TabIndex = 1;
            txtSifre.UseSystemPasswordChar = true;
            // 
            // lblSifreTekrar
            // 
            lblSifreTekrar.AutoSize = true;
            lblSifreTekrar.Dock = DockStyle.Bottom;
            lblSifreTekrar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSifreTekrar.ForeColor = Color.FromArgb(154, 160, 176);
            lblSifreTekrar.Name = "lblSifreTekrar";
            lblSifreTekrar.TabIndex = 2;
            lblSifreTekrar.Text = "Şifre Tekrar";
            // 
            // txtSifreTekrar
            // 
            txtSifreTekrar.BackColor = Color.FromArgb(42, 46, 61);
            txtSifreTekrar.BorderStyle = BorderStyle.FixedSingle;
            txtSifreTekrar.Dock = DockStyle.Top;
            txtSifreTekrar.Font = new Font("Segoe UI", 11F);
            txtSifreTekrar.ForeColor = Color.FromArgb(236, 237, 232);
            txtSifreTekrar.Margin = new Padding(8, 0, 0, 0);
            txtSifreTekrar.Name = "txtSifreTekrar";
            txtSifreTekrar.TabIndex = 3;
            txtSifreTekrar.UseSystemPasswordChar = true;
            // 
            // lblRolBilgi
            // 
            lblRolBilgi.AutoSize = true;
            lblRolBilgi.Dock = DockStyle.Fill;
            lblRolBilgi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRolBilgi.ForeColor = Color.FromArgb(197, 225, 29);
            lblRolBilgi.Name = "lblRolBilgi";
            lblRolBilgi.TabIndex = 7;
            lblRolBilgi.TextAlign = ContentAlignment.MiddleLeft;
            lblRolBilgi.Text = "Atanacak rol: Apartman Sakini";
            // 
            // btnKayitOl
            // 
            btnKayitOl.BackColor = Color.FromArgb(197, 225, 29);
            btnKayitOl.Cursor = Cursors.Hand;
            btnKayitOl.Dock = DockStyle.Fill;
            btnKayitOl.FlatAppearance.BorderSize = 0;
            btnKayitOl.FlatStyle = FlatStyle.Flat;
            btnKayitOl.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnKayitOl.ForeColor = Color.FromArgb(31, 34, 43);
            btnKayitOl.Name = "btnKayitOl";
            btnKayitOl.TabIndex = 8;
            btnKayitOl.Text = "Kayıt Ol";
            btnKayitOl.UseVisualStyleBackColor = false;
            btnKayitOl.Click += btnKayitOl_Click;
            // 
            // btnGirisDon
            // 
            btnGirisDon.BackColor = Color.FromArgb(50, 55, 72);
            btnGirisDon.Cursor = Cursors.Hand;
            btnGirisDon.Dock = DockStyle.Fill;
            btnGirisDon.FlatAppearance.BorderColor = Color.FromArgb(61, 66, 84);
            btnGirisDon.FlatStyle = FlatStyle.Flat;
            btnGirisDon.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnGirisDon.ForeColor = Color.FromArgb(236, 237, 232);
            btnGirisDon.Name = "btnGirisDon";
            btnGirisDon.TabIndex = 9;
            btnGirisDon.Text = "Giriş Ekranına Dön";
            btnGirisDon.UseVisualStyleBackColor = false;
            btnGirisDon.Click += btnGirisDon_Click;
            // 
            // FrmKayit
            // 
            AcceptButton = btnKayitOl;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(31, 34, 43);
            ClientSize = new Size(980, 640);
            Controls.Add(tableAna);
            MinimumSize = new Size(920, 580);
            Name = "FrmKayit";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kayıt Ol - Apartman Yönetim Paneli";
            tableAna.ResumeLayout(false);
            pnlSol.ResumeLayout(false);
            pnlSol.PerformLayout();
            pnlSag.ResumeLayout(false);
            tableForm.ResumeLayout(false);
            tableForm.PerformLayout();
            tableAdSoyad.ResumeLayout(false);
            tableAdSoyad.PerformLayout();
            tableSifre.ResumeLayout(false);
            tableSifre.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableAna;
        private Panel pnlSol;
        private Label lblSolAlt;
        private Label lblSolBaslik;
        private Label lblSolUst;
        private Panel pnlSag;
        private TableLayoutPanel tableForm;
        private TableLayoutPanel tableAdSoyad;
        private TableLayoutPanel tableSifre;
        private Button btnGirisDon;
        private Button btnKayitOl;
        private Label lblRolBilgi;
        private TextBox txtSifreTekrar;
        private Label lblSifreTekrar;
        private TextBox txtSifre;
        private Label lblSifre;
        private TextBox txtKullaniciAdi;
        private Label lblKullaniciAdi;
        private TextBox txtSoyad;
        private Label lblSoyad;
        private TextBox txtAd;
        private Label lblAd;
        private Label lblBaslik;
        private Label lblAltBaslik;
    }
}

namespace Apartman_Yönetim_Paneli_V0._1
{
    partial class FrmGiris
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
            lblKullaniciAdi = new Label();
            txtKullaniciAdi = new TextBox();
            lblSifre = new Label();
            txtSifre = new TextBox();
            btnGirisYap = new Button();
            lblHesapYok = new Label();
            btnKayitOl = new Button();
            tableAna.SuspendLayout();
            pnlSol.SuspendLayout();
            pnlSag.SuspendLayout();
            tableForm.SuspendLayout();
            SuspendLayout();
            // 
            // tableAna
            // 
            tableAna.BackColor = Color.FromArgb(31, 34, 43);
            tableAna.ColumnCount = 2;
            tableAna.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
            tableAna.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
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
            pnlSol.Padding = new Padding(40);
            pnlSol.Size = new Size(372, 640);
            pnlSol.TabIndex = 0;
            // 
            // lblSolAlt
            // 
            lblSolAlt.Font = new Font("Segoe UI", 10F);
            lblSolAlt.ForeColor = Color.FromArgb(154, 160, 176);
            lblSolAlt.Location = new Point(42, 330);
            lblSolAlt.Name = "lblSolAlt";
            lblSolAlt.Size = new Size(270, 90);
            lblSolAlt.TabIndex = 2;
            lblSolAlt.Text = "Sakin kayıtları açık, yönetici hesapları yalnızca sistem yöneticisi tarafından oluşturulur.";
            // 
            // lblSolBaslik
            // 
            lblSolBaslik.Font = new Font("Segoe UI Semibold", 28F, FontStyle.Bold);
            lblSolBaslik.ForeColor = Color.FromArgb(236, 237, 232);
            lblSolBaslik.Location = new Point(40, 210);
            lblSolBaslik.Name = "lblSolBaslik";
            lblSolBaslik.Size = new Size(280, 100);
            lblSolBaslik.TabIndex = 1;
            lblSolBaslik.Text = "Yönetim Paneli";
            // 
            // lblSolUst
            // 
            lblSolUst.AutoSize = true;
            lblSolUst.Font = new Font("Segoe UI", 10F);
            lblSolUst.ForeColor = Color.FromArgb(197, 225, 29);
            lblSolUst.Location = new Point(42, 180);
            lblSolUst.Name = "lblSolUst";
            lblSolUst.Size = new Size(147, 19);
            lblSolUst.TabIndex = 0;
            lblSolUst.Text = "APARTMAN YÖNETİMİ";
            // 
            // pnlSag
            // 
            pnlSag.BackColor = Color.FromArgb(31, 34, 43);
            pnlSag.Controls.Add(tableForm);
            pnlSag.Dock = DockStyle.Fill;
            pnlSag.Location = new Point(372, 0);
            pnlSag.Margin = new Padding(0);
            pnlSag.Name = "pnlSag";
            pnlSag.Padding = new Padding(48, 40, 48, 40);
            pnlSag.Size = new Size(608, 640);
            pnlSag.TabIndex = 1;
            // 
            // tableForm
            // 
            tableForm.ColumnCount = 1;
            tableForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableForm.Controls.Add(lblBaslik, 0, 0);
            tableForm.Controls.Add(lblAltBaslik, 0, 1);
            tableForm.Controls.Add(lblKullaniciAdi, 0, 2);
            tableForm.Controls.Add(txtKullaniciAdi, 0, 3);
            tableForm.Controls.Add(lblSifre, 0, 4);
            tableForm.Controls.Add(txtSifre, 0, 5);
            tableForm.Controls.Add(btnGirisYap, 0, 6);
            tableForm.Controls.Add(lblHesapYok, 0, 7);
            tableForm.Controls.Add(btnKayitOl, 0, 8);
            tableForm.Dock = DockStyle.Fill;
            tableForm.Location = new Point(48, 40);
            tableForm.Name = "tableForm";
            tableForm.RowCount = 10;
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableForm.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableForm.Size = new Size(512, 560);
            tableForm.TabIndex = 0;
            // 
            // lblBaslik
            // 
            lblBaslik.AutoSize = true;
            lblBaslik.Dock = DockStyle.Bottom;
            lblBaslik.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold);
            lblBaslik.ForeColor = Color.FromArgb(236, 237, 232);
            lblBaslik.Location = new Point(3, 7);
            lblBaslik.Name = "lblBaslik";
            lblBaslik.Size = new Size(506, 41);
            lblBaslik.TabIndex = 0;
            lblBaslik.Text = "Giriş Yap";
            // 
            // lblAltBaslik
            // 
            lblAltBaslik.AutoSize = true;
            lblAltBaslik.Dock = DockStyle.Top;
            lblAltBaslik.Font = new Font("Segoe UI", 10F);
            lblAltBaslik.ForeColor = Color.FromArgb(154, 160, 176);
            lblAltBaslik.Location = new Point(3, 48);
            lblAltBaslik.Name = "lblAltBaslik";
            lblAltBaslik.Padding = new Padding(0, 6, 0, 0);
            lblAltBaslik.Size = new Size(506, 25);
            lblAltBaslik.TabIndex = 1;
            lblAltBaslik.Text = "Hesabınızla sisteme giriş yapın.";
            // 
            // lblKullaniciAdi
            // 
            lblKullaniciAdi.AutoSize = true;
            lblKullaniciAdi.Dock = DockStyle.Bottom;
            lblKullaniciAdi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblKullaniciAdi.ForeColor = Color.FromArgb(154, 160, 176);
            lblKullaniciAdi.Location = new Point(3, 97);
            lblKullaniciAdi.Name = "lblKullaniciAdi";
            lblKullaniciAdi.Size = new Size(506, 15);
            lblKullaniciAdi.TabIndex = 2;
            lblKullaniciAdi.Text = "Kullanıcı Adı";
            // 
            // txtKullaniciAdi
            // 
            txtKullaniciAdi.BackColor = Color.FromArgb(42, 46, 61);
            txtKullaniciAdi.BorderStyle = BorderStyle.FixedSingle;
            txtKullaniciAdi.Dock = DockStyle.Top;
            txtKullaniciAdi.Font = new Font("Segoe UI", 11F);
            txtKullaniciAdi.ForeColor = Color.FromArgb(236, 237, 232);
            txtKullaniciAdi.Location = new Point(3, 115);
            txtKullaniciAdi.Name = "txtKullaniciAdi";
            txtKullaniciAdi.Size = new Size(506, 27);
            txtKullaniciAdi.TabIndex = 3;
            // 
            // lblSifre
            // 
            lblSifre.AutoSize = true;
            lblSifre.Dock = DockStyle.Bottom;
            lblSifre.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblSifre.ForeColor = Color.FromArgb(154, 160, 176);
            lblSifre.Location = new Point(3, 167);
            lblSifre.Name = "lblSifre";
            lblSifre.Size = new Size(506, 15);
            lblSifre.TabIndex = 4;
            lblSifre.Text = "Şifre";
            // 
            // txtSifre
            // 
            txtSifre.BackColor = Color.FromArgb(42, 46, 61);
            txtSifre.BorderStyle = BorderStyle.FixedSingle;
            txtSifre.Dock = DockStyle.Top;
            txtSifre.Font = new Font("Segoe UI", 11F);
            txtSifre.ForeColor = Color.FromArgb(236, 237, 232);
            txtSifre.Location = new Point(3, 185);
            txtSifre.Name = "txtSifre";
            txtSifre.Size = new Size(506, 27);
            txtSifre.TabIndex = 5;
            txtSifre.UseSystemPasswordChar = true;
            // 
            // btnGirisYap
            // 
            btnGirisYap.BackColor = Color.FromArgb(197, 225, 29);
            btnGirisYap.Cursor = Cursors.Hand;
            btnGirisYap.Dock = DockStyle.Bottom;
            btnGirisYap.FlatAppearance.BorderSize = 0;
            btnGirisYap.FlatStyle = FlatStyle.Flat;
            btnGirisYap.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            btnGirisYap.ForeColor = Color.FromArgb(31, 34, 43);
            btnGirisYap.Location = new Point(3, 252);
            btnGirisYap.Name = "btnGirisYap";
            btnGirisYap.Size = new Size(506, 29);
            btnGirisYap.TabIndex = 6;
            btnGirisYap.Text = "Giriş Yap";
            btnGirisYap.UseVisualStyleBackColor = false;
            btnGirisYap.Click += btnGirisYap_Click;
            // 
            // lblHesapYok
            // 
            lblHesapYok.AutoSize = true;
            lblHesapYok.Dock = DockStyle.Bottom;
            lblHesapYok.Font = new Font("Segoe UI", 9F);
            lblHesapYok.ForeColor = Color.FromArgb(154, 160, 176);
            lblHesapYok.Location = new Point(3, 297);
            lblHesapYok.Name = "lblHesapYok";
            lblHesapYok.Size = new Size(506, 15);
            lblHesapYok.TabIndex = 7;
            lblHesapYok.Text = "Sakin hesabınız yok mu?";
            // 
            // btnKayitOl
            // 
            btnKayitOl.BackColor = Color.FromArgb(50, 55, 72);
            btnKayitOl.Cursor = Cursors.Hand;
            btnKayitOl.Dock = DockStyle.Top;
            btnKayitOl.FlatAppearance.BorderColor = Color.FromArgb(61, 66, 84);
            btnKayitOl.FlatStyle = FlatStyle.Flat;
            btnKayitOl.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnKayitOl.ForeColor = Color.FromArgb(236, 237, 232);
            btnKayitOl.Location = new Point(3, 315);
            btnKayitOl.Name = "btnKayitOl";
            btnKayitOl.Size = new Size(506, 28);
            btnKayitOl.TabIndex = 8;
            btnKayitOl.Text = "Sakin Olarak Kayıt Ol";
            btnKayitOl.UseVisualStyleBackColor = false;
            btnKayitOl.Click += btnKayitOl_Click;
            // 
            // FrmGiris
            // 
            AcceptButton = btnGirisYap;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(31, 34, 43);
            ClientSize = new Size(980, 640);
            Controls.Add(tableAna);
            MinimumSize = new Size(920, 580);
            Name = "FrmGiris";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Giriş Yap - Apartman Yönetim Paneli";
            Load += FrmGiris_Load;
            tableAna.ResumeLayout(false);
            pnlSol.ResumeLayout(false);
            pnlSol.PerformLayout();
            pnlSag.ResumeLayout(false);
            tableForm.ResumeLayout(false);
            tableForm.PerformLayout();
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
        private Button btnKayitOl;
        private Label lblHesapYok;
        private Button btnGirisYap;
        private TextBox txtSifre;
        private Label lblSifre;
        private TextBox txtKullaniciAdi;
        private Label lblKullaniciAdi;
        private Label lblBaslik;
        private Label lblAltBaslik;
    }
}

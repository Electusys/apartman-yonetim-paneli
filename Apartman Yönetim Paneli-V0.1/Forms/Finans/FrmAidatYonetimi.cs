using MySqlConnector;

namespace Apartman_Yönetim_Paneli_V0._1
{

    /// <summary>
    /// Yönetici: dönem + tutar + son ödeme tarihi girer, daireleri seçer, borç satırları oluşur.
    /// Aynı daire + aynı dönem ikinci kez yazılmaz.
    /// </summary>
    public class FrmAidatYonetimi : Form
    {
        private readonly DateTimePicker _dtpDonem;
        private readonly NumericUpDown _nudTutar;
        private readonly DateTimePicker _dtpSonOdeme;
        private readonly CheckedListBox _lstDaireler;
        private readonly DataGridView _dgv;
        private readonly int _kullaniciId;

        public FrmAidatYonetimi(int kullaniciId, string adSoyad)
        {
            _kullaniciId = kullaniciId;
            Panel govde = FormIskeleti.Olustur(this, adSoyad, "Aidat Yönetimi", "Dönem ve tutarı girip aidat uygulanacak daireleri seçin.");

            TableLayoutPanel tablo = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
            tablo.RowStyles.Add(new RowStyle(SizeType.Absolute, 280F));
            tablo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            _dtpDonem = FormIskeleti.Tarih("yyyy-MM");
            _nudTutar = FormIskeleti.Para();
            _dtpSonOdeme = FormIskeleti.Tarih();
            _dtpSonOdeme.Value = DateTime.Today.AddDays(15);
            _lstDaireler = new CheckedListBox
            {
                Dock = DockStyle.Fill,
                BackColor = UiTema.Kart,
                ForeColor = UiTema.Yazi,
                BorderStyle = BorderStyle.FixedSingle,
                CheckOnClick = true
            };
            Button btnTumunu = new Button { Text = "Tüm daireleri seç", Width = 180, Height = 36 };
            UiTema.IkincilButon(btnTumunu);
            btnTumunu.Click += (_, _) =>
            {
                for (int i = 0; i < _lstDaireler.Items.Count; i++)
                {
                    _lstDaireler.SetItemChecked(i, true);
                }
            };
            Button btnOlustur = FormIskeleti.Birincil("Yeni Aidat Oluştur");
            btnOlustur.Click += BtnOlustur_Click;

            TableLayoutPanel form = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 6 };
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
            form.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            form.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            form.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            form.Controls.Add(FormIskeleti.Etiket("Aidat dönemi"), 0, 0);
            form.Controls.Add(_dtpDonem, 1, 0);
            form.Controls.Add(FormIskeleti.Etiket("Tutar"), 0, 1);
            form.Controls.Add(_nudTutar, 1, 1);
            form.Controls.Add(FormIskeleti.Etiket("Son ödeme tarihi"), 0, 2);
            form.Controls.Add(_dtpSonOdeme, 1, 2);
            form.Controls.Add(FormIskeleti.Etiket("Uygulanacak daireler"), 0, 3);
            form.Controls.Add(_lstDaireler, 1, 3);
            form.Controls.Add(new Label(), 0, 4);
            form.Controls.Add(btnTumunu, 1, 4);
            form.Controls.Add(new Label(), 0, 5);
            form.Controls.Add(btnOlustur, 1, 5);

            _dgv = FormIskeleti.Tablo();
            tablo.Controls.Add(form, 0, 0);
            tablo.Controls.Add(_dgv, 0, 1);
            govde.Controls.Add(tablo);
            Yukle();
        }

        private void Yukle()
        {
            try
            {
                _dgv.DataSource = Veritabani.TabloGetir(
                    @"SELECT b.AidatBorcID, dnm.Donem, CONCAT(IFNULL(d.Blok, '-'), ' / ', d.DaireNo) AS Daire,
                             b.Tutar, b.KalanTutar, b.Durum, dnm.SonOdemeTarihi
                      FROM AidatBorclari b
                      INNER JOIN AidatDonemleri dnm ON dnm.AidatDonemID = b.AidatDonemID
                      INNER JOIN Daireler d ON d.DaireID = b.DaireID
                      ORDER BY dnm.Donem DESC, d.Blok, d.DaireNo");
                UiTema.KolonBasliklari(_dgv,
                    ("AidatBorcID", "ID"),
                    ("KalanTutar", "Kalan"),
                    ("SonOdemeTarihi", "Son ödeme"));

                _lstDaireler.Items.Clear();
                foreach (ListeKalemi kalem in Veritabani.DaireListesi())
                {
                    _lstDaireler.Items.Add(kalem, true);
                }
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Aidat listesi yüklenemedi");
            }
        }

        private void BtnOlustur_Click(object? sender, EventArgs e)
        {
            if (_nudTutar.Value <= 0)
            {
                FormIskeleti.Uyari("Tutar geçersiz. Sıfırdan büyük bir tutar giriniz.");
                return;
            }

            List<int> daireler = new List<int>();
            foreach (object item in _lstDaireler.CheckedItems)
            {
                if (item is ListeKalemi kalem)
                {
                    daireler.Add(kalem.Id);
                }
            }

            if (daireler.Count == 0)
            {
                FormIskeleti.Uyari("En az bir daire seçiniz.");
                return;
            }

            string donem = _dtpDonem.Value.ToString("yyyy-MM");
            decimal tutar = _nudTutar.Value;
            DateTime sonOdeme = _dtpSonOdeme.Value.Date;
            int eklenen = 0;
            int atlanan = 0;

            try
            {
                using MySqlConnection baglanti = Veritabani.BaglantiAc();
                using MySqlTransaction tx = baglanti.BeginTransaction();

                long donemId;
                using (MySqlCommand ara = new MySqlCommand("SELECT AidatDonemID FROM AidatDonemleri WHERE Donem = @donem", baglanti, tx))
                {
                    ara.Parameters.AddWithValue("@donem", donem);
                    object? id = ara.ExecuteScalar();
                    if (id == null)
                    {
                        using MySqlCommand ekle = new MySqlCommand(
                            @"INSERT INTO AidatDonemleri (Donem, Tutar, SonOdemeTarihi, OlusturanKullaniciID)
                              VALUES (@donem, @tutar, @son, @kullanici)", baglanti, tx);
                        ekle.Parameters.AddWithValue("@donem", donem);
                        ekle.Parameters.AddWithValue("@tutar", tutar);
                        ekle.Parameters.AddWithValue("@son", sonOdeme);
                        ekle.Parameters.AddWithValue("@kullanici", _kullaniciId);
                        ekle.ExecuteNonQuery();
                        donemId = ekle.LastInsertedId;
                    }
                    else
                    {
                        donemId = Convert.ToInt64(id);
                    }
                }

                foreach (int daireId in daireler)
                {
                    using MySqlCommand kontrol = new MySqlCommand(
                        "SELECT COUNT(*) FROM AidatBorclari WHERE AidatDonemID = @donem AND DaireID = @daire", baglanti, tx);
                    kontrol.Parameters.AddWithValue("@donem", donemId);
                    kontrol.Parameters.AddWithValue("@daire", daireId);
                    if (Convert.ToInt32(kontrol.ExecuteScalar()) > 0)
                    {
                        atlanan++;
                        continue;
                    }

                    using MySqlCommand borc = new MySqlCommand(
                        @"INSERT INTO AidatBorclari (AidatDonemID, DaireID, Tutar, KalanTutar, Durum)
                          VALUES (@donem, @daire, @tutar, @tutar, @durum)", baglanti, tx);
                    borc.Parameters.AddWithValue("@donem", donemId);
                    borc.Parameters.AddWithValue("@daire", daireId);
                    borc.Parameters.AddWithValue("@tutar", tutar);
                    borc.Parameters.AddWithValue("@durum", Veritabani.BorcOdenmedi);
                    borc.ExecuteNonQuery();
                    eklenen++;
                }

                tx.Commit();
                FormIskeleti.Bilgi($"{eklenen} daire için aidat borcu oluşturuldu." + (atlanan > 0 ? $" {atlanan} dairede bu dönem zaten kayıtlıydı, mükerrer yazılmadı." : ""));
                Yukle();
            }
            catch (Exception ex)
            {
                FormIskeleti.Hata(ex, "Aidat oluşturulamadı");
            }
        }
    }
}

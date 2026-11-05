namespace Apartman_Yönetim_Paneli_V0._1
{
    /// <summary>
    /// ComboBox / CheckedListBox satırı: ekranda metin, veritabanında Id.
    /// </summary>
    internal sealed class ListeKalemi
    {
        public int Id { get; }
        public string Metin { get; }

        public ListeKalemi(int id, string metin)
        {
            Id = id;
            Metin = metin;
        }

        public override string ToString() => Metin;
    }
}

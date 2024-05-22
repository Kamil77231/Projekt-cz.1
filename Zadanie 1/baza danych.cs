using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class BazaDanych
{
    public List<Album> Albumy { get; set; } = new List<Album>();

    public void DodajAlbum(Album album)
    {
        Albumy.Add(album);
    }

    public Album PobierzSzczegółyAlbumu(int id)
    {
        return Albumy.FirstOrDefault(a => a.Id == id);
    }

    public Utwór PobierzSzczegółyUtworu(int idAlbumu, int idUtworu)
    {
        var album = PobierzSzczegółyAlbumu(idAlbumu);
        return album?.Utwory.FirstOrDefault(u => u.Id == idUtworu);
    }

    public List<Wykonawca> PobierzWykonawcówAlbumu(int idAlbumu)
    {
        var album = PobierzSzczegółyAlbumu(idAlbumu);
        return album?.Utwory.SelectMany(u => u.Wykonawcy).Distinct().ToList();
    }

    public void ZapiszDoPliku(string ścieżka)
    {
        var formatter = new BinaryFormatter();
        using (var stream = new FileStream(ścieżka, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            formatter.Serialize(stream, this);
        }
    }

    public void OdczytajZPliku(string ścieżka)
    {
        var formatter = new BinaryFormatter();
        using (var stream = new FileStream(ścieżka, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            var baza = (BazaDanych)formatter.Deserialize(stream);
            Albumy = baza.Albumy;
        }
    }
}
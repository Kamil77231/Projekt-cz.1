[Serializable]
public class Album
{
    public int Id { get; set; }
    public string Tytuł { get; set; }
    public string Typ { get; set; } 

    
    public long CzasTrwania
    {
        get { return (long)Utwory.Sum(u => u.CzasTrwania.TotalSeconds); }
    }

    public List<Utwór> Utwory { get; set; } = new List<Utwór>();

    public Album()
    {
        
    }

}
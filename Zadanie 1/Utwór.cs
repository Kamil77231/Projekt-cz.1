[Serializable]
public class Utwór
{
    public int Id { get; set; }
    public string Tytuł { get; set; }
    public TimeSpan CzasTrwania { get; set; }
    public List<Wykonawca> Wykonawcy { get; set; } = new List<Wykonawca>();
    public string Kompozytor { get; set; }
}
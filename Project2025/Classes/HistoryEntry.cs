namespace Project2025.Classes
{
    public class HistoryEntry
    {
        //file with variables for history data

        public string Title { get; set; }
        public int Year { get; set; }
        public List<string> Genre { get; set; }
        public string Emoji { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

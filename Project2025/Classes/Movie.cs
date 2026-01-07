using System.Text.Json.Serialization;

namespace Project2025.Classes
{
    public class Movie
    {
        //file with variables for movie entrys

        public string title { get; set; }
        public int year { get; set; }
        public List<string> genre { get; set; }
        public string director { get; set; }
        public double rating { get; set; }
        public string emoji { get; set; }
    }

}

/*
 * Base model for TV Shows. Categories can use this structure, add extras when needed.
 * Last Modified: 22 Aug 2016.
 */ 

namespace WatchfulBot.Models
{
    public class Shows
    {
        public string title { get; set; }
        public int year { get; set; }
        public Ids ids { get; set; }
    }
    public class Ids
    {
        public int trakt { get; set; }
        public string slug { get; set; }
        public int tvdb { get; set; }
        public string imdb { get; set; }
        public int tmdb { get; set; }
        public int tvrage { get; set; }
    }
}
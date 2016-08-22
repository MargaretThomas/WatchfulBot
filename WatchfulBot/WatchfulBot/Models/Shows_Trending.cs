/*
 * Based off trakt's version 2 of Trending Shows.
 * Last Modified: 22 Aug 2016.
 */ 

namespace WatchfulBot.Models
{
    public class Shows_Trending
    {
        public int watchers { get; set; }
        public Shows shows { get; set; }
    }
}
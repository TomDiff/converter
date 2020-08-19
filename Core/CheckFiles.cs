using System.Collections.Generic;

namespace Core
{
    public class CheckFiles
    {
        public CheckFiles()
        {
            Original = new List<string>();
            Images = new List<string>();
        }
        public List<string> Original { get; set; }
        public List<string> Images { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlStemming.Core
{
    public class UrlStemmingSettings
    {
        public UrlStemmingSettings()
        {
            // These are reasonable defaults
            TrailingSlashes = TrailingSlashes.Ignore;
            ArgumentBlacklist = new List<string>();
            ArgumentWhitelist = new List<string>();
        }

        public string ForceHost { get; set; }
        public bool ForceLowerCase { get; set; }
        public string ForceScheme { get; set;}
        public bool RemoveBookmarks { get; set;}
        public bool RemoveSubdomain { get; set;}
        public List<string> ArgumentWhitelist { get; set; }
        public List<string> ArgumentBlacklist { get; set; }
        public TrailingSlashes TrailingSlashes { get; set; }
        public bool ReorderQuerystringArgs { get; set; }
    }
}

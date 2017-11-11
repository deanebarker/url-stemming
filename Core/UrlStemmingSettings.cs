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
            DefaultHost = "example.com";
            DefaultScheme = "http";
            EncodingBehavior = EncodingBehavior.None;
        }

        public string ForceHost { get; set; }
        public bool ForceLowerCase { get; set; }
        public string ForceScheme { get; set;}
        public bool RemoveBookmarks { get; set;}
        public bool RemoveSubdomain { get; set;}
        public List<string> ArgumentWhitelist { get; set; }
        public List<string> ArgumentBlacklist { get; set; }
        public TrailingSlashes TrailingSlashes { get; set; }
        public bool ReorderQuerystringArguments { get; set; }
        public string DefaultHost { get; set; }
        public string DefaultScheme { get; set; }
        public EncodingBehavior EncodingBehavior { get; set; }
		public bool ClearQuerystring { get; set; }
        public int ForcePort { get; set; }
    }

    public enum EncodingBehavior
    {
        None,
        Encode,
        Decode
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UrlStemming.Core;

namespace Website.Models
{
    public class EqualityCheckerFormModel
    {
        public EqualityCheckerFormModel()
        {
            // Interface defauts
            ReorderQuerystringArgs = true;
            TrailingSlashes = TrailingSlashes.Ignore;
            ForceLowerCase = true;
            RemoveBookmarks = true;

            // These need to be set because we try to split them...
            ArgumentBlacklist = string.Empty;
            ArgumentWhitelist = string.Empty;
        }

        public bool AreEqual { get; set; }
        public bool Submitted { get; set; }

        public bool Compared
        {
            get
            {
                return (Submitted && (TwoStemmed != null));
            }
        }

        [Required]
        public string One { get; set; }
        public string Two { get; set; }

        public StemmedUrl OneStemmed { get; set; }
        public StemmedUrl TwoStemmed { get; set; }

        public string ForceHost { get; set; }
        public bool ForceLowerCase { get; set; }
        public string ForceScheme { get; set; }
        public bool RemoveBookmarks { get; set; }
        public bool RemoveSubdomain { get; set; }
        public TrailingSlashes TrailingSlashes { get; set; }
        public bool ReorderQuerystringArgs { get; set; }
        public string ArgumentWhitelist { get; set; }
        public string ArgumentBlacklist { get; set; }

        public UrlStemmingSettings GetSettings()
        {
            var settings = new UrlStemmingSettings()
            {
                ForceHost = ForceHost,
                ForceLowerCase = ForceLowerCase,
                ForceScheme = ForceScheme,
                TrailingSlashes = TrailingSlashes,
                ReorderQuerystringArguments = ReorderQuerystringArgs,
                RemoveBookmarks = RemoveBookmarks,
                RemoveSubdomain = RemoveSubdomain
            };

            if (!string.IsNullOrWhiteSpace(ArgumentWhitelist))
            {
                settings.ArgumentWhitelist = ArgumentWhitelist.Split(",".ToCharArray()).Select(x => x.Trim()).ToList();
            }

            if (!string.IsNullOrWhiteSpace(ArgumentBlacklist))
            {
                settings.ArgumentBlacklist = ArgumentBlacklist.Split(",".ToCharArray()).Select(x => x.Trim()).ToList();
            }

            return settings;
        }
    }
}
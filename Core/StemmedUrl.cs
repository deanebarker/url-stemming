using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UrlStemming.Core
{
    public class StemmedUrl
    {
        private UriBuilder workingUrl;

        public StemmedUrl(string url, UrlStemmingSettings settings = null)
        {
            settings = settings ?? UrlStemmer.Settings;


            // Pre-parse

            if (settings.ForceLowerCase)
            {
                url = url.ToLower();
            }

            workingUrl = new UriBuilder(url);


            // Post-parse

            if(settings.RemoveBookmarks)
            {
                workingUrl.Fragment = null;
            }

            if(settings.RemoveSubdomain)
            {
                var domainSegments = workingUrl.Host.Split(".".ToCharArray());
                if(domainSegments.Count() > 2)
                {
                    workingUrl.Host = string.Join(".", domainSegments.Skip(Math.Max(0, domainSegments.Count() - 2)));
                }
            }

            if (settings.TrailingSlashes == TrailingSlashes.AlwaysStrip)
            {
                workingUrl.Path = workingUrl.Path.TrimEnd("/".ToCharArray());
            }

            if(settings.TrailingSlashes == TrailingSlashes.AlwaysAdd)
            {
                workingUrl.Path = string.Concat(workingUrl.Path.TrimEnd("/".ToCharArray()), "/"); 
            }
            
            if(HasValue(settings.ForceScheme))
            {
                workingUrl.Scheme = settings.ForceScheme;
            }

            if (HasValue(settings.ForceHost))
            {
                workingUrl.Host = settings.ForceHost;
            }


            // Querystring stuff

            var existingQuery = HttpUtility.ParseQueryString(workingUrl.Query);

            if (settings.ArgumentWhitelist.Any())
            {
                existingQuery.AllKeys.Where(x => !settings.ArgumentWhitelist.Contains(x)).ToList().ForEach(y => existingQuery.Remove(y));
            }

            settings.ArgumentBlacklist.ForEach(x => existingQuery.Remove(x));

            // We're just going to blanket ignore port for the time being
            workingUrl.Port = -1;

            if (settings.ReorderQuerystringArguments)
            {
                if (!string.IsNullOrWhiteSpace(workingUrl.Query))
                {
                    var keys = existingQuery.AllKeys;
                    Array.Sort(keys);
                    foreach (var key in keys)
                    {
                        var value = existingQuery[key];
                        existingQuery.Remove(key);
                        existingQuery.Add(key, value);
                    }
                }
            }

            workingUrl.Query = existingQuery.ToString();

        }

        private bool HasValue(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public override string ToString()
        {
            return workingUrl.Uri.AbsoluteUri;
        }

        public override int GetHashCode()
        {
            return workingUrl.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(!(obj is StemmedUrl))
            {
                return false;
            }
            return ToString() == ((StemmedUrl)obj).ToString();
        }

        public static implicit operator string(StemmedUrl url) { return url != null ? url.ToString() : string.Empty; }
    }
}

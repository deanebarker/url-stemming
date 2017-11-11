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

            // If the URL starts with a slash, we're assuming it's a relative URL from root
            if(url.StartsWith("/"))
            {
                url = string.Concat(settings.DefaultScheme, "://", settings.DefaultHost, url);
            }

            // If the URL doesn't have "://" and doesn't start with a slash, then it's a domain and URL with no scheme
            if(!url.Contains("://"))
            {
                url = string.Concat(settings.DefaultScheme, "://", url);
            }

            if (settings.ForceLowerCase)
            {
                url = url.ToLower();
            }

            if(settings.EncodingBehavior == EncodingBehavior.Encode)
            {
                url = HttpUtility.UrlEncode(url);
            }

            if (settings.EncodingBehavior == EncodingBehavior.Decode)
            {
                url = HttpUtility.UrlDecode(url);
            }

            workingUrl = new UriBuilder(url);


            // Post-parse

            if (settings.ForcePort != 0)
            {
                workingUrl.Port = settings.ForcePort;
            }

			if (settings.RemoveBookmarks)
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

			if (settings.ClearQuerystring)
			{
				existingQuery.Clear();
			}
			else
			{
				// This only matters if we're NOT clearly the querystring

				if (settings.ArgumentWhitelist.Any())
				{
					existingQuery.AllKeys.Where(x => !settings.ArgumentWhitelist.Contains(x)).ToList().ForEach(y => existingQuery.Remove(y));
				}

				settings.ArgumentBlacklist.ForEach(x => existingQuery.Remove(x));

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
			}

            workingUrl.Query = existingQuery.ToString();

        }

        public Uri Uri
        {
            get
            {
                return workingUrl.Uri;
            }
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

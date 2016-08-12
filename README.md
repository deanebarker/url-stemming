# URL Stemmer

This utility class attempts to normalize a URL based on provided settings, usually to prepare it for comparison.

    var stemmedUrl = UrlStemmer.Stem("http://gadgetopia.com");
	var areTheyEqual = UrlStemmer.Compare("http://gadgetopia.com", "http://www.gadgetopia.com");

When comparing, both URLs are stemmed using the same settings (both URLs are actually stemmed using the same settings, then simply string compared).

`UrlStemmer` has a static `UrlStemmingSettings` object which can be changed.

    UrlStemmer.ForceHost = "gadgetopia.com";
    UrlStemmer.RemoveBookmarks = true;

If no settings are changed from the defaults, the output of `Stem` should be the same as the input.

Custom settings objects can be optionally passed into either method:

    var stemmedUrl = UrlStemmer.Stem("http://gadgetopia.com", new UrlStemmingSettings());

If not passed in, the static settings are used.

Available settings on `UrlStemmingSettings`:

**ForceHost** (string)   
If set, the host (domain) will be changed to this value

**ForceLowerCase** (bool)   
The URL will be converted to lower-case

**ForceScheme** (string)   
If set, the scheme (protocol) will be changed to this value

**RemoveBookmarks** (bool)   
Any bookmarks at the end of the URL will be removed

**RemoveSubdomain** (bool)   
Any and all subdomains will be removed

**ArgumentBlacklist** (List<string\>)   
These querystring argument keys will be removed from the URL

**ArgumentWhitelist** (List<string\>)    
Anything _other than_ these querystring argument keys will be removed from the URL

**TrailingSlashes** (enum)   
Trailing slashes will be always added, always stripped, or ignored. Defaults to ignore (meaning, slashes will be left as they were passed in).

**ReorderQuerystringArgs** (bool)   
Querystring arguments will be reordered alphabetically

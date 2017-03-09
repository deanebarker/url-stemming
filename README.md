# URL Stemmer

This utility class attempts to normalize, stem, or simplify a URL based on provided settings, usually to prepare it for comparison or storage.

An online tester is located here: [http://url-stemmer.azurewebsites.net/][1].  (The source of this tester is contained in this repository under "Website" folder.)

[1]: http://url-stemmer.azurewebsites.net/

Basic usage:

    UrlStemmer.Settings.RemoveSubdomain = true;
    UrlStemmer.Settings.ForceScheme = "http";
    UrlStemmer.Settings.ReorderQuerystringArguments = true;
    var stemmedUrl = UrlStemmer.Stem("https://www.gadgetopia.com/?c=d&a=b");
    // Result: "http://gadgetopia.com/?a=b&c=d

If desired, the `StemmedUrl` class can be created directly:

    var url = new StemmedUrl("http://gadgetopia.com");
    var stem = url.ToString();

`StemmedUrl` overrides `ToString` which always provides the stemmed output of the URL passed into the constructor.

If the URL begins with a leading slash, it's assumed that it's local URL without a domain. The values of `DefaultScheme` and `DefaultHost` will be prepended. If the URL does not begin with a leading slash and does not contain "://", it's assumed that it's an absolute URL with a domain but not scheme. The value of `DefaultScheme` will be prepended. The default values of these settings are `http` and `example.com`.

    UrlStemmer.Stem("/my/path");
    // Result: http://example.com/my/path

    UrlStemmer.Stem("gadgetopia.com");
    // Result: http://gadgetopia.com/

`UrlStemmer` has a static `UrlStemmingSettings` object which is changed to affect how URLs are stemmed.

    UrlStemmer.Settings.ForceHost = "gadgetopia.com";
    UrlStemmer.Settings.RemoveBookmarks = true;

Stemming will always these static settings to ensure all URLs are stemmed identically. However it is possible to pass a custom settings object into the `Stem` and `Compare` methods, and into the constructor of `StemmedUrl`:

    var customSettings = new UrlStemmingSettings() { RemoveBookmarks = true; };
    var stemmedUrl = UrlStemmer.Stem("http://gadgetopia.com", customSettings);

All settings default to a "pass-through" state, meaning that if no settings are changed from the defaults, the output of `Stem` should be the same as the input (with a few built-in exceptions -- see "Built-in Stemming" below for some rules that the `UrlBuilder` class imposes on us).

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
Any (and all) subdomains will be removed

**ReorderQuerystringArguments** (bool)   
Querystring arguments will be reordered alphabetically

**ArgumentBlacklist** (List<string\>)   
These querystring argument keys will be removed from the URL

**ArgumentWhitelist** (List<string\>)    
Anything _other than_ these querystring argument keys will be removed from the URL

**ClearQuerystring** (bool)     
Will remove _all_ querystring arguments. (Setting this flag will make any ArgumentWhitelist, ArgumentBlacklist, and ReorderQuerystringArguments a moot point. They will simply be ignored.)

**TrailingSlashes** (enum)   
Trailing slashes will be always added, always stripped, or ignored (meaning, slashes will be left as they were passed in).  There is a limitation here (see "Built-in Stemming" below), in that you cannot strip the trailing slash if there is no folder path.

**DefaultHost** (string)   
The default host prepended to the URL when no host is provided. Defaults to "example.com" as specified in [RFC 2606][2].

**DefaultScheme** (string)   
The default scheme prepended to the URL when no scheme is provided. Defaults to "http".

**EncodingBehavior** (enum: None, Decode, Encode)     
Whether and how to encode or decode "%" encoded URL characters. Defaults to "None," which passes the URL through.


[2]: https://tools.ietf.org/html/rfc2606

## Built-in Stemming

`UrlStemmer` uses `UrlBuilder` under the hood, and there are three built-in stemming formats that it imposes which cannot be changed.  However, in most cases, these are acceptable and desirable.

* The URL will always have a path, even if that path is simply a forward slash to represent "root." It is not possible to generate a URL like `http://gadgetopia.com` (with no slash on the end).

* Backslashes in a URL will always be replaced with forward slashes.

* Double- and single dot notation ("parent pathing") will be removed and normalized. So, a double-dot segment (`/../`) meant to climb back up the path will be applied and the URL automatically corrected for it. (`/foo/../bar/./baz` becomes `/bar/baz`)

## Examples

Calling `Reset` will replace all settings with a default `UrlStemmingSettings` object.

    String stem;

    UrlStemmer.Settings.RemoveSubdomain = true;
    stem = UrlStemmer.Stem("http://www.gadgetopia.com/");
    // Result: "http://gadgetopia.com"

    UrlStemmer.Reset();
    UrlStemmer.Settings.ReorderQuerystringArgs = true;
    UrlStemmer.Settings.ArgumentBlacklist.Add("a");
    stem = UrlStemmer.Stem("http://gadgetopia.com/?e=f&c=d&a=b
    // Result: "http://gadgetopia.com/?c=d&e=f

    UrlStemmer.Reset()
    UrlStemmer.Settings.RemoveBookmarks = true;
    UrlStemmer.Settings.ForceScheme = "http";
    UrlStemmer.Settings.TrailingSlashes = TrailingSlashes.AlwaysRemove;
    stem = UrlStemmer.Stem("https://gadgetopia.com/foo/#chapter-1
    // Result: "http://gadgetopia.com/foo

    UrlStemmer.Reset();
    UrlStemmer.Settings.ForceLowerCase = true;
    UrlStemmer.Settings.ArgumentWhitelist.Add("c");
    stem = UrlStemmer.Stem("http://gadgetopia.com/FOO/?e=f&c=d&a=b
    // Result: "http://gadgetopia.com/foo/?c=d


## To Do

* We need to have a consistent handling of multiple querystring arguments. For example: `?a=b&c=d&a=f`
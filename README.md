# URL Equality

Given two URLs as strings, create two `NormalizedUrl` objects, then call the `Equals` method on one of them, passing the other one in.

    var settings = new UrlEqualitySettings();
    var a = new NormalizedUrl("http://gadgetopia.com/", settings); 
    var b = new NormalizedUrl("http://gadgetopia.com/", settings);

	var areTheyTheSame = a.Equals(b);

The `ToString` method is overridden to show you the normalized result. (If you just want to normalize a URL for storage, just create the object and save the `ToString` result.)

`UrlEqualitySettings` currently has four `boolean` values, all which should be self-explanatory.

* IgnoreHost
* CaseSensitive
* IgnoreScheme
* IgnoreTrailingSlash

Obviously, all initialize to `false`.

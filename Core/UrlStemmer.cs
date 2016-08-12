namespace UrlStemming.Core
{
    public static class UrlStemmer
    {
        public static UrlStemmingSettings Settings { get; set; }

        static UrlStemmer()
        {
            Reset();
        }

        public static void Reset()
        {
            Settings = new UrlStemmingSettings();
        }

        public static string Stem(string url, UrlStemmingSettings settings = null)
        {
            return new StemmedUrl(url, settings ?? Settings);
        }

        public static bool AreEqual(string a, string b, UrlStemmingSettings settings = null)
        {
            settings = settings ?? Settings;
            return new StemmedUrl(a, settings).Equals(new StemmedUrl(b, settings));
        }
    }
}

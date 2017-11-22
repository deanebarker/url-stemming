using System.ComponentModel;

namespace UrlStemming.Core
{
    public enum TrailingSlashes
    {
        [Description("Always Strip")]
        AlwaysStrip,

        [Description("Always Add")]
        AlwaysAdd,

        Ignore
    }
}
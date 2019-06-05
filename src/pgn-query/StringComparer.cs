namespace pgn_query
{
    public class StringComparer
    {
        public bool Compare(string source, string comparison)
        {
            if(string.IsNullOrEmpty(comparison)) return true;
            source = source.ToLower();
            switch (comparison.ToLower())
            {
                case string t when t.StartsWith("^"):
                    return source.StartsWith(comparison.Substring(1));
                case string t when t.StartsWith("*"):
                    return source.Contains(comparison.Substring(1));
                case string t when t.EndsWith("$"):
                    return source.EndsWith(comparison.Substring(0, comparison.Length - 1));
                default:
                    return !string.IsNullOrEmpty(comparison) && source.Equals(comparison);
            }
        }

    }
}
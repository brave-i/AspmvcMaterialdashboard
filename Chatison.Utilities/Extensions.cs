namespace Chatison.Utilities
{
    public static class Extensions
    {
        public static string RemovePhoneMask(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            return input.Replace("(", "")
                .Replace(")", "")
                .Replace("-", "")
                .Replace(" ", "")
                .Trim();
        }
    }
}

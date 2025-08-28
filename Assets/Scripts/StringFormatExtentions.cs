namespace UI
{
    public static class StringFormatExtentions
    {
        private const string CURRENCY_SYMBOL = "$";
        private const string PERCENTAGE_SYMBOL = "%";
        private const string PLUS_SYMBOL = "+";
        private const string COLON_SYMBOL = ":";
        private const string QUOTE_SYMBOL = "\"";

        public static string AsCurrency(this float value)
        {
            if (value >= 1000000000f)
                return (value / 1000000000f).ToString("F1") + "B" + CURRENCY_SYMBOL;
            
            if (value >= 1000000f)
                return (value / 1000000f).ToString("F1") + "M" + CURRENCY_SYMBOL;
            
            if (value >= 1000f)
                return (value / 1000f).ToString("F1") + "K" + CURRENCY_SYMBOL;
            
            return value.ToString("F0") + CURRENCY_SYMBOL;
        }

        public static string AsCurrency(this int value)
        {
            return AsCurrency((float)value);
        }

        public static string AsIncomeBonus(this float percentage)
        {
            return PLUS_SYMBOL + percentage.ToString("F0") + PERCENTAGE_SYMBOL;
        }

        public static string AsIncomeBonus(this int percentage)
        {
            return PLUS_SYMBOL + percentage.ToString() + PERCENTAGE_SYMBOL;
        }

        public static string InQuotes(this string text)
        {
            return QUOTE_SYMBOL + text + QUOTE_SYMBOL;
        }

        public static string AsLabel(this string text)
        {
            return text + COLON_SYMBOL;
        }
    }
}
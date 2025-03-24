namespace NLPC.PCMS.Common.Extensions
{
    public static class DecimalExtensions
    {
        public static double ToDouble(this decimal value)
        {
            return Convert.ToDouble(value);
        }
    }

}

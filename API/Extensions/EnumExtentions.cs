namespace API.Extensions
{
    /// <summary>
    /// adds index to IEnumerable for use in foreach loop
    /// </summary>
    public static class EnumExtension
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
           => self.Select((item, index) => (item, index));
    }
}

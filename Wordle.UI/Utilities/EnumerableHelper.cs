namespace Wordle.UI.Utilities;

public static class EnumerableHelper
{
    public static IEnumerable<TElement> RepeatUnique<TElement>(Func<TElement> factory, int count)
    {
        if (count < 1)
        {
            throw new ArgumentException($"Count is less than 1", nameof(count));
        }

        while (count-- > 0)
        {
            yield return factory();
        }
    }
}
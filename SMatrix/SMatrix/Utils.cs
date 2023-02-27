using System.Globalization;

namespace SMatrix;

internal static class Utils
{
    /// <summary>
    /// Reads a one-line sequence of values from a reader.
    /// The length of the sequence may be asserted.
    /// Extra info about line index may be added to exception messsages.
    /// </summary>
    /// <param name="reader">Reader to read the line from</param>
    /// <param name="parseValue">Function to parse each word into type T</param>
    /// <param name="lineIndex">The index of the line the reader was on before the read.
    /// If distinct from -1, is included into exception messages.
    /// Doesn't affect the return value</param>
    /// <param name="expectedLength">The expected length of the read sequence of values.
    /// If the actual length is different, an exception is thrown.
    /// This assertion may be skipped by using -1 as the parameter value.</param>
    /// <typeparam name="T">The type of the sequence values</typeparam>
    /// <returns>The read and parsed sequence</returns>
    /// <exception cref="ArgumentException">The reader is empty</exception>
    /// <exception cref="FormatException">The length of the read sequence != expectedLength</exception>
    public static T[] ReadVector<T>(TextReader reader,
                                    Func<string, T> parseValue,
                                    int expectedLength = -1,
                                    int lineIndex = -1)
    {
        // StringSplitOptions.RemoveEmptyEntries = 1
        // StringSplitOptions.TrimEntries = 2
        const StringSplitOptions options = (StringSplitOptions)(1 + 2);
        T[] xs = reader.ReadLine()
                     ?
                    .Split(new[] { ' ', '\t' }, options)
                       .Select(parseValue)
                       .ToArray()
              ?? throw new ArgumentException(
                     lineIndex == -1
                         ? "The reader is empty"
                         : $"Expected to read {lineIndex}th line, but reader is empty",
                     nameof(reader));
        if (expectedLength != -1 && xs.Length != expectedLength)
        {
            var prefix = lineIndex == -1 ? "" : $" {lineIndex}th";
            throw new FormatException(
                $"The{prefix} line expected to contain {expectedLength} values, but got {xs.Length}");
        }
        return xs;
    }

    public static double ParseDouble(string s)
    {
        return double.Parse(s, NumberStyles.Number, CultureInfo.InvariantCulture);
    }

    public static string DoubleToString(double x)
    {
        return x.ToString(CultureInfo.InvariantCulture);
    }
}

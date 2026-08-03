using System.Globalization;
using System.Text;

namespace ModularPipelines.SourceGenerator;

internal static class GeneratedTypeName
{
    public static string FromAssembly(
        string? assemblyName,
        string fallbackName,
        string suffix)
    {
        var name = assemblyName ?? fallbackName;
        var builder = new StringBuilder(name.Length + suffix.Length + 1);
        var wasSanitized = false;

        if (name.Length == 0 || !IsIdentifierStart(name[0]))
        {
            builder.Append('_');
            wasSanitized = true;
        }

        foreach (var character in name)
        {
            if (IsIdentifierPart(character))
            {
                builder.Append(character);
            }
            else
            {
                builder.Append('_');
                wasSanitized = true;
            }
        }

        if (wasSanitized)
        {
            builder.Append('_');
            builder.Append(ComputeStableHash(name).ToString("x16", CultureInfo.InvariantCulture));
        }

        return builder.Append(suffix).ToString();
    }

    private static bool IsIdentifierStart(char character) =>
        character == '_' || char.IsLetter(character);

    private static bool IsIdentifierPart(char character) =>
        character == '_' || char.IsLetterOrDigit(character);

    private static ulong ComputeStableHash(string value)
    {
        const ulong offsetBasis = 14695981039346656037;
        const ulong prime = 1099511628211;
        var hash = offsetBasis;

        foreach (var character in value)
        {
            hash ^= character;
            hash *= prime;
        }

        return hash;
    }
}

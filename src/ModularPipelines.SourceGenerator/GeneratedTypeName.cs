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

        if (name.Length == 0 || !IsIdentifierStart(name[0]))
        {
            builder.Append('_');
        }

        foreach (var character in name)
        {
            builder.Append(IsIdentifierPart(character) ? character : '_');
        }

        return builder.Append(suffix).ToString();
    }

    private static bool IsIdentifierStart(char character) =>
        character == '_' || char.IsLetter(character);

    private static bool IsIdentifierPart(char character) =>
        character == '_' || char.IsLetterOrDigit(character);
}

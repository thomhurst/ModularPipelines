namespace ModularPipelines.Distributed;

/// <summary>
/// Validates identifiers that distributed backends embed in keys, channels and URLs.
/// </summary>
internal static class DistributedIdentifier
{
    internal const int MaximumLength = 128;

    internal const string AllowedCharactersDescription =
        "ASCII letters, digits, '.', '_' and '-'";

    public static bool IsValid(string? value)
    {
        if (string.IsNullOrEmpty(value) || value.Length > MaximumLength)
        {
            return false;
        }

        foreach (var character in value)
        {
            if (!(char.IsAsciiLetterOrDigit(character) || character is '.' or '_' or '-'))
            {
                return false;
            }
        }

        return true;
    }

    public static void ThrowIfInvalid(string? value, string description)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(
                $"{description} '{value}' is invalid. It must contain 1-{MaximumLength} "
                + $"{AllowedCharactersDescription}.",
                nameof(value));
        }
    }
}

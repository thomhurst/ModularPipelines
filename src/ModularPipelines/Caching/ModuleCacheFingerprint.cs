namespace ModularPipelines.Caching;

/// <summary>
/// Validates module cache fingerprints used by cache store implementations.
/// </summary>
public static class ModuleCacheFingerprint
{
    /// <summary>
    /// Validates that a fingerprint is a SHA-256 hexadecimal value.
    /// </summary>
    /// <param name="fingerprint">The fingerprint to validate.</param>
    /// <exception cref="ArgumentException">Thrown when the fingerprint is not a SHA-256 hexadecimal value.</exception>
    public static void Validate(string fingerprint)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fingerprint);
        if (fingerprint.Length != 64 || fingerprint.Any(character => !Uri.IsHexDigit(character)))
        {
            throw new ArgumentException(
                "A module cache fingerprint must be a 64-character SHA-256 value.",
                nameof(fingerprint));
        }
    }
}

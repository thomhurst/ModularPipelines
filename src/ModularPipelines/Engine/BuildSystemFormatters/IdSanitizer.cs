namespace ModularPipelines.Engine.BuildSystemFormatters;

/// <summary>
/// Provides common ID sanitization logic for build system formatters.
/// </summary>
/// <remarks>
/// Many build systems require identifiers (section IDs, fold IDs) to be alphanumeric.
/// This utility centralizes that logic to avoid duplication across formatters.
/// </remarks>
internal static class IdSanitizer
{
    /// <summary>
    /// Sanitizes a name to a valid identifier for CI/CD section IDs.
    /// </summary>
    /// <param name="name">The name to sanitize.</param>
    /// <returns>A lowercase identifier containing only letters, digits, and underscores.</returns>
    /// <example>
    /// <code>
    /// var id = IdSanitizer.ToSectionId("Build Step (Release)");
    /// // Returns: "build_step_release"
    /// </code>
    /// </example>
    public static string ToSectionId(string name)
    {
        // Section IDs must be alphanumeric with underscores, lowercase
        return string.Concat(name.Where(c => char.IsLetterOrDigit(c) || c == '_')).ToLowerInvariant();
    }
}

namespace ModularPipelines.Helpers;

/// <summary>
/// Provides utility methods for escaping text for use in Spectre.Console markup.
/// </summary>
internal static class SpectreMarkupEscaper
{
    /// <summary>
    /// Escapes text for safe use within Spectre.Console markup.
    /// This prevents characters like [ and ] from being interpreted as markup tags.
    /// </summary>
    /// <param name="text">The text to escape.</param>
    /// <returns>The escaped text safe for use in Spectre.Console markup.</returns>
    public static string Escape(string? text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return string.Empty;
        }

        // Spectre.Console markup uses [ and ] for tags.
        // To display literal [ and ], they must be doubled: [[ and ]]
        return text.Replace("[", "[[").Replace("]", "]]");
    }
}

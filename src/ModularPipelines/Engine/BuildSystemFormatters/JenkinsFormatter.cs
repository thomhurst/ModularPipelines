namespace ModularPipelines.Engine.BuildSystemFormatters;

/// <summary>
/// Formatter for Jenkins build system.
/// Supports console annotations for log parsing and secret masking via credentials plugin.
/// </summary>
/// <remarks>
/// Jenkins does not have native collapsible sections in the same way as GitHub/GitLab.
/// However, plugins like "Timestamper" and "AnsiColor" enhance console output.
/// Secret masking is handled by the Credentials Binding plugin automatically.
/// </remarks>
/// <example>
/// <code>
/// // Jenkins doesn't have native collapsible blocks, but we can use visual markers
/// // ===== Build Step =====
/// // ... log content ...
/// // ===== End Build Step =====
/// </code>
/// </example>
internal class JenkinsFormatter : IBuildSystemFormatter
{
    public string? GetStartBlockCommand(string name)
    {
        // Jenkins doesn't support collapsible sections natively
        // Return null to use default behavior
        return null;
    }

    public string? GetEndBlockCommand(string name)
    {
        // Jenkins doesn't support collapsible sections natively
        return null;
    }

    public string? GetMaskSecretCommand(string secret)
    {
        // Jenkins Credentials Binding plugin handles masking automatically
        // No runtime command available
        return null;
    }
}
namespace ModularPipelines.Engine.BuildSystemFormatters;

/// <summary>
/// Formatter for GitLab CI/CD build system.
/// Supports collapsible log sections and variable masking.
/// </summary>
/// <remarks>
/// GitLab uses special escape sequences for collapsible sections.
/// Documentation: https://docs.gitlab.com/ee/ci/jobs/#custom-collapsible-sections
/// Secret masking is automatic via masked variables, but can be controlled via CI_DEBUG_TRACE.
/// </remarks>
/// <example>
/// <code>
/// // Example output:
/// // \e[0Ksection_start:1234567890:build_step\r\e[0KBuild Step
/// // ... log content ...
/// // \e[0Ksection_end:1234567890:build_step\r\e[0K
/// </code>
/// </example>
internal class GitLabFormatter : IBuildSystemFormatter
{
    public string GetStartBlockCommand(string name)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var sectionId = SanitizeSectionId(name);
        return $"\x1b[0Ksection_start:{timestamp}:{sectionId}\r\x1b[0K{name}";
    }

    public string GetEndBlockCommand(string name)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var sectionId = SanitizeSectionId(name);
        return $"\x1b[0Ksection_end:{timestamp}:{sectionId}\r\x1b[0K";
    }

    public string? GetMaskSecretCommand(string secret)
    {
        // GitLab automatically masks variables marked as masked in CI/CD settings
        // There's no runtime command to mask arbitrary values
        return null;
    }

    private static string SanitizeSectionId(string name)
    {
        // Section IDs must be alphanumeric with underscores
        return string.Concat(name.Where(c => char.IsLetterOrDigit(c) || c == '_')).ToLowerInvariant();
    }
}
namespace ModularPipelines.Engine.BuildSystemFormatters;

/// <summary>
/// Formatter for Bitbucket Pipelines build system.
/// Limited support for advanced logging features.
/// </summary>
/// <remarks>
/// Bitbucket Pipelines does not currently support collapsible log sections or runtime secret masking.
/// Secrets must be configured in repository settings and are automatically masked.
/// </remarks>
internal class BitbucketFormatter : IBuildSystemFormatter
{
    public string? GetStartBlockCommand(string name) => null;

    public string? GetEndBlockCommand(string name) => null;

    public string? GetMaskSecretCommand(string secret) => null;
}

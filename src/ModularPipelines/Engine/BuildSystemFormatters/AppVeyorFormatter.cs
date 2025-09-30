namespace ModularPipelines.Engine.BuildSystemFormatters;

/// <summary>
/// Formatter for AppVeyor build system.
/// Limited support for advanced logging features.
/// </summary>
/// <remarks>
/// AppVeyor does not currently support collapsible log sections or runtime secret masking.
/// Secrets configured in project settings are automatically masked in logs.
/// </remarks>
internal class AppVeyorFormatter : IBuildSystemFormatter
{
    public string? GetStartBlockCommand(string name) => null;

    public string? GetEndBlockCommand(string name) => null;

    public string? GetMaskSecretCommand(string secret) => null;
}
using ModularPipelines.Helpers;

namespace ModularPipelines.Engine.BuildSystemFormatters;

/// <summary>
/// Default formatter for unknown or local build systems.
/// Provides visual markers using icons instead of build system-specific commands.
/// </summary>
/// <remarks>
/// Used when no specific build system is detected or for local development.
/// Uses MarkupFormatter icons for visual consistency with Spectre.Console output.
/// </remarks>
internal class DefaultFormatter : IBuildSystemFormatter
{
    public string GetStartBlockCommand(string name) => $"{MarkupFormatter.PlayIcon} {name}";

    public string? GetEndBlockCommand(string name) => null;

    public string? GetMaskSecretCommand(string secret) => null;
}
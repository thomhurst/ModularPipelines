using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "print-settings", "npm")]
public record GcloudArtifactsPrintSettingsNpmOptions : GcloudOptions
{
    [CommandSwitch("--json-key")]
    public string? JsonKey { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }
}
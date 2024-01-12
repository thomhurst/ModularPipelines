using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "print-settings", "mvn")]
public record GcloudArtifactsPrintSettingsMvnOptions : GcloudOptions
{
    [CommandSwitch("--json-key")]
    public string? JsonKey { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "print-settings", "gradle")]
public record GcloudArtifactsPrintSettingsGradleOptions : GcloudOptions
{
    [CliOption("--json-key")]
    public string? JsonKey { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }
}
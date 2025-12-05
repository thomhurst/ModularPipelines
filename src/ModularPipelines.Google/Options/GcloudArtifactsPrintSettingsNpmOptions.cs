using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "print-settings", "npm")]
public record GcloudArtifactsPrintSettingsNpmOptions : GcloudOptions
{
    [CliOption("--json-key")]
    public string? JsonKey { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }
}
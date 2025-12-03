using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "files", "list")]
public record GcloudArtifactsFilesListOptions : GcloudOptions
{
    [CliOption("--package")]
    public string? Package { get; set; }

    [CliOption("--tag")]
    public string? Tag { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }
}
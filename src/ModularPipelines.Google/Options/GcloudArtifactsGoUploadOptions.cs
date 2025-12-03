using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "go", "upload")]
public record GcloudArtifactsGoUploadOptions : GcloudOptions
{
    public GcloudArtifactsGoUploadOptions(
        string modulePath,
        string version
    )
    {
        ModulePath = modulePath;
        Version = version;
    }

    [CliOption("--module-path")]
    public string ModulePath { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }
}

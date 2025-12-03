using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "apt", "import")]
public record GcloudArtifactsAptImportOptions(
[property: CliArgument] string Repository,
[property: CliArgument] string Location,
[property: CliOption("--gcs-source")] string[] GcsSource
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}
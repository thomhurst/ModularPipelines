using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "apt", "upload")]
public record GcloudArtifactsAptUploadOptions(
[property: CliArgument] string Repository,
[property: CliArgument] string Location,
[property: CliOption("--source")] string Source
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}
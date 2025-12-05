using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "yum", "upload")]
public record GcloudArtifactsYumUploadOptions(
[property: CliArgument] string Repository,
[property: CliArgument] string Location,
[property: CliOption("--source")] string Source
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}
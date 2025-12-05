using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "packages", "delete")]
public record GcloudArtifactsPackagesDeleteOptions(
[property: CliArgument] string Package,
[property: CliArgument] string Location,
[property: CliArgument] string Repository
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}
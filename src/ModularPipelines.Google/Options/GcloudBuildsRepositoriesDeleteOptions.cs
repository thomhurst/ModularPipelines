using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "repositories", "delete")]
public record GcloudBuildsRepositoriesDeleteOptions(
[property: CliArgument] string Repository,
[property: CliArgument] string Connection,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}
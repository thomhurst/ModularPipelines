using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "repositories", "create")]
public record GcloudBuildsRepositoriesCreateOptions(
[property: CliArgument] string Repository,
[property: CliArgument] string Connection,
[property: CliArgument] string Region,
[property: CliOption("--remote-uri")] string RemoteUri
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}
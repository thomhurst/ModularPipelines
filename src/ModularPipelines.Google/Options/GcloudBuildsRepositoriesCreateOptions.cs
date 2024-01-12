using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "repositories", "create")]
public record GcloudBuildsRepositoriesCreateOptions(
[property: PositionalArgument] string Repository,
[property: PositionalArgument] string Connection,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--remote-uri")] string RemoteUri
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}
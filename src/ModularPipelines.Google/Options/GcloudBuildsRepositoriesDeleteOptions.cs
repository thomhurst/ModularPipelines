using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "repositories", "delete")]
public record GcloudBuildsRepositoriesDeleteOptions(
[property: PositionalArgument] string Repository,
[property: PositionalArgument] string Connection,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}
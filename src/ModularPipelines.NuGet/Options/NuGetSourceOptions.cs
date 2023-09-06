using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.NuGet.Options;

[CommandPrecedingArguments("nuget", "add", "source")]
[ExcludeFromCodeCoverage]
public record NuGetSourceOptions
(
    [property: PositionalArgument]
    Uri FeedUri,

    [property: CommandSwitch("--name")]
    string Name
) : NuGetOptions
{
    [CommandSwitch("--username")]
    public string? Username { get; init; }

    [CommandSwitch("--password")]
    public string? Password { get; init; }
}

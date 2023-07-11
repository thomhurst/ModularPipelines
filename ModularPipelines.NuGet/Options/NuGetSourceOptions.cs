using ModularPipelines.Attributes;

namespace ModularPipelines.NuGet.Options;

[CommandPrecedingArguments("nuget", "add", "source")]
public record NuGetSourceOptions
(
    [property: PositionalArgument]
    Uri FeedUri,

    [property: CommandSwitch("n")]
    string Name
) : NuGetOptions
{

    [CommandEqualsSeparatorSwitch("--username", SwitchValueSeparator = " ")]
    public string? Username { get; init; }

    [CommandEqualsSeparatorSwitch("--password", SwitchValueSeparator = " ")]
    public string? Password { get; init; }
}

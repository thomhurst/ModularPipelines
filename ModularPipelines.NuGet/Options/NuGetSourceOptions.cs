using ModularPipelines.Attributes;

namespace ModularPipelines.NuGet.Options;

[CommandPrecedingArguments("nuget", "add", "source")]
public record NuGetSourceOptions
(
    Uri FeedUri,

    [property: CommandSwitch("n")]
    string Name
) : NuGetOptions
{

    [CommandLongSwitch("username", SwitchValueSeparator = " ")]
    public string? Username { get; init; }

    [CommandLongSwitch("password", SwitchValueSeparator = " ")]
    public string? Password { get; init; }
}

using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.NuGet.Options;

public record NuGetSourceOptions
(
    Uri FeedUri,

    [property: CommandSwitch("n")]
    string Name
) : CommandLineOptions
{

    [CommandLongSwitch("username", SwitchValueSeparator = " ")]
    public string? Username { get; init; }

    [CommandLongSwitch("password", SwitchValueSeparator = " ")]
    public string? Password { get; init; }
}

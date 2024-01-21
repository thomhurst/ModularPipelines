using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetWatchOptions : DotNetOptions
{
    public DotNetWatchOptions(
        string command,
        string forwardedArguments
    )
    {
        CommandParts = ["watch", "[<command>]"];

        Command = command;

        ForwardedArguments = forwardedArguments;
    }

    public DotNetWatchOptions(
        string forwardedArguments
    )
    {
        CommandParts = ["watch", "[<command>]"];

        ForwardedArguments = forwardedArguments;
    }

    [PositionalArgument(PlaceholderName = "[<command>]")]
    public string? Command { get; set; }

    [BooleanCommandSwitch("--list")]
    public bool? List { get; set; }

    [BooleanCommandSwitch("--no-hot-reload")]
    public bool? NoHotReload { get; set; }

    [BooleanCommandSwitch("--non-interactive")]
    public bool? NonInteractive { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--version")]
    public bool? Version { get; set; }

    [PositionalArgument(PlaceholderName = "<forwarded arguments>")]
    public string? ForwardedArguments { get; set; }
}

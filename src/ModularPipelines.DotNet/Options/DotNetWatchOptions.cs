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
    public virtual bool? List { get; set; }

    [BooleanCommandSwitch("--no-hot-reload")]
    public virtual bool? NoHotReload { get; set; }

    [BooleanCommandSwitch("--non-interactive")]
    public virtual bool? NonInteractive { get; set; }

    [CommandSwitch("--project")]
    public virtual string? Project { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--version")]
    public virtual bool? Version { get; set; }

    [PositionalArgument(PlaceholderName = "<forwarded arguments>")]
    public string? ForwardedArguments { get; set; }
}

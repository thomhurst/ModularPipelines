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

    [CliArgument(Name = "[<command>]")]
    public string? Command { get; set; }

    [CliFlag("--list")]
    public virtual bool? List { get; set; }

    [CliFlag("--no-hot-reload")]
    public virtual bool? NoHotReload { get; set; }

    [CliFlag("--non-interactive")]
    public virtual bool? NonInteractive { get; set; }

    [CliOption("--project")]
    public virtual string? Project { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--version")]
    public virtual bool? Version { get; set; }

    [CliArgument(Name = "<forwarded arguments>")]
    public string? ForwardedArguments { get; set; }
}

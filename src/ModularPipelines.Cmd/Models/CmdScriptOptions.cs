using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Cmd.Models;

[ExcludeFromCodeCoverage]
public record CmdScriptOptions([property: CliArgument(Placement = ArgumentPlacement.AfterOptions)] string Script) : CommandLineToolOptions("cmd")
{
    [CliFlag("/q")]
    public virtual bool DisableEcho { get; init; }

    [CliFlag("/c")]
    public virtual bool StopAfter { get; init; } = true;

    [CliFlag("/u")]
    public virtual bool Unicode { get; init; }

    [CliFlag("/a")]
    public virtual bool Ansi { get; init; }

    [CliFlag("/d")]
    public virtual bool DisableAutoRunCommands { get; init; }
}
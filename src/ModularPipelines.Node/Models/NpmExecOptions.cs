using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("exec")]
public record NpmExecOptions(
    [property: CliArgument(0, Phase = CommandLinePhase.Passthrough, PrependOptionTerminator = true)] string Value,
    [property: CliArgument(1, Phase = CommandLinePhase.Passthrough)] string Cmd
) : NpmOptions
{
    [CliOption("--package")]
    public virtual string[]? Package { get; set; }

    [CliOption("--call")]
    public virtual string? Call { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CliFlag("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [CliFlag("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [CliArgument(2, Phase = CommandLinePhase.Passthrough)]
    public virtual string? Pkg { get; set; }

    [CliArgument(3, Phase = CommandLinePhase.Passthrough)]
    public virtual string? Version { get; set; }
}

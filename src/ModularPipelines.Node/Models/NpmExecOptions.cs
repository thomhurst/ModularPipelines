using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("exec")]
public record NpmExecOptions(
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string Value,
    [property: CliArgument(Phase = CommandLinePhase.EarlyOperand)] string Cmd
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

    [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
    public virtual string? Pkg { get; set; }

    [CliArgument(Phase = CommandLinePhase.EarlyOperand)]
    public virtual string? Version { get; set; }
}

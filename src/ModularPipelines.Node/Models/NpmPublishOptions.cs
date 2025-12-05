using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("publish")]
public record NpmPublishOptions : NpmOptions
{
    [CliOption("--tag")]
    public virtual string? Tag { get; set; }

    [CliOption("--access")]
    public virtual string? Access { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliOption("--otp")]
    public virtual string? Otp { get; set; }

    [CliOption("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [CliFlag("--workspaces")]
    public virtual bool? Workspaces { get; set; }

    [CliFlag("--include-workspace-root")]
    public virtual bool? IncludeWorkspaceRoot { get; set; }

    [CliFlag("--provenance")]
    public virtual bool? Provenance { get; set; }

    [CliOption("--provenance-file")]
    public virtual string? ProvenanceFile { get; set; }
}
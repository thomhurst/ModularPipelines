using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("publish")]
public record NpmPublishOptions : NpmOptions
{
    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--access")]
    public string? Access { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [CommandSwitch("--otp")]
    public string? Otp { get; set; }

    [CommandSwitch("--workspace")]
    public string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public bool? Workspaces { get; set; }

    [BooleanCommandSwitch("--include-workspace-root")]
    public bool? IncludeWorkspaceRoot { get; set; }

    [BooleanCommandSwitch("--provenance")]
    public bool? Provenance { get; set; }

    [CommandSwitch("--provenance-file")]
    public string? ProvenanceFile { get; set; }

}
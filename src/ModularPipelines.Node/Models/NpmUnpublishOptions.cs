using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("unpublish")]
public record NpmUnpublishOptions : NpmOptions
{
    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [CommandSwitch("--workspace")]
    public virtual string[]? Workspace { get; set; }

    [BooleanCommandSwitch("--workspaces")]
    public virtual bool? Workspaces { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "vmware", "admin-clusters", "list")]
public record GcloudContainerVmwareAdminClustersListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}
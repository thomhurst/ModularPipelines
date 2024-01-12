using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "vmware", "clusters", "list")]
public record GcloudContainerVmwareClustersListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}
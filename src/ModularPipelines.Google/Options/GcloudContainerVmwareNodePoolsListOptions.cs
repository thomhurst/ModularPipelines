using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "vmware", "node-pools", "list")]
public record GcloudContainerVmwareNodePoolsListOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;
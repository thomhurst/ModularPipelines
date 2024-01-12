using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "azure", "node-pools", "list")]
public record GcloudContainerAzureNodePoolsListOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;
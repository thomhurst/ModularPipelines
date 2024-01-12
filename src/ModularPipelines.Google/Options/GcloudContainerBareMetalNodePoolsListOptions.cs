using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "bare-metal", "node-pools", "list")]
public record GcloudContainerBareMetalNodePoolsListOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;
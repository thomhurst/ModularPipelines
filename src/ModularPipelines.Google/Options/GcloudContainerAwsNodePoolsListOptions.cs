using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "aws", "node-pools", "list")]
public record GcloudContainerAwsNodePoolsListOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;
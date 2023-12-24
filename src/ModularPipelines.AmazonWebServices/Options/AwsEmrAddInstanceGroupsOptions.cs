using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "add-instance-groups")]
public record AwsEmrAddInstanceGroupsOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--instance-groups")] string[] InstanceGroups
) : AwsOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "add-instance-groups")]
public record AwsEmrAddInstanceGroupsOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--instance-groups")] string[] InstanceGroups
) : AwsOptions;
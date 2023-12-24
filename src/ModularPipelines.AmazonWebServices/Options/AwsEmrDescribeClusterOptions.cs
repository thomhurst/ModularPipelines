using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "describe-cluster")]
public record AwsEmrDescribeClusterOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId
) : AwsOptions;
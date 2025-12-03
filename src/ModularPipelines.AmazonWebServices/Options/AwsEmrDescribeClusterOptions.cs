using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "describe-cluster")]
public record AwsEmrDescribeClusterOptions(
[property: CliOption("--cluster-id")] string ClusterId
) : AwsOptions;
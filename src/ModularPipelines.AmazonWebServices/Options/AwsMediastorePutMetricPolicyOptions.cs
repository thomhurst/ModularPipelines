using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediastore", "put-metric-policy")]
public record AwsMediastorePutMetricPolicyOptions(
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--metric-policy")] string MetricPolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
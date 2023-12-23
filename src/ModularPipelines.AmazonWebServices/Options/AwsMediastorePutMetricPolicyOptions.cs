using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediastore", "put-metric-policy")]
public record AwsMediastorePutMetricPolicyOptions(
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--metric-policy")] string MetricPolicy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
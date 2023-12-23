using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "get-health-check-last-failure-reason")]
public record AwsRoute53GetHealthCheckLastFailureReasonOptions(
[property: CommandSwitch("--health-check-id")] string HealthCheckId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
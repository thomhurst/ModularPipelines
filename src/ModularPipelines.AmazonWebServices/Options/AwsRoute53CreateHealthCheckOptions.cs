using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "create-health-check")]
public record AwsRoute53CreateHealthCheckOptions(
[property: CommandSwitch("--caller-reference")] string CallerReference,
[property: CommandSwitch("--health-check-config")] string HealthCheckConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
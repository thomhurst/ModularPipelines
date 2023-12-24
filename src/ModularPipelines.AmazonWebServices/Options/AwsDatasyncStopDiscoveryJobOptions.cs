using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "stop-discovery-job")]
public record AwsDatasyncStopDiscoveryJobOptions(
[property: CommandSwitch("--discovery-job-arn")] string DiscoveryJobArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
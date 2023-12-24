using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "update-discovery-job")]
public record AwsDatasyncUpdateDiscoveryJobOptions(
[property: CommandSwitch("--discovery-job-arn")] string DiscoveryJobArn,
[property: CommandSwitch("--collection-duration-minutes")] int CollectionDurationMinutes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
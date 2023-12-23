using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "generate-recommendations")]
public record AwsDatasyncGenerateRecommendationsOptions(
[property: CommandSwitch("--discovery-job-arn")] string DiscoveryJobArn,
[property: CommandSwitch("--resource-ids")] string[] ResourceIds,
[property: CommandSwitch("--resource-type")] string ResourceType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
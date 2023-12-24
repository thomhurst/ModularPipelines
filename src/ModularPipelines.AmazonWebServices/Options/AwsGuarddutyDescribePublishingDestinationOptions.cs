using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "describe-publishing-destination")]
public record AwsGuarddutyDescribePublishingDestinationOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--destination-id")] string DestinationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
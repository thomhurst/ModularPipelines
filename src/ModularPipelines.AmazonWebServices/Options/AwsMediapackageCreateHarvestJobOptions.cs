using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediapackage", "create-harvest-job")]
public record AwsMediapackageCreateHarvestJobOptions(
[property: CommandSwitch("--end-time")] string EndTime,
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--origin-endpoint-id")] string OriginEndpointId,
[property: CommandSwitch("--s3-destination")] string S3Destination,
[property: CommandSwitch("--start-time")] string StartTime
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
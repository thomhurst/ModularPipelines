using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-contact-schedule")]
public record AwsConnectUpdateContactScheduleOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--contact-id")] string ContactId,
[property: CliOption("--scheduled-time")] long ScheduledTime
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
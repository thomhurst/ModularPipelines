using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-instance-event-start-time")]
public record AwsEc2ModifyInstanceEventStartTimeOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--instance-event-id")] string InstanceEventId,
[property: CliOption("--not-before")] long NotBefore
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-availability-zone-group")]
public record AwsEc2ModifyAvailabilityZoneGroupOptions(
[property: CliOption("--group-name")] string GroupName,
[property: CliOption("--opt-in-status")] string OptInStatus
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
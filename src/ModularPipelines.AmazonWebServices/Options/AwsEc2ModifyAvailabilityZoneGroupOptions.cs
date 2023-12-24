using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-availability-zone-group")]
public record AwsEc2ModifyAvailabilityZoneGroupOptions(
[property: CommandSwitch("--group-name")] string GroupName,
[property: CommandSwitch("--opt-in-status")] string OptInStatus
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
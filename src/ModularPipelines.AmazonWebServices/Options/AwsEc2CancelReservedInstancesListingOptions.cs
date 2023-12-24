using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "cancel-reserved-instances-listing")]
public record AwsEc2CancelReservedInstancesListingOptions(
[property: CommandSwitch("--reserved-instances-listing-id")] string ReservedInstancesListingId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
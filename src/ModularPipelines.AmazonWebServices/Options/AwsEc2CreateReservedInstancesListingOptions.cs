using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-reserved-instances-listing")]
public record AwsEc2CreateReservedInstancesListingOptions(
[property: CommandSwitch("--client-token")] string ClientToken,
[property: CommandSwitch("--instance-count")] int InstanceCount,
[property: CommandSwitch("--price-schedules")] string[] PriceSchedules,
[property: CommandSwitch("--reserved-instances-id")] string ReservedInstancesId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
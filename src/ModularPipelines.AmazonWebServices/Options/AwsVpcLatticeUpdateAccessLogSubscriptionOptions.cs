using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vpc-lattice", "update-access-log-subscription")]
public record AwsVpcLatticeUpdateAccessLogSubscriptionOptions(
[property: CommandSwitch("--access-log-subscription-identifier")] string AccessLogSubscriptionIdentifier,
[property: CommandSwitch("--destination-arn")] string DestinationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
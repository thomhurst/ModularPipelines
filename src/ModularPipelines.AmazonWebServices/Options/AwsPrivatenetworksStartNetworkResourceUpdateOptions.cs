using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privatenetworks", "start-network-resource-update")]
public record AwsPrivatenetworksStartNetworkResourceUpdateOptions(
[property: CommandSwitch("--network-resource-arn")] string NetworkResourceArn,
[property: CommandSwitch("--update-type")] string UpdateType
) : AwsOptions
{
    [CommandSwitch("--commitment-configuration")]
    public string? CommitmentConfiguration { get; set; }

    [CommandSwitch("--return-reason")]
    public string? ReturnReason { get; set; }

    [CommandSwitch("--shipping-address")]
    public string? ShippingAddress { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
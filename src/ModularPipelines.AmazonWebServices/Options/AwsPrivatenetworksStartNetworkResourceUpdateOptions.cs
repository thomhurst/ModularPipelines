using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privatenetworks", "start-network-resource-update")]
public record AwsPrivatenetworksStartNetworkResourceUpdateOptions(
[property: CliOption("--network-resource-arn")] string NetworkResourceArn,
[property: CliOption("--update-type")] string UpdateType
) : AwsOptions
{
    [CliOption("--commitment-configuration")]
    public string? CommitmentConfiguration { get; set; }

    [CliOption("--return-reason")]
    public string? ReturnReason { get; set; }

    [CliOption("--shipping-address")]
    public string? ShippingAddress { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
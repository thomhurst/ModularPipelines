using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "get-network-resource-relationships")]
public record AwsNetworkmanagerGetNetworkResourceRelationshipsOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId
) : AwsOptions
{
    [CliOption("--core-network-id")]
    public string? CoreNetworkId { get; set; }

    [CliOption("--registered-gateway-arn")]
    public string? RegisteredGatewayArn { get; set; }

    [CliOption("--aws-region")]
    public string? AwsRegion { get; set; }

    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
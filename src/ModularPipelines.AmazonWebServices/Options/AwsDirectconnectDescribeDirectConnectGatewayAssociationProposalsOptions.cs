using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "describe-direct-connect-gateway-association-proposals")]
public record AwsDirectconnectDescribeDirectConnectGatewayAssociationProposalsOptions : AwsOptions
{
    [CliOption("--direct-connect-gateway-id")]
    public string? DirectConnectGatewayId { get; set; }

    [CliOption("--proposal-id")]
    public string? ProposalId { get; set; }

    [CliOption("--associated-gateway-id")]
    public string? AssociatedGatewayId { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
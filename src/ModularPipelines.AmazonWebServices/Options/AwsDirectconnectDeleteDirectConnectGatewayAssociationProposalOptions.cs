using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "delete-direct-connect-gateway-association-proposal")]
public record AwsDirectconnectDeleteDirectConnectGatewayAssociationProposalOptions(
[property: CliOption("--proposal-id")] string ProposalId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
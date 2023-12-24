using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "describe-direct-connect-gateway-association-proposals")]
public record AwsDirectconnectDescribeDirectConnectGatewayAssociationProposalsOptions : AwsOptions
{
    [CommandSwitch("--direct-connect-gateway-id")]
    public string? DirectConnectGatewayId { get; set; }

    [CommandSwitch("--proposal-id")]
    public string? ProposalId { get; set; }

    [CommandSwitch("--associated-gateway-id")]
    public string? AssociatedGatewayId { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
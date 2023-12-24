using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "delete-direct-connect-gateway-association-proposal")]
public record AwsDirectconnectDeleteDirectConnectGatewayAssociationProposalOptions(
[property: CommandSwitch("--proposal-id")] string ProposalId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
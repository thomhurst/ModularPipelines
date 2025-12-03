using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "disassociate-connection-from-lag")]
public record AwsDirectconnectDisassociateConnectionFromLagOptions(
[property: CliOption("--connection-id")] string ConnectionId,
[property: CliOption("--lag-id")] string LagId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
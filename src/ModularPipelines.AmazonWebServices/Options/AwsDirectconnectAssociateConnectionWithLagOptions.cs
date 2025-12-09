using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "associate-connection-with-lag")]
public record AwsDirectconnectAssociateConnectionWithLagOptions(
[property: CliOption("--connection-id")] string ConnectionId,
[property: CliOption("--lag-id")] string LagId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
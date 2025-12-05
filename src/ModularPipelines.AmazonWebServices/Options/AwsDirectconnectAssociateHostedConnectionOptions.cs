using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "associate-hosted-connection")]
public record AwsDirectconnectAssociateHostedConnectionOptions(
[property: CliOption("--connection-id")] string ConnectionId,
[property: CliOption("--parent-connection-id")] string ParentConnectionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "allocate-hosted-connection")]
public record AwsDirectconnectAllocateHostedConnectionOptions(
[property: CliOption("--connection-id")] string ConnectionId,
[property: CliOption("--owner-account")] string OwnerAccount,
[property: CliOption("--bandwidth")] string Bandwidth,
[property: CliOption("--connection-name")] string ConnectionName,
[property: CliOption("--vlan")] int Vlan
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
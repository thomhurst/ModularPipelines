using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearch", "create-outbound-connection")]
public record AwsOpensearchCreateOutboundConnectionOptions(
[property: CliOption("--local-domain-info")] string LocalDomainInfo,
[property: CliOption("--remote-domain-info")] string RemoteDomainInfo,
[property: CliOption("--connection-alias")] string ConnectionAlias
) : AwsOptions
{
    [CliOption("--connection-mode")]
    public string? ConnectionMode { get; set; }

    [CliOption("--connection-properties")]
    public string? ConnectionProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
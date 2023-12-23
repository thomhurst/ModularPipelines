using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearch", "create-outbound-connection")]
public record AwsOpensearchCreateOutboundConnectionOptions(
[property: CommandSwitch("--local-domain-info")] string LocalDomainInfo,
[property: CommandSwitch("--remote-domain-info")] string RemoteDomainInfo,
[property: CommandSwitch("--connection-alias")] string ConnectionAlias
) : AwsOptions
{
    [CommandSwitch("--connection-mode")]
    public string? ConnectionMode { get; set; }

    [CommandSwitch("--connection-properties")]
    public string? ConnectionProperties { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
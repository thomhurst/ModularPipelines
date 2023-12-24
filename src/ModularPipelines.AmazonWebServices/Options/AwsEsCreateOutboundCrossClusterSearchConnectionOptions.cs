using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("es", "create-outbound-cross-cluster-search-connection")]
public record AwsEsCreateOutboundCrossClusterSearchConnectionOptions(
[property: CommandSwitch("--source-domain-info")] string SourceDomainInfo,
[property: CommandSwitch("--destination-domain-info")] string DestinationDomainInfo,
[property: CommandSwitch("--connection-alias")] string ConnectionAlias
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
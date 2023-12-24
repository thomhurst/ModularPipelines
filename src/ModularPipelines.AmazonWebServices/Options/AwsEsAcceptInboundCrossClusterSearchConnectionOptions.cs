using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("es", "accept-inbound-cross-cluster-search-connection")]
public record AwsEsAcceptInboundCrossClusterSearchConnectionOptions(
[property: CommandSwitch("--cross-cluster-search-connection-id")] string CrossClusterSearchConnectionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
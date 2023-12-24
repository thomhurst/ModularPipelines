using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("drs", "stop-source-network-replication")]
public record AwsDrsStopSourceNetworkReplicationOptions(
[property: CommandSwitch("--source-network-id")] string SourceNetworkId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
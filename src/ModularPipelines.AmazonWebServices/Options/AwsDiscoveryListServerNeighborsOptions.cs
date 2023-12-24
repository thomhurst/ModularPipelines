using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("discovery", "list-server-neighbors")]
public record AwsDiscoveryListServerNeighborsOptions(
[property: CommandSwitch("--configuration-id")] string ConfigurationId
) : AwsOptions
{
    [CommandSwitch("--neighbor-configuration-ids")]
    public string[]? NeighborConfigurationIds { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
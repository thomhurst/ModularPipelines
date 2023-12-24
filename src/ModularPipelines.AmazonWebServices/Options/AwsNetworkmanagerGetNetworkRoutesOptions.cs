using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "get-network-routes")]
public record AwsNetworkmanagerGetNetworkRoutesOptions(
[property: CommandSwitch("--global-network-id")] string GlobalNetworkId,
[property: CommandSwitch("--route-table-identifier")] string RouteTableIdentifier
) : AwsOptions
{
    [CommandSwitch("--exact-cidr-matches")]
    public string[]? ExactCidrMatches { get; set; }

    [CommandSwitch("--longest-prefix-matches")]
    public string[]? LongestPrefixMatches { get; set; }

    [CommandSwitch("--subnet-of-matches")]
    public string[]? SubnetOfMatches { get; set; }

    [CommandSwitch("--supernet-of-matches")]
    public string[]? SupernetOfMatches { get; set; }

    [CommandSwitch("--prefix-list-ids")]
    public string[]? PrefixListIds { get; set; }

    [CommandSwitch("--states")]
    public string[]? States { get; set; }

    [CommandSwitch("--types")]
    public string[]? Types { get; set; }

    [CommandSwitch("--destination-filters")]
    public IEnumerable<KeyValue>? DestinationFilters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
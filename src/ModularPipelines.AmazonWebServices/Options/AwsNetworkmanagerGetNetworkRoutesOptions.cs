using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "get-network-routes")]
public record AwsNetworkmanagerGetNetworkRoutesOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--route-table-identifier")] string RouteTableIdentifier
) : AwsOptions
{
    [CliOption("--exact-cidr-matches")]
    public string[]? ExactCidrMatches { get; set; }

    [CliOption("--longest-prefix-matches")]
    public string[]? LongestPrefixMatches { get; set; }

    [CliOption("--subnet-of-matches")]
    public string[]? SubnetOfMatches { get; set; }

    [CliOption("--supernet-of-matches")]
    public string[]? SupernetOfMatches { get; set; }

    [CliOption("--prefix-list-ids")]
    public string[]? PrefixListIds { get; set; }

    [CliOption("--states")]
    public string[]? States { get; set; }

    [CliOption("--types")]
    public string[]? Types { get; set; }

    [CliOption("--destination-filters")]
    public IEnumerable<KeyValue>? DestinationFilters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
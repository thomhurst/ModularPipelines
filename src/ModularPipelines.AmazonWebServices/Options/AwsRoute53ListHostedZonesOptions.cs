using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "list-hosted-zones")]
public record AwsRoute53ListHostedZonesOptions : AwsOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--delegation-set-id")]
    public string? DelegationSetId { get; set; }

    [CliOption("--hosted-zone-type")]
    public string? HostedZoneType { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("outposts", "list-outposts")]
public record AwsOutpostsListOutpostsOptions : AwsOptions
{
    [CliOption("--life-cycle-status-filter")]
    public string[]? LifeCycleStatusFilter { get; set; }

    [CliOption("--availability-zone-filter")]
    public string[]? AvailabilityZoneFilter { get; set; }

    [CliOption("--availability-zone-id-filter")]
    public string[]? AvailabilityZoneIdFilter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
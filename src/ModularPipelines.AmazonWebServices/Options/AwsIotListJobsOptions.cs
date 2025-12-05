using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "list-jobs")]
public record AwsIotListJobsOptions : AwsOptions
{
    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--target-selection")]
    public string? TargetSelection { get; set; }

    [CliOption("--thing-group-name")]
    public string? ThingGroupName { get; set; }

    [CliOption("--thing-group-id")]
    public string? ThingGroupId { get; set; }

    [CliOption("--namespace-id")]
    public string? NamespaceId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
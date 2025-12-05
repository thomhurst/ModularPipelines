using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resourcegroupstaggingapi", "get-compliance-summary")]
public record AwsResourcegroupstaggingapiGetComplianceSummaryOptions : AwsOptions
{
    [CliOption("--target-id-filters")]
    public string[]? TargetIdFilters { get; set; }

    [CliOption("--region-filters")]
    public string[]? RegionFilters { get; set; }

    [CliOption("--resource-type-filters")]
    public string[]? ResourceTypeFilters { get; set; }

    [CliOption("--tag-key-filters")]
    public string[]? TagKeyFilters { get; set; }

    [CliOption("--group-by")]
    public string[]? GroupBy { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resourcegroupstaggingapi", "get-resources")]
public record AwsResourcegroupstaggingapiGetResourcesOptions : AwsOptions
{
    [CliOption("--tag-filters")]
    public string[]? TagFilters { get; set; }

    [CliOption("--tags-per-page")]
    public int? TagsPerPage { get; set; }

    [CliOption("--resource-type-filters")]
    public string[]? ResourceTypeFilters { get; set; }

    [CliOption("--resource-arn-list")]
    public string[]? ResourceArnList { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
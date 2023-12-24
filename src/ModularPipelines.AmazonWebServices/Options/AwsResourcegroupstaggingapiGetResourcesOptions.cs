using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resourcegroupstaggingapi", "get-resources")]
public record AwsResourcegroupstaggingapiGetResourcesOptions : AwsOptions
{
    [CommandSwitch("--tag-filters")]
    public string[]? TagFilters { get; set; }

    [CommandSwitch("--tags-per-page")]
    public int? TagsPerPage { get; set; }

    [CommandSwitch("--resource-type-filters")]
    public string[]? ResourceTypeFilters { get; set; }

    [CommandSwitch("--resource-arn-list")]
    public string[]? ResourceArnList { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
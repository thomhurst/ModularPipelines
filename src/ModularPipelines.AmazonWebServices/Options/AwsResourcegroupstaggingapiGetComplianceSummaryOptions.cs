using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resourcegroupstaggingapi", "get-compliance-summary")]
public record AwsResourcegroupstaggingapiGetComplianceSummaryOptions : AwsOptions
{
    [CommandSwitch("--target-id-filters")]
    public string[]? TargetIdFilters { get; set; }

    [CommandSwitch("--region-filters")]
    public string[]? RegionFilters { get; set; }

    [CommandSwitch("--resource-type-filters")]
    public string[]? ResourceTypeFilters { get; set; }

    [CommandSwitch("--tag-key-filters")]
    public string[]? TagKeyFilters { get; set; }

    [CommandSwitch("--group-by")]
    public string[]? GroupBy { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "get-compliance-summary-by-resource-type")]
public record AwsConfigserviceGetComplianceSummaryByResourceTypeOptions : AwsOptions
{
    [CommandSwitch("--resource-types")]
    public string[]? ResourceTypes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
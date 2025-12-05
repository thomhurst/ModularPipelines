using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "get-compliance-summary-by-resource-type")]
public record AwsConfigserviceGetComplianceSummaryByResourceTypeOptions : AwsOptions
{
    [CliOption("--resource-types")]
    public string[]? ResourceTypes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
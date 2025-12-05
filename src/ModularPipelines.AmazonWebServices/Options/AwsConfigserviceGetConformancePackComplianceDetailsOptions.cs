using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "get-conformance-pack-compliance-details")]
public record AwsConfigserviceGetConformancePackComplianceDetailsOptions(
[property: CliOption("--conformance-pack-name")] string ConformancePackName
) : AwsOptions
{
    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "describe-fleet-advisor-lsa-analysis")]
public record AwsDmsDescribeFleetAdvisorLsaAnalysisOptions : AwsOptions
{
    [CliOption("--max-records")]
    public int? MaxRecords { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
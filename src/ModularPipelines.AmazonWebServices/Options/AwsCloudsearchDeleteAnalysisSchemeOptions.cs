using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudsearch", "delete-analysis-scheme")]
public record AwsCloudsearchDeleteAnalysisSchemeOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--analysis-scheme-name")] string AnalysisSchemeName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
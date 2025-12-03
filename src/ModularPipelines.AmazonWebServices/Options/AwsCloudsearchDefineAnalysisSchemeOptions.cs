using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudsearch", "define-analysis-scheme")]
public record AwsCloudsearchDefineAnalysisSchemeOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--analysis-scheme")] string AnalysisScheme
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
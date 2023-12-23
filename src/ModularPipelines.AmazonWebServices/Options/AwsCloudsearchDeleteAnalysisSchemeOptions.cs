using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudsearch", "delete-analysis-scheme")]
public record AwsCloudsearchDeleteAnalysisSchemeOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--analysis-scheme-name")] string AnalysisSchemeName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
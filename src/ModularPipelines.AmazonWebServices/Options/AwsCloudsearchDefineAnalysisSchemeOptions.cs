using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudsearch", "define-analysis-scheme")]
public record AwsCloudsearchDefineAnalysisSchemeOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--analysis-scheme")] string AnalysisScheme
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
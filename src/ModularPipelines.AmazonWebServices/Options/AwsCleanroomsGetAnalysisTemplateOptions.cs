using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "get-analysis-template")]
public record AwsCleanroomsGetAnalysisTemplateOptions(
[property: CliOption("--membership-identifier")] string MembershipIdentifier,
[property: CliOption("--analysis-template-identifier")] string AnalysisTemplateIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
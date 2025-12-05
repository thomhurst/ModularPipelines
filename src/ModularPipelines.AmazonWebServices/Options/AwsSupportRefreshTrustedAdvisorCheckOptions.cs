using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support", "refresh-trusted-advisor-check")]
public record AwsSupportRefreshTrustedAdvisorCheckOptions(
[property: CliOption("--check-id")] string CheckId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support", "describe-trusted-advisor-check-result")]
public record AwsSupportDescribeTrustedAdvisorCheckResultOptions(
[property: CliOption("--check-id")] string CheckId
) : AwsOptions
{
    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
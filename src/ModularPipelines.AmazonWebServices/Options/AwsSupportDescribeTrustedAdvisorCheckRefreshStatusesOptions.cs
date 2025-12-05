using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support", "describe-trusted-advisor-check-refresh-statuses")]
public record AwsSupportDescribeTrustedAdvisorCheckRefreshStatusesOptions(
[property: CliOption("--check-ids")] string[] CheckIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
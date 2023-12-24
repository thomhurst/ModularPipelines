using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "describe-trusted-advisor-check-summaries")]
public record AwsSupportDescribeTrustedAdvisorCheckSummariesOptions(
[property: CommandSwitch("--check-ids")] string[] CheckIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "describe-trusted-advisor-check-refresh-statuses")]
public record AwsSupportDescribeTrustedAdvisorCheckRefreshStatusesOptions(
[property: CommandSwitch("--check-ids")] string[] CheckIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
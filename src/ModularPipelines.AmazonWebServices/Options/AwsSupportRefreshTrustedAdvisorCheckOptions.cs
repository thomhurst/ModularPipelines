using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "refresh-trusted-advisor-check")]
public record AwsSupportRefreshTrustedAdvisorCheckOptions(
[property: CommandSwitch("--check-id")] string CheckId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
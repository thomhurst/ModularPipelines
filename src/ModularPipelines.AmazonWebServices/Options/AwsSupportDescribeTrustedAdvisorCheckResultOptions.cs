using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "describe-trusted-advisor-check-result")]
public record AwsSupportDescribeTrustedAdvisorCheckResultOptions(
[property: CommandSwitch("--check-id")] string CheckId
) : AwsOptions
{
    [CommandSwitch("--language")]
    public string? Language { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
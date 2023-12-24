using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops-guru", "describe-feedback")]
public record AwsDevopsGuruDescribeFeedbackOptions : AwsOptions
{
    [CommandSwitch("--insight-id")]
    public string? InsightId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
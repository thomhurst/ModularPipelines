using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops-guru", "put-feedback")]
public record AwsDevopsGuruPutFeedbackOptions : AwsOptions
{
    [CommandSwitch("--insight-feedback")]
    public string? InsightFeedback { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
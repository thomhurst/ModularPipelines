using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops-guru", "put-feedback")]
public record AwsDevopsGuruPutFeedbackOptions : AwsOptions
{
    [CliOption("--insight-feedback")]
    public string? InsightFeedback { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
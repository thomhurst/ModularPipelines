using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qconnect", "put-feedback")]
public record AwsQconnectPutFeedbackOptions(
[property: CliOption("--assistant-id")] string AssistantId,
[property: CliOption("--content-feedback")] string ContentFeedback,
[property: CliOption("--target-id")] string TargetId,
[property: CliOption("--target-type")] string TargetType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
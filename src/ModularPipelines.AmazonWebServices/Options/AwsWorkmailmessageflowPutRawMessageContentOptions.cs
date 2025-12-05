using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmailmessageflow", "put-raw-message-content")]
public record AwsWorkmailmessageflowPutRawMessageContentOptions(
[property: CliOption("--message-id")] string MessageId,
[property: CliOption("--content")] string Content
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
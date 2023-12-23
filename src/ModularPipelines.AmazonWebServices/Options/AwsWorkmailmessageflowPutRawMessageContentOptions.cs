using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmailmessageflow", "put-raw-message-content")]
public record AwsWorkmailmessageflowPutRawMessageContentOptions(
[property: CommandSwitch("--message-id")] string MessageId,
[property: CommandSwitch("--content")] string Content
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
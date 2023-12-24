using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "update-comment")]
public record AwsCodecommitUpdateCommentOptions(
[property: CommandSwitch("--comment-id")] string CommentId,
[property: CommandSwitch("--content")] string Content
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
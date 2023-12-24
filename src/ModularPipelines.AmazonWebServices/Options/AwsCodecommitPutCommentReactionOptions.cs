using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "put-comment-reaction")]
public record AwsCodecommitPutCommentReactionOptions(
[property: CommandSwitch("--comment-id")] string CommentId,
[property: CommandSwitch("--reaction-value")] string ReactionValue
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
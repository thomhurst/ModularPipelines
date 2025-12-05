using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "put-comment-reaction")]
public record AwsCodecommitPutCommentReactionOptions(
[property: CliOption("--comment-id")] string CommentId,
[property: CliOption("--reaction-value")] string ReactionValue
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
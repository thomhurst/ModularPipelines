using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "get-comment-reactions")]
public record AwsCodecommitGetCommentReactionsOptions(
[property: CommandSwitch("--comment-id")] string CommentId
) : AwsOptions
{
    [CommandSwitch("--reaction-user-arn")]
    public string? ReactionUserArn { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
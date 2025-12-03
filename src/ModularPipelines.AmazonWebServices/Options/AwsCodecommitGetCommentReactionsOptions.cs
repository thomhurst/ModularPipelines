using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "get-comment-reactions")]
public record AwsCodecommitGetCommentReactionsOptions(
[property: CliOption("--comment-id")] string CommentId
) : AwsOptions
{
    [CliOption("--reaction-user-arn")]
    public string? ReactionUserArn { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
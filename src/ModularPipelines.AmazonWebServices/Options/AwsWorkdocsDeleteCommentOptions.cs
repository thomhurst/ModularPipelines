using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "delete-comment")]
public record AwsWorkdocsDeleteCommentOptions(
[property: CommandSwitch("--document-id")] string DocumentId,
[property: CommandSwitch("--version-id")] string VersionId,
[property: CommandSwitch("--comment-id")] string CommentId
) : AwsOptions
{
    [CommandSwitch("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
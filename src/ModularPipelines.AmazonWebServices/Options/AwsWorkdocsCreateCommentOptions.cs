using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "create-comment")]
public record AwsWorkdocsCreateCommentOptions(
[property: CliOption("--document-id")] string DocumentId,
[property: CliOption("--version-id")] string VersionId,
[property: CliOption("--text")] string Text
) : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--parent-id")]
    public string? ParentId { get; set; }

    [CliOption("--thread-id")]
    public string? ThreadId { get; set; }

    [CliOption("--visibility")]
    public string? Visibility { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
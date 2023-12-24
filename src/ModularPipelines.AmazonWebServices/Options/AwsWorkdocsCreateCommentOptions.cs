using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "create-comment")]
public record AwsWorkdocsCreateCommentOptions(
[property: CommandSwitch("--document-id")] string DocumentId,
[property: CommandSwitch("--version-id")] string VersionId,
[property: CommandSwitch("--text")] string Text
) : AwsOptions
{
    [CommandSwitch("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CommandSwitch("--parent-id")]
    public string? ParentId { get; set; }

    [CommandSwitch("--thread-id")]
    public string? ThreadId { get; set; }

    [CommandSwitch("--visibility")]
    public string? Visibility { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
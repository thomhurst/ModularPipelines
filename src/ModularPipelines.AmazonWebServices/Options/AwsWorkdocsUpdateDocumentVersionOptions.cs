using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "update-document-version")]
public record AwsWorkdocsUpdateDocumentVersionOptions(
[property: CommandSwitch("--document-id")] string DocumentId,
[property: CommandSwitch("--version-id")] string VersionId
) : AwsOptions
{
    [CommandSwitch("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CommandSwitch("--version-status")]
    public string? VersionStatus { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
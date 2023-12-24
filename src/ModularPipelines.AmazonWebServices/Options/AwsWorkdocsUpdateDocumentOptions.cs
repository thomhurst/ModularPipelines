using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "update-document")]
public record AwsWorkdocsUpdateDocumentOptions(
[property: CommandSwitch("--document-id")] string DocumentId
) : AwsOptions
{
    [CommandSwitch("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--parent-folder-id")]
    public string? ParentFolderId { get; set; }

    [CommandSwitch("--resource-state")]
    public string? ResourceState { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
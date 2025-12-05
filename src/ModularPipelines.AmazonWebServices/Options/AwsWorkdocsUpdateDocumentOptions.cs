using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "update-document")]
public record AwsWorkdocsUpdateDocumentOptions(
[property: CliOption("--document-id")] string DocumentId
) : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--parent-folder-id")]
    public string? ParentFolderId { get; set; }

    [CliOption("--resource-state")]
    public string? ResourceState { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
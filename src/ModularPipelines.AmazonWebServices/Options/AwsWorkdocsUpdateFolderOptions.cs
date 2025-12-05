using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "update-folder")]
public record AwsWorkdocsUpdateFolderOptions(
[property: CliOption("--folder-id")] string FolderId
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
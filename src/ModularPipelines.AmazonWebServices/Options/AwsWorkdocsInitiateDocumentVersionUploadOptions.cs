using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "initiate-document-version-upload")]
public record AwsWorkdocsInitiateDocumentVersionUploadOptions : AwsOptions
{
    [CommandSwitch("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--content-created-timestamp")]
    public long? ContentCreatedTimestamp { get; set; }

    [CommandSwitch("--content-modified-timestamp")]
    public long? ContentModifiedTimestamp { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--document-size-in-bytes")]
    public long? DocumentSizeInBytes { get; set; }

    [CommandSwitch("--parent-folder-id")]
    public string? ParentFolderId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
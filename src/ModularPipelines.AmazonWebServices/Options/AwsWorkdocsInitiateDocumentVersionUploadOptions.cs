using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "initiate-document-version-upload")]
public record AwsWorkdocsInitiateDocumentVersionUploadOptions : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--content-created-timestamp")]
    public long? ContentCreatedTimestamp { get; set; }

    [CliOption("--content-modified-timestamp")]
    public long? ContentModifiedTimestamp { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--document-size-in-bytes")]
    public long? DocumentSizeInBytes { get; set; }

    [CliOption("--parent-folder-id")]
    public string? ParentFolderId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
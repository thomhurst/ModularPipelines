using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "abort-document-version-upload")]
public record AwsWorkdocsAbortDocumentVersionUploadOptions(
[property: CliOption("--document-id")] string DocumentId,
[property: CliOption("--version-id")] string VersionId
) : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
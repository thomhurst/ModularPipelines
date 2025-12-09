using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "update-document-version")]
public record AwsWorkdocsUpdateDocumentVersionOptions(
[property: CliOption("--document-id")] string DocumentId,
[property: CliOption("--version-id")] string VersionId
) : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--version-status")]
    public string? VersionStatus { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "get-document-version")]
public record AwsWorkdocsGetDocumentVersionOptions(
[property: CliOption("--document-id")] string DocumentId,
[property: CliOption("--version-id")] string VersionId
) : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--fields")]
    public string? Fields { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
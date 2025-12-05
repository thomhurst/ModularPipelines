using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "get-document-path")]
public record AwsWorkdocsGetDocumentPathOptions(
[property: CliOption("--document-id")] string DocumentId
) : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--fields")]
    public string? Fields { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
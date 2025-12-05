using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "update-notebook")]
public record AwsAthenaUpdateNotebookOptions(
[property: CliOption("--notebook-id")] string NotebookId,
[property: CliOption("--payload")] string Payload,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--session-id")]
    public string? SessionId { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
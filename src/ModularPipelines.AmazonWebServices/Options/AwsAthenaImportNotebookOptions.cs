using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "import-notebook")]
public record AwsAthenaImportNotebookOptions(
[property: CliOption("--work-group")] string WorkGroup,
[property: CliOption("--name")] string Name,
[property: CliOption("--payload")] string Payload,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "update-notebook-metadata")]
public record AwsAthenaUpdateNotebookMetadataOptions(
[property: CliOption("--notebook-id")] string NotebookId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
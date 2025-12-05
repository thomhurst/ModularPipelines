using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutvision", "create-dataset")]
public record AwsLookoutvisionCreateDatasetOptions(
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--dataset-type")] string DatasetType
) : AwsOptions
{
    [CliOption("--dataset-source")]
    public string? DatasetSource { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
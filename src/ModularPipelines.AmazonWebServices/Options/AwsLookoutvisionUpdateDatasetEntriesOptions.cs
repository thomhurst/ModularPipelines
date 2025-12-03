using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutvision", "update-dataset-entries")]
public record AwsLookoutvisionUpdateDatasetEntriesOptions(
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--dataset-type")] string DatasetType,
[property: CliOption("--changes")] string Changes
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
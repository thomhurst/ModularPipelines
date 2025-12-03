using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("forecast", "create-dataset")]
public record AwsForecastCreateDatasetOptions(
[property: CliOption("--dataset-name")] string DatasetName,
[property: CliOption("--domain")] string Domain,
[property: CliOption("--dataset-type")] string DatasetType,
[property: CliOption("--schema")] string Schema
) : AwsOptions
{
    [CliOption("--data-frequency")]
    public string? DataFrequency { get; set; }

    [CliOption("--encryption-config")]
    public string? EncryptionConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
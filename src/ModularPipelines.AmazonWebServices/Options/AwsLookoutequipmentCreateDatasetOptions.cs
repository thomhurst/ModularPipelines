using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "create-dataset")]
public record AwsLookoutequipmentCreateDatasetOptions(
[property: CliOption("--dataset-name")] string DatasetName
) : AwsOptions
{
    [CliOption("--dataset-schema")]
    public string? DatasetSchema { get; set; }

    [CliOption("--server-side-kms-key-id")]
    public string? ServerSideKmsKeyId { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
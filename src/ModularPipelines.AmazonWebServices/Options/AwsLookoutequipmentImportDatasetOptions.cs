using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "import-dataset")]
public record AwsLookoutequipmentImportDatasetOptions(
[property: CliOption("--source-dataset-arn")] string SourceDatasetArn
) : AwsOptions
{
    [CliOption("--dataset-name")]
    public string? DatasetName { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--server-side-kms-key-id")]
    public string? ServerSideKmsKeyId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
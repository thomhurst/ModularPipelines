using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "import-model-version")]
public record AwsLookoutequipmentImportModelVersionOptions(
[property: CliOption("--source-model-version-arn")] string SourceModelVersionArn,
[property: CliOption("--dataset-name")] string DatasetName
) : AwsOptions
{
    [CliOption("--model-name")]
    public string? ModelName { get; set; }

    [CliOption("--labels-input-configuration")]
    public string? LabelsInputConfiguration { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--server-side-kms-key-id")]
    public string? ServerSideKmsKeyId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--inference-data-import-strategy")]
    public string? InferenceDataImportStrategy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
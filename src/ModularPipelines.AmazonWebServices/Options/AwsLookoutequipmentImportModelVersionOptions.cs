using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "import-model-version")]
public record AwsLookoutequipmentImportModelVersionOptions(
[property: CommandSwitch("--source-model-version-arn")] string SourceModelVersionArn,
[property: CommandSwitch("--dataset-name")] string DatasetName
) : AwsOptions
{
    [CommandSwitch("--model-name")]
    public string? ModelName { get; set; }

    [CommandSwitch("--labels-input-configuration")]
    public string? LabelsInputConfiguration { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--server-side-kms-key-id")]
    public string? ServerSideKmsKeyId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--inference-data-import-strategy")]
    public string? InferenceDataImportStrategy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
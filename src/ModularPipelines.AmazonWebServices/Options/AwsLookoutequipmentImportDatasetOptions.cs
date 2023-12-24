using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "import-dataset")]
public record AwsLookoutequipmentImportDatasetOptions(
[property: CommandSwitch("--source-dataset-arn")] string SourceDatasetArn
) : AwsOptions
{
    [CommandSwitch("--dataset-name")]
    public string? DatasetName { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--server-side-kms-key-id")]
    public string? ServerSideKmsKeyId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "create-dataset")]
public record AwsForecastCreateDatasetOptions(
[property: CommandSwitch("--dataset-name")] string DatasetName,
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--dataset-type")] string DatasetType,
[property: CommandSwitch("--schema")] string Schema
) : AwsOptions
{
    [CommandSwitch("--data-frequency")]
    public string? DataFrequency { get; set; }

    [CommandSwitch("--encryption-config")]
    public string? EncryptionConfig { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
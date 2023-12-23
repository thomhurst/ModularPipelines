using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "create-dataset")]
public record AwsComprehendCreateDatasetOptions(
[property: CommandSwitch("--flywheel-arn")] string FlywheelArn,
[property: CommandSwitch("--dataset-name")] string DatasetName,
[property: CommandSwitch("--input-data-config")] string InputDataConfig
) : AwsOptions
{
    [CommandSwitch("--dataset-type")]
    public string? DatasetType { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
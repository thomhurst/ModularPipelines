using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "create-dataset")]
public record AwsComprehendCreateDatasetOptions(
[property: CliOption("--flywheel-arn")] string FlywheelArn,
[property: CliOption("--dataset-name")] string DatasetName,
[property: CliOption("--input-data-config")] string InputDataConfig
) : AwsOptions
{
    [CliOption("--dataset-type")]
    public string? DatasetType { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
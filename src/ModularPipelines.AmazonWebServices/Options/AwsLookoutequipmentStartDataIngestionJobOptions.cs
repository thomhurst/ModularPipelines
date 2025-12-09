using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "start-data-ingestion-job")]
public record AwsLookoutequipmentStartDataIngestionJobOptions(
[property: CliOption("--dataset-name")] string DatasetName,
[property: CliOption("--ingestion-input-configuration")] string IngestionInputConfiguration,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
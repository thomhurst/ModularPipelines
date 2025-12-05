using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-geospatial", "export-earth-observation-job")]
public record AwsSagemakerGeospatialExportEarthObservationJobOptions(
[property: CliOption("--arn")] string Arn,
[property: CliOption("--execution-role-arn")] string ExecutionRoleArn,
[property: CliOption("--output-config")] string OutputConfig
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
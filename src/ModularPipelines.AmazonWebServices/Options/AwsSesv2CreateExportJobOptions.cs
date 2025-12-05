using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "create-export-job")]
public record AwsSesv2CreateExportJobOptions(
[property: CliOption("--export-data-source")] string ExportDataSource,
[property: CliOption("--export-destination")] string ExportDestination
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
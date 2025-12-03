using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sesv2", "create-import-job")]
public record AwsSesv2CreateImportJobOptions(
[property: CliOption("--import-destination")] string ImportDestination,
[property: CliOption("--import-data-source")] string ImportDataSource
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
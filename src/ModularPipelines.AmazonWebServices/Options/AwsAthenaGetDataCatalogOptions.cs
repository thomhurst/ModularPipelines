using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "get-data-catalog")]
public record AwsAthenaGetDataCatalogOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--work-group")]
    public string? WorkGroup { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
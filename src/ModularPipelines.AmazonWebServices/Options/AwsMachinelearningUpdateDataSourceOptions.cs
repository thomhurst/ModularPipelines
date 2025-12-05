using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "update-data-source")]
public record AwsMachinelearningUpdateDataSourceOptions(
[property: CliOption("--data-source-id")] string DataSourceId,
[property: CliOption("--data-source-name")] string DataSourceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
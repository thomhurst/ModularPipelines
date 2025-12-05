using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "delete-data-source")]
public record AwsMachinelearningDeleteDataSourceOptions(
[property: CliOption("--data-source-id")] string DataSourceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("machinelearning", "create-data-source-from-s3")]
public record AwsMachinelearningCreateDataSourceFromS3Options(
[property: CliOption("--data-source-id")] string DataSourceId,
[property: CliOption("--data-spec")] string DataSpec
) : AwsOptions
{
    [CliOption("--data-source-name")]
    public string? DataSourceName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
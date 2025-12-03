using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "delete-data-source")]
public record AwsQbusinessDeleteDataSourceOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--data-source-id")] string DataSourceId,
[property: CliOption("--index-id")] string IndexId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "describe-data-source")]
public record AwsKendraDescribeDataSourceOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--index-id")] string IndexId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
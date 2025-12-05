using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearch", "update-data-source")]
public record AwsOpensearchUpdateDataSourceOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--name")] string Name,
[property: CliOption("--data-source-type")] string DataSourceType
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
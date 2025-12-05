using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "start-data-source-run")]
public record AwsDatazoneStartDataSourceRunOptions(
[property: CliOption("--data-source-identifier")] string DataSourceIdentifier,
[property: CliOption("--domain-identifier")] string DomainIdentifier
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
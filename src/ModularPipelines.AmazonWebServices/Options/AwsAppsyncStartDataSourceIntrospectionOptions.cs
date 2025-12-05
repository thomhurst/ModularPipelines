using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "start-data-source-introspection")]
public record AwsAppsyncStartDataSourceIntrospectionOptions : AwsOptions
{
    [CliOption("--rds-data-api-config")]
    public string? RdsDataApiConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
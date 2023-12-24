using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "start-data-source-introspection")]
public record AwsAppsyncStartDataSourceIntrospectionOptions : AwsOptions
{
    [CommandSwitch("--rds-data-api-config")]
    public string? RdsDataApiConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
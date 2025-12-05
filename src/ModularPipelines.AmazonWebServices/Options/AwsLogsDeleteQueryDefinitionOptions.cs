using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "delete-query-definition")]
public record AwsLogsDeleteQueryDefinitionOptions(
[property: CliOption("--query-definition-id")] string QueryDefinitionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
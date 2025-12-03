using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "put-query-definition")]
public record AwsLogsPutQueryDefinitionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--query-string")] string QueryString
) : AwsOptions
{
    [CliOption("--query-definition-id")]
    public string? QueryDefinitionId { get; set; }

    [CliOption("--log-group-names")]
    public string[]? LogGroupNames { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
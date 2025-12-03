using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "start-query-execution")]
public record AwsAthenaStartQueryExecutionOptions(
[property: CliOption("--query-string")] string QueryString
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--query-execution-context")]
    public string? QueryExecutionContext { get; set; }

    [CliOption("--result-configuration")]
    public string? ResultConfiguration { get; set; }

    [CliOption("--work-group")]
    public string? WorkGroup { get; set; }

    [CliOption("--execution-parameters")]
    public string[]? ExecutionParameters { get; set; }

    [CliOption("--result-reuse-configuration")]
    public string? ResultReuseConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
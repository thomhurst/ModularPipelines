using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "start-query-execution")]
public record AwsAthenaStartQueryExecutionOptions(
[property: CommandSwitch("--query-string")] string QueryString
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--query-execution-context")]
    public string? QueryExecutionContext { get; set; }

    [CommandSwitch("--result-configuration")]
    public string? ResultConfiguration { get; set; }

    [CommandSwitch("--work-group")]
    public string? WorkGroup { get; set; }

    [CommandSwitch("--execution-parameters")]
    public string[]? ExecutionParameters { get; set; }

    [CommandSwitch("--result-reuse-configuration")]
    public string? ResultReuseConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
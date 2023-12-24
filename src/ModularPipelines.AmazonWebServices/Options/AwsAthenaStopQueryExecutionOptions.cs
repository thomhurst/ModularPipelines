using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "stop-query-execution")]
public record AwsAthenaStopQueryExecutionOptions : AwsOptions
{
    [CommandSwitch("--query-execution-id")]
    public string? QueryExecutionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
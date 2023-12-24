using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "batch-get-query-execution")]
public record AwsAthenaBatchGetQueryExecutionOptions(
[property: CommandSwitch("--query-execution-ids")] string[] QueryExecutionIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
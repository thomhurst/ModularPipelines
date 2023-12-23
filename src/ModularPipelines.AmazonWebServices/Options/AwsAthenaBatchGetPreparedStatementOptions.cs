using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "batch-get-prepared-statement")]
public record AwsAthenaBatchGetPreparedStatementOptions(
[property: CommandSwitch("--prepared-statement-names")] string[] PreparedStatementNames,
[property: CommandSwitch("--work-group")] string WorkGroup
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "get-prepared-statement")]
public record AwsAthenaGetPreparedStatementOptions(
[property: CommandSwitch("--statement-name")] string StatementName,
[property: CommandSwitch("--work-group")] string WorkGroup
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
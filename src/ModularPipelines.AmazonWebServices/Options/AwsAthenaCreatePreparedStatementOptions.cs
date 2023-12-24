using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "create-prepared-statement")]
public record AwsAthenaCreatePreparedStatementOptions(
[property: CommandSwitch("--statement-name")] string StatementName,
[property: CommandSwitch("--work-group")] string WorkGroup,
[property: CommandSwitch("--query-statement")] string QueryStatement
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
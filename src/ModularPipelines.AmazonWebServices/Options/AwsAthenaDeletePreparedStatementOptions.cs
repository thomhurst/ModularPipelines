using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "delete-prepared-statement")]
public record AwsAthenaDeletePreparedStatementOptions(
[property: CommandSwitch("--statement-name")] string StatementName,
[property: CommandSwitch("--work-group")] string WorkGroup
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
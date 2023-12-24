using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("athena", "stop-calculation-execution")]
public record AwsAthenaStopCalculationExecutionOptions(
[property: CommandSwitch("--calculation-execution-id")] string CalculationExecutionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
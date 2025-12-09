using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "get-calculation-execution-code")]
public record AwsAthenaGetCalculationExecutionCodeOptions(
[property: CliOption("--calculation-execution-id")] string CalculationExecutionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "put-function-concurrency")]
public record AwsLambdaPutFunctionConcurrencyOptions(
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--reserved-concurrent-executions")] int ReservedConcurrentExecutions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
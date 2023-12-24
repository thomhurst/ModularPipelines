using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "put-function-concurrency")]
public record AwsLambdaPutFunctionConcurrencyOptions(
[property: CommandSwitch("--function-name")] string FunctionName,
[property: CommandSwitch("--reserved-concurrent-executions")] int ReservedConcurrentExecutions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "put-provisioned-concurrency-config")]
public record AwsLambdaPutProvisionedConcurrencyConfigOptions(
[property: CommandSwitch("--function-name")] string FunctionName,
[property: CommandSwitch("--qualifier")] string Qualifier,
[property: CommandSwitch("--provisioned-concurrent-executions")] int ProvisionedConcurrentExecutions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
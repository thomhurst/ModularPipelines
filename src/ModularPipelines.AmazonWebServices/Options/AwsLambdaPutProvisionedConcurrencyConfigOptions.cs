using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "put-provisioned-concurrency-config")]
public record AwsLambdaPutProvisionedConcurrencyConfigOptions(
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--qualifier")] string Qualifier,
[property: CliOption("--provisioned-concurrent-executions")] int ProvisionedConcurrentExecutions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
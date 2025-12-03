using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "get-provisioned-concurrency-config")]
public record AwsLambdaGetProvisionedConcurrencyConfigOptions(
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--qualifier")] string Qualifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
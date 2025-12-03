using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "delete-provisioned-concurrency-config")]
public record AwsLambdaDeleteProvisionedConcurrencyConfigOptions(
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--qualifier")] string Qualifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
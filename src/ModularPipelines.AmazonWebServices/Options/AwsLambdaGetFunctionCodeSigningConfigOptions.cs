using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "get-function-code-signing-config")]
public record AwsLambdaGetFunctionCodeSigningConfigOptions(
[property: CliOption("--function-name")] string FunctionName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
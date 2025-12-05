using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "put-function-code-signing-config")]
public record AwsLambdaPutFunctionCodeSigningConfigOptions(
[property: CliOption("--code-signing-config-arn")] string CodeSigningConfigArn,
[property: CliOption("--function-name")] string FunctionName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
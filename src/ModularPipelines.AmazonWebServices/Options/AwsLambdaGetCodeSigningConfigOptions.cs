using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "get-code-signing-config")]
public record AwsLambdaGetCodeSigningConfigOptions(
[property: CliOption("--code-signing-config-arn")] string CodeSigningConfigArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
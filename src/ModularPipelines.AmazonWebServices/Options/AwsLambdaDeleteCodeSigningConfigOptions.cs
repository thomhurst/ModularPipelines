using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "delete-code-signing-config")]
public record AwsLambdaDeleteCodeSigningConfigOptions(
[property: CliOption("--code-signing-config-arn")] string CodeSigningConfigArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
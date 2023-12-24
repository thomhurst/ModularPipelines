using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "delete-code-signing-config")]
public record AwsLambdaDeleteCodeSigningConfigOptions(
[property: CommandSwitch("--code-signing-config-arn")] string CodeSigningConfigArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
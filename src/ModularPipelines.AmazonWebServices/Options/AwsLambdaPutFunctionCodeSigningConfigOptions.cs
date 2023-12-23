using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "put-function-code-signing-config")]
public record AwsLambdaPutFunctionCodeSigningConfigOptions(
[property: CommandSwitch("--code-signing-config-arn")] string CodeSigningConfigArn,
[property: CommandSwitch("--function-name")] string FunctionName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
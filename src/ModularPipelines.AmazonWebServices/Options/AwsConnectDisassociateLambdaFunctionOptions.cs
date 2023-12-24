using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "disassociate-lambda-function")]
public record AwsConnectDisassociateLambdaFunctionOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--function-arn")] string FunctionArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
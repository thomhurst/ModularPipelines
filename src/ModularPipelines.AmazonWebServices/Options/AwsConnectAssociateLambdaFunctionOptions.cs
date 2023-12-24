using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "associate-lambda-function")]
public record AwsConnectAssociateLambdaFunctionOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--function-arn")] string FunctionArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
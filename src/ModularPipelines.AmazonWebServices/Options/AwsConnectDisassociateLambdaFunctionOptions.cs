using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "disassociate-lambda-function")]
public record AwsConnectDisassociateLambdaFunctionOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--function-arn")] string FunctionArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
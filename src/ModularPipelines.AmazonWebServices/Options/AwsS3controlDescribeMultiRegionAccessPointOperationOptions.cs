using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "describe-multi-region-access-point-operation")]
public record AwsS3controlDescribeMultiRegionAccessPointOperationOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--request-token-arn")] string RequestTokenArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
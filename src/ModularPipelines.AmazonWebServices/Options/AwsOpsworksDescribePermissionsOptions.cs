using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "describe-permissions")]
public record AwsOpsworksDescribePermissionsOptions : AwsOptions
{
    [CommandSwitch("--iam-user-arn")]
    public string? IamUserArn { get; set; }

    [CommandSwitch("--stack-id")]
    public string? StackId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
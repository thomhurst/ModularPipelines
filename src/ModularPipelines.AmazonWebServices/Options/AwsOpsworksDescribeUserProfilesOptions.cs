using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "describe-user-profiles")]
public record AwsOpsworksDescribeUserProfilesOptions : AwsOptions
{
    [CommandSwitch("--iam-user-arns")]
    public string[]? IamUserArns { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "describe-user-profiles")]
public record AwsOpsworksDescribeUserProfilesOptions : AwsOptions
{
    [CliOption("--iam-user-arns")]
    public string[]? IamUserArns { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
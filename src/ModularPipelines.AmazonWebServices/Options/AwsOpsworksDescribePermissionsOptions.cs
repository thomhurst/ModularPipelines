using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "describe-permissions")]
public record AwsOpsworksDescribePermissionsOptions : AwsOptions
{
    [CliOption("--iam-user-arn")]
    public string? IamUserArn { get; set; }

    [CliOption("--stack-id")]
    public string? StackId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "set-permission")]
public record AwsOpsworksSetPermissionOptions(
[property: CliOption("--stack-id")] string StackId,
[property: CliOption("--iam-user-arn")] string IamUserArn
) : AwsOptions
{
    [CliOption("--level")]
    public string? Level { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
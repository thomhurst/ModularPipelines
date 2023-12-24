using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "create-access-grants-location")]
public record AwsS3controlCreateAccessGrantsLocationOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--location-scope")] string LocationScope,
[property: CommandSwitch("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
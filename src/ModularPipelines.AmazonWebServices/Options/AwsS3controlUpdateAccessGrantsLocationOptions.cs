using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "update-access-grants-location")]
public record AwsS3controlUpdateAccessGrantsLocationOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--access-grants-location-id")] string AccessGrantsLocationId,
[property: CommandSwitch("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
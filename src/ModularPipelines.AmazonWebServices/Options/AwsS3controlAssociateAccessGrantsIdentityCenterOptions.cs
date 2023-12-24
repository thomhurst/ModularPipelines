using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "associate-access-grants-identity-center")]
public record AwsS3controlAssociateAccessGrantsIdentityCenterOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--identity-center-arn")] string IdentityCenterArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
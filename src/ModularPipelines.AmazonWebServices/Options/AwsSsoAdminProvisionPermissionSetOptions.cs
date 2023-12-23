using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "provision-permission-set")]
public record AwsSsoAdminProvisionPermissionSetOptions(
[property: CommandSwitch("--instance-arn")] string InstanceArn,
[property: CommandSwitch("--permission-set-arn")] string PermissionSetArn,
[property: CommandSwitch("--target-type")] string TargetType
) : AwsOptions
{
    [CommandSwitch("--target-id")]
    public string? TargetId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "update-permission-set")]
public record AwsSsoAdminUpdatePermissionSetOptions(
[property: CommandSwitch("--instance-arn")] string InstanceArn,
[property: CommandSwitch("--permission-set-arn")] string PermissionSetArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--relay-state")]
    public string? RelayState { get; set; }

    [CommandSwitch("--session-duration")]
    public string? SessionDuration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
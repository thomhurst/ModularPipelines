using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "create-permission-set")]
public record AwsSsoAdminCreatePermissionSetOptions(
[property: CommandSwitch("--instance-arn")] string InstanceArn,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--relay-state")]
    public string? RelayState { get; set; }

    [CommandSwitch("--session-duration")]
    public string? SessionDuration { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
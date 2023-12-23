using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "register")]
public record AwsOpsworksRegisterOptions(
[property: CommandSwitch("--stack-id")] string StackId,
[property: CommandSwitch("--infrastructure-class")] string InfrastructureClass
) : AwsOptions
{
    [CommandSwitch("--override-hostname")]
    public string? OverrideHostname { get; set; }

    [CommandSwitch("--override-private-ip")]
    public string? OverridePrivateIp { get; set; }

    [CommandSwitch("--override-public-ip")]
    public string? OverridePublicIp { get; set; }

    [CommandSwitch("--override-ssh")]
    public string? OverrideSsh { get; set; }

    [CommandSwitch("--ssh-username")]
    public string? SshUsername { get; set; }

    [CommandSwitch("--ssh-private-key")]
    public string? SshPrivateKey { get; set; }

    [BooleanCommandSwitch("--local")]
    public bool? Local { get; set; }

    [BooleanCommandSwitch("--use-instance-profile")]
    public bool? UseInstanceProfile { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "register")]
public record AwsOpsworksRegisterOptions(
[property: CliOption("--stack-id")] string StackId,
[property: CliOption("--infrastructure-class")] string InfrastructureClass
) : AwsOptions
{
    [CliOption("--override-hostname")]
    public string? OverrideHostname { get; set; }

    [CliOption("--override-private-ip")]
    public string? OverridePrivateIp { get; set; }

    [CliOption("--override-public-ip")]
    public string? OverridePublicIp { get; set; }

    [CliOption("--override-ssh")]
    public string? OverrideSsh { get; set; }

    [CliOption("--ssh-username")]
    public string? SshUsername { get; set; }

    [CliOption("--ssh-private-key")]
    public string? SshPrivateKey { get; set; }

    [CliFlag("--local")]
    public bool? Local { get; set; }

    [CliFlag("--use-instance-profile")]
    public bool? UseInstanceProfile { get; set; }
}
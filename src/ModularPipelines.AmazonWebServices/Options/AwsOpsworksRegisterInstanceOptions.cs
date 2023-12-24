using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "register-instance")]
public record AwsOpsworksRegisterInstanceOptions(
[property: CommandSwitch("--stack-id")] string StackId
) : AwsOptions
{
    [CommandSwitch("--hostname")]
    public string? Hostname { get; set; }

    [CommandSwitch("--public-ip")]
    public string? PublicIp { get; set; }

    [CommandSwitch("--private-ip")]
    public string? PrivateIp { get; set; }

    [CommandSwitch("--rsa-public-key")]
    public string? RsaPublicKey { get; set; }

    [CommandSwitch("--rsa-public-key-fingerprint")]
    public string? RsaPublicKeyFingerprint { get; set; }

    [CommandSwitch("--instance-identity")]
    public string? InstanceIdentity { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
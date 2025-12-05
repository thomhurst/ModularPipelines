using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "register-instance")]
public record AwsOpsworksRegisterInstanceOptions(
[property: CliOption("--stack-id")] string StackId
) : AwsOptions
{
    [CliOption("--hostname")]
    public string? Hostname { get; set; }

    [CliOption("--public-ip")]
    public string? PublicIp { get; set; }

    [CliOption("--private-ip")]
    public string? PrivateIp { get; set; }

    [CliOption("--rsa-public-key")]
    public string? RsaPublicKey { get; set; }

    [CliOption("--rsa-public-key-fingerprint")]
    public string? RsaPublicKeyFingerprint { get; set; }

    [CliOption("--instance-identity")]
    public string? InstanceIdentity { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
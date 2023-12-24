using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "delete-key-pair")]
public record AwsLightsailDeleteKeyPairOptions(
[property: CommandSwitch("--key-pair-name")] string KeyPairName
) : AwsOptions
{
    [CommandSwitch("--expected-fingerprint")]
    public string? ExpectedFingerprint { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
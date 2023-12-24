using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-verified-access-trust-provider")]
public record AwsEc2CreateVerifiedAccessTrustProviderOptions(
[property: CommandSwitch("--trust-provider-type")] string TrustProviderType,
[property: CommandSwitch("--policy-reference-name")] string PolicyReferenceName
) : AwsOptions
{
    [CommandSwitch("--user-trust-provider-type")]
    public string? UserTrustProviderType { get; set; }

    [CommandSwitch("--device-trust-provider-type")]
    public string? DeviceTrustProviderType { get; set; }

    [CommandSwitch("--oidc-options")]
    public string? OidcOptions { get; set; }

    [CommandSwitch("--device-options")]
    public string? DeviceOptions { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--sse-specification")]
    public string? SseSpecification { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
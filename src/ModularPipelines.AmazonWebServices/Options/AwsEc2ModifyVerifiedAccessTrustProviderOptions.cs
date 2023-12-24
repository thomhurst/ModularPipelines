using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-verified-access-trust-provider")]
public record AwsEc2ModifyVerifiedAccessTrustProviderOptions(
[property: CommandSwitch("--verified-access-trust-provider-id")] string VerifiedAccessTrustProviderId
) : AwsOptions
{
    [CommandSwitch("--oidc-options")]
    public string? OidcOptions { get; set; }

    [CommandSwitch("--device-options")]
    public string? DeviceOptions { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--sse-specification")]
    public string? SseSpecification { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
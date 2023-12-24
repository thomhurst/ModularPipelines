using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "attach-verified-access-trust-provider")]
public record AwsEc2AttachVerifiedAccessTrustProviderOptions(
[property: CommandSwitch("--verified-access-instance-id")] string VerifiedAccessInstanceId,
[property: CommandSwitch("--verified-access-trust-provider-id")] string VerifiedAccessTrustProviderId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
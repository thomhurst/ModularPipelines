using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "detach-verified-access-trust-provider")]
public record AwsEc2DetachVerifiedAccessTrustProviderOptions(
[property: CommandSwitch("--verified-access-instance-id")] string VerifiedAccessInstanceId,
[property: CommandSwitch("--verified-access-trust-provider-id")] string VerifiedAccessTrustProviderId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
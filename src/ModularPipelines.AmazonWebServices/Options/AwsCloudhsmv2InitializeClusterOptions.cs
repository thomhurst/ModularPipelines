using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudhsmv2", "initialize-cluster")]
public record AwsCloudhsmv2InitializeClusterOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--signed-cert")] string SignedCert,
[property: CommandSwitch("--trust-anchor")] string TrustAnchor
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
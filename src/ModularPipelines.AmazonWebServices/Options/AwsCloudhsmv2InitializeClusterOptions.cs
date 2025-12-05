using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudhsmv2", "initialize-cluster")]
public record AwsCloudhsmv2InitializeClusterOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--signed-cert")] string SignedCert,
[property: CliOption("--trust-anchor")] string TrustAnchor
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
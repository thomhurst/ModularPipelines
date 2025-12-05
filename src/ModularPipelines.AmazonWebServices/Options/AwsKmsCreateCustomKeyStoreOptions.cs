using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "create-custom-key-store")]
public record AwsKmsCreateCustomKeyStoreOptions(
[property: CliOption("--custom-key-store-name")] string CustomKeyStoreName
) : AwsOptions
{
    [CliOption("--cloud-hsm-cluster-id")]
    public string? CloudHsmClusterId { get; set; }

    [CliOption("--trust-anchor-certificate")]
    public string? TrustAnchorCertificate { get; set; }

    [CliOption("--key-store-password")]
    public string? KeyStorePassword { get; set; }

    [CliOption("--custom-key-store-type")]
    public string? CustomKeyStoreType { get; set; }

    [CliOption("--xks-proxy-uri-endpoint")]
    public string? XksProxyUriEndpoint { get; set; }

    [CliOption("--xks-proxy-uri-path")]
    public string? XksProxyUriPath { get; set; }

    [CliOption("--xks-proxy-vpc-endpoint-service-name")]
    public string? XksProxyVpcEndpointServiceName { get; set; }

    [CliOption("--xks-proxy-authentication-credential")]
    public string? XksProxyAuthenticationCredential { get; set; }

    [CliOption("--xks-proxy-connectivity")]
    public string? XksProxyConnectivity { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
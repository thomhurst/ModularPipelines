using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "create-custom-key-store")]
public record AwsKmsCreateCustomKeyStoreOptions(
[property: CommandSwitch("--custom-key-store-name")] string CustomKeyStoreName
) : AwsOptions
{
    [CommandSwitch("--cloud-hsm-cluster-id")]
    public string? CloudHsmClusterId { get; set; }

    [CommandSwitch("--trust-anchor-certificate")]
    public string? TrustAnchorCertificate { get; set; }

    [CommandSwitch("--key-store-password")]
    public string? KeyStorePassword { get; set; }

    [CommandSwitch("--custom-key-store-type")]
    public string? CustomKeyStoreType { get; set; }

    [CommandSwitch("--xks-proxy-uri-endpoint")]
    public string? XksProxyUriEndpoint { get; set; }

    [CommandSwitch("--xks-proxy-uri-path")]
    public string? XksProxyUriPath { get; set; }

    [CommandSwitch("--xks-proxy-vpc-endpoint-service-name")]
    public string? XksProxyVpcEndpointServiceName { get; set; }

    [CommandSwitch("--xks-proxy-authentication-credential")]
    public string? XksProxyAuthenticationCredential { get; set; }

    [CommandSwitch("--xks-proxy-connectivity")]
    public string? XksProxyConnectivity { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
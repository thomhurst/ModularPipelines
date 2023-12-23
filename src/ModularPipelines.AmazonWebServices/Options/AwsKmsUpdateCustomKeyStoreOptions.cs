using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "update-custom-key-store")]
public record AwsKmsUpdateCustomKeyStoreOptions(
[property: CommandSwitch("--custom-key-store-id")] string CustomKeyStoreId
) : AwsOptions
{
    [CommandSwitch("--new-custom-key-store-name")]
    public string? NewCustomKeyStoreName { get; set; }

    [CommandSwitch("--key-store-password")]
    public string? KeyStorePassword { get; set; }

    [CommandSwitch("--cloud-hsm-cluster-id")]
    public string? CloudHsmClusterId { get; set; }

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
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "create-server")]
public record AwsTransferCreateServerOptions : AwsOptions
{
    [CommandSwitch("--certificate")]
    public string? Certificate { get; set; }

    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--endpoint-details")]
    public string? EndpointDetails { get; set; }

    [CommandSwitch("--endpoint-type")]
    public string? EndpointType { get; set; }

    [CommandSwitch("--host-key")]
    public string? HostKey { get; set; }

    [CommandSwitch("--identity-provider-details")]
    public string? IdentityProviderDetails { get; set; }

    [CommandSwitch("--identity-provider-type")]
    public string? IdentityProviderType { get; set; }

    [CommandSwitch("--logging-role")]
    public string? LoggingRole { get; set; }

    [CommandSwitch("--post-authentication-login-banner")]
    public string? PostAuthenticationLoginBanner { get; set; }

    [CommandSwitch("--pre-authentication-login-banner")]
    public string? PreAuthenticationLoginBanner { get; set; }

    [CommandSwitch("--protocols")]
    public string[]? Protocols { get; set; }

    [CommandSwitch("--protocol-details")]
    public string? ProtocolDetails { get; set; }

    [CommandSwitch("--security-policy-name")]
    public string? SecurityPolicyName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--workflow-details")]
    public string? WorkflowDetails { get; set; }

    [CommandSwitch("--structured-log-destinations")]
    public string[]? StructuredLogDestinations { get; set; }

    [CommandSwitch("--s3-storage-options")]
    public string? S3StorageOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
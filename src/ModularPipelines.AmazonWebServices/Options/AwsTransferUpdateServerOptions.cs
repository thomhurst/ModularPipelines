using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "update-server")]
public record AwsTransferUpdateServerOptions(
[property: CliOption("--server-id")] string ServerId
) : AwsOptions
{
    [CliOption("--certificate")]
    public string? Certificate { get; set; }

    [CliOption("--protocol-details")]
    public string? ProtocolDetails { get; set; }

    [CliOption("--endpoint-details")]
    public string? EndpointDetails { get; set; }

    [CliOption("--endpoint-type")]
    public string? EndpointType { get; set; }

    [CliOption("--host-key")]
    public string? HostKey { get; set; }

    [CliOption("--identity-provider-details")]
    public string? IdentityProviderDetails { get; set; }

    [CliOption("--logging-role")]
    public string? LoggingRole { get; set; }

    [CliOption("--post-authentication-login-banner")]
    public string? PostAuthenticationLoginBanner { get; set; }

    [CliOption("--pre-authentication-login-banner")]
    public string? PreAuthenticationLoginBanner { get; set; }

    [CliOption("--protocols")]
    public string[]? Protocols { get; set; }

    [CliOption("--security-policy-name")]
    public string? SecurityPolicyName { get; set; }

    [CliOption("--workflow-details")]
    public string? WorkflowDetails { get; set; }

    [CliOption("--structured-log-destinations")]
    public string[]? StructuredLogDestinations { get; set; }

    [CliOption("--s3-storage-options")]
    public string? S3StorageOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearch", "create-domain")]
public record AwsOpensearchCreateDomainOptions(
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--cluster-config")]
    public string? ClusterConfig { get; set; }

    [CliOption("--ebs-options")]
    public string? EbsOptions { get; set; }

    [CliOption("--access-policies")]
    public string? AccessPolicies { get; set; }

    [CliOption("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CliOption("--snapshot-options")]
    public string? SnapshotOptions { get; set; }

    [CliOption("--vpc-options")]
    public string? VpcOptions { get; set; }

    [CliOption("--cognito-options")]
    public string? CognitoOptions { get; set; }

    [CliOption("--encryption-at-rest-options")]
    public string? EncryptionAtRestOptions { get; set; }

    [CliOption("--node-to-node-encryption-options")]
    public string? NodeToNodeEncryptionOptions { get; set; }

    [CliOption("--advanced-options")]
    public IEnumerable<KeyValue>? AdvancedOptions { get; set; }

    [CliOption("--log-publishing-options")]
    public IEnumerable<KeyValue>? LogPublishingOptions { get; set; }

    [CliOption("--domain-endpoint-options")]
    public string? DomainEndpointOptions { get; set; }

    [CliOption("--advanced-security-options")]
    public string? AdvancedSecurityOptions { get; set; }

    [CliOption("--tag-list")]
    public string[]? TagList { get; set; }

    [CliOption("--auto-tune-options")]
    public string? AutoTuneOptions { get; set; }

    [CliOption("--off-peak-window-options")]
    public string? OffPeakWindowOptions { get; set; }

    [CliOption("--software-update-options")]
    public string? SoftwareUpdateOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
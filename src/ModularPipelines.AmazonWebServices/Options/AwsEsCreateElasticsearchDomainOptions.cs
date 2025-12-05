using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "create-elasticsearch-domain")]
public record AwsEsCreateElasticsearchDomainOptions(
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--elasticsearch-version")]
    public string? ElasticsearchVersion { get; set; }

    [CliOption("--elasticsearch-cluster-config")]
    public string? ElasticsearchClusterConfig { get; set; }

    [CliOption("--ebs-options")]
    public string? EbsOptions { get; set; }

    [CliOption("--access-policies")]
    public string? AccessPolicies { get; set; }

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

    [CliOption("--auto-tune-options")]
    public string? AutoTuneOptions { get; set; }

    [CliOption("--tag-list")]
    public string[]? TagList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
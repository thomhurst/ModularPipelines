using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("es", "create-elasticsearch-domain")]
public record AwsEsCreateElasticsearchDomainOptions(
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--elasticsearch-version")]
    public string? ElasticsearchVersion { get; set; }

    [CommandSwitch("--elasticsearch-cluster-config")]
    public string? ElasticsearchClusterConfig { get; set; }

    [CommandSwitch("--ebs-options")]
    public string? EbsOptions { get; set; }

    [CommandSwitch("--access-policies")]
    public string? AccessPolicies { get; set; }

    [CommandSwitch("--snapshot-options")]
    public string? SnapshotOptions { get; set; }

    [CommandSwitch("--vpc-options")]
    public string? VpcOptions { get; set; }

    [CommandSwitch("--cognito-options")]
    public string? CognitoOptions { get; set; }

    [CommandSwitch("--encryption-at-rest-options")]
    public string? EncryptionAtRestOptions { get; set; }

    [CommandSwitch("--node-to-node-encryption-options")]
    public string? NodeToNodeEncryptionOptions { get; set; }

    [CommandSwitch("--advanced-options")]
    public IEnumerable<KeyValue>? AdvancedOptions { get; set; }

    [CommandSwitch("--log-publishing-options")]
    public IEnumerable<KeyValue>? LogPublishingOptions { get; set; }

    [CommandSwitch("--domain-endpoint-options")]
    public string? DomainEndpointOptions { get; set; }

    [CommandSwitch("--advanced-security-options")]
    public string? AdvancedSecurityOptions { get; set; }

    [CommandSwitch("--auto-tune-options")]
    public string? AutoTuneOptions { get; set; }

    [CommandSwitch("--tag-list")]
    public string[]? TagList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
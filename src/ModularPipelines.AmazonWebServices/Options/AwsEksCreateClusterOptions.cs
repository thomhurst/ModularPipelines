using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "create-cluster")]
public record AwsEksCreateClusterOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--resources-vpc-config")] string ResourcesVpcConfig
) : AwsOptions
{
    [CliOption("--kubernetes-network-config")]
    public string? KubernetesNetworkConfig { get; set; }

    [CliOption("--logging")]
    public string? Logging { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--encryption-config")]
    public string[]? EncryptionConfig { get; set; }

    [CliOption("--outpost-config")]
    public string? OutpostConfig { get; set; }

    [CliOption("--access-config")]
    public string? AccessConfig { get; set; }

    [CliOption("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
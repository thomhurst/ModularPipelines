using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "create-cluster")]
public record AwsEksCreateClusterOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--resources-vpc-config")] string ResourcesVpcConfig
) : AwsOptions
{
    [CommandSwitch("--kubernetes-network-config")]
    public string? KubernetesNetworkConfig { get; set; }

    [CommandSwitch("--logging")]
    public string? Logging { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--encryption-config")]
    public string[]? EncryptionConfig { get; set; }

    [CommandSwitch("--outpost-config")]
    public string? OutpostConfig { get; set; }

    [CommandSwitch("--access-config")]
    public string? AccessConfig { get; set; }

    [CommandSwitch("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr-containers", "create-managed-endpoint")]
public record AwsEmrContainersCreateManagedEndpointOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--virtual-cluster-id")] string VirtualClusterId,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--release-label")] string ReleaseLabel,
[property: CommandSwitch("--execution-role-arn")] string ExecutionRoleArn
) : AwsOptions
{
    [CommandSwitch("--certificate-arn")]
    public string? CertificateArn { get; set; }

    [CommandSwitch("--configuration-overrides")]
    public string? ConfigurationOverrides { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
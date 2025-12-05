using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr-containers", "create-managed-endpoint")]
public record AwsEmrContainersCreateManagedEndpointOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--virtual-cluster-id")] string VirtualClusterId,
[property: CliOption("--type")] string Type,
[property: CliOption("--release-label")] string ReleaseLabel,
[property: CliOption("--execution-role-arn")] string ExecutionRoleArn
) : AwsOptions
{
    [CliOption("--certificate-arn")]
    public string? CertificateArn { get; set; }

    [CliOption("--configuration-overrides")]
    public string? ConfigurationOverrides { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
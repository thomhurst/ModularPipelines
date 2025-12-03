using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aro", "validate")]
public record AzAroValidateOptions(
[property: CliOption("--master-subnet")] string MasterSubnet,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--worker-subnet")] string WorkerSubnet
) : AzOptions
{
    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--client-secret")]
    public string? ClientSecret { get; set; }

    [CliOption("--cluster-resource-group")]
    public string? ClusterResourceGroup { get; set; }

    [CliOption("--disk-encryption-set")]
    public string? DiskEncryptionSet { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--pod-cidr")]
    public string? PodCidr { get; set; }

    [CliOption("--service-cidr")]
    public string? ServiceCidr { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }

    [CliOption("--vnet-resource-group")]
    public string? VnetResourceGroup { get; set; }
}
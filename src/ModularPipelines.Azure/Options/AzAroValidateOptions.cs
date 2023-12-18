using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aro", "validate")]
public record AzAroValidateOptions(
[property: CommandSwitch("--master-subnet")] string MasterSubnet,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--worker-subnet")] string WorkerSubnet
) : AzOptions
{
    [CommandSwitch("--client-id")]
    public string? ClientId { get; set; }

    [CommandSwitch("--client-secret")]
    public string? ClientSecret { get; set; }

    [CommandSwitch("--cluster-resource-group")]
    public string? ClusterResourceGroup { get; set; }

    [CommandSwitch("--disk-encryption-set")]
    public string? DiskEncryptionSet { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--pod-cidr")]
    public string? PodCidr { get; set; }

    [CommandSwitch("--service-cidr")]
    public string? ServiceCidr { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }

    [CommandSwitch("--vnet-resource-group")]
    public string? VnetResourceGroup { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qumulo", "storage", "file-system", "create")]
public record AzQumuloStorageFileSystemCreateOptions(
[property: CommandSwitch("--admin-password")] string AdminPassword,
[property: CommandSwitch("--delegated-subnet-id")] string DelegatedSubnetId,
[property: CommandSwitch("--file-system-name")] string FileSystemName,
[property: CommandSwitch("--initial-capacity")] string InitialCapacity,
[property: CommandSwitch("--marketplace-details")] string MarketplaceDetails,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-sku")] string StorageSku,
[property: CommandSwitch("--user-details")] string UserDetails
) : AzOptions
{
    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--cluster-login-url")]
    public string? ClusterLoginUrl { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--private-ips")]
    public string? PrivateIps { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}


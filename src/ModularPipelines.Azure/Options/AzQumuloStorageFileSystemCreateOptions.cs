using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qumulo", "storage", "file-system", "create")]
public record AzQumuloStorageFileSystemCreateOptions(
[property: CliOption("--admin-password")] string AdminPassword,
[property: CliOption("--delegated-subnet-id")] string DelegatedSubnetId,
[property: CliOption("--file-system-name")] string FileSystemName,
[property: CliOption("--initial-capacity")] string InitialCapacity,
[property: CliOption("--marketplace-details")] string MarketplaceDetails,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--storage-sku")] string StorageSku,
[property: CliOption("--user-details")] string UserDetails
) : AzOptions
{
    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--cluster-login-url")]
    public string? ClusterLoginUrl { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-ips")]
    public string? PrivateIps { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}
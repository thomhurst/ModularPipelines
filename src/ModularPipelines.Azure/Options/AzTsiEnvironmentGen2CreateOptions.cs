using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tsi", "environment", "gen2", "create")]
public record AzTsiEnvironmentGen2CreateOptions(
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--id-properties")] string IdProperties,
[property: CliOption("--location")] string Location,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku,
[property: CliOption("--storage-config")] string StorageConfig
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--warm-store-config")]
    public string? WarmStoreConfig { get; set; }
}
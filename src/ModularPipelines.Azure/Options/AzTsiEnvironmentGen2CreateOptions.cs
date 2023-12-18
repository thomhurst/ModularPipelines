using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tsi", "environment", "gen2", "create")]
public record AzTsiEnvironmentGen2CreateOptions(
[property: CommandSwitch("--environment-name")] string EnvironmentName,
[property: CommandSwitch("--id-properties")] string IdProperties,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku,
[property: CommandSwitch("--storage-config")] string StorageConfig
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--warm-store-config")]
    public string? WarmStoreConfig { get; set; }
}
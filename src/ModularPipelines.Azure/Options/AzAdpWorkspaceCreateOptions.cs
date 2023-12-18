using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("adp", "workspace", "create")]
public record AzAdpWorkspaceCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--data-location")]
    public string? DataLocation { get; set; }

    [CommandSwitch("--direct-read-access")]
    public string? DirectReadAccess { get; set; }

    [CommandSwitch("--domain-name-scope")]
    public string? DomainNameScope { get; set; }

    [CommandSwitch("--encryption")]
    public string? Encryption { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--source-resource-id")]
    public string? SourceResourceId { get; set; }

    [CommandSwitch("--storage-account-count")]
    public int? StorageAccountCount { get; set; }

    [CommandSwitch("--storage-sku")]
    public string? StorageSku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}
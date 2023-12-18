using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "update")]
public record AzAcrUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--admin-enabled")]
    public bool? AdminEnabled { get; set; }

    [BooleanCommandSwitch("--allow-exports")]
    public bool? AllowExports { get; set; }

    [BooleanCommandSwitch("--allow-trusted-services")]
    public bool? AllowTrustedServices { get; set; }

    [BooleanCommandSwitch("--anonymous-pull-enabled")]
    public bool? AnonymousPullEnabled { get; set; }

    [BooleanCommandSwitch("--data-endpoint-enabled")]
    public bool? DataEndpointEnabled { get; set; }

    [CommandSwitch("--default-action")]
    public string? DefaultAction { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [BooleanCommandSwitch("--public-network-enabled")]
    public bool? PublicNetworkEnabled { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}
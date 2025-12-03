using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "update")]
public record AzAcrUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--admin-enabled")]
    public bool? AdminEnabled { get; set; }

    [CliFlag("--allow-exports")]
    public bool? AllowExports { get; set; }

    [CliFlag("--allow-trusted-services")]
    public bool? AllowTrustedServices { get; set; }

    [CliFlag("--anonymous-pull-enabled")]
    public bool? AnonymousPullEnabled { get; set; }

    [CliFlag("--data-endpoint-enabled")]
    public bool? DataEndpointEnabled { get; set; }

    [CliOption("--default-action")]
    public string? DefaultAction { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliFlag("--public-network-enabled")]
    public bool? PublicNetworkEnabled { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}
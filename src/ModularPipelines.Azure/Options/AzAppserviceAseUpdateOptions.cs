using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appservice", "ase", "update")]
public record AzAppserviceAseUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--allow-incoming-ftp-connections")]
    public bool? AllowIncomingFtpConnections { get; set; }

    [CliFlag("--allow-new-private-endpoint-connections")]
    public bool? AllowNewPrivateEndpointConnections { get; set; }

    [CliFlag("--allow-remote-debugging")]
    public bool? AllowRemoteDebugging { get; set; }

    [CliOption("--front-end-scale-factor")]
    public string? FrontEndScaleFactor { get; set; }

    [CliOption("--front-end-sku")]
    public string? FrontEndSku { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}